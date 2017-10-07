using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Nachhilfe
{
    public class Function
    {

        enum eStates
        {
            Initial,
            SubjectChosser = 1,
            ClassChooser,
            TimeChooser,
            Result
        }


        private static HttpClient _httpClient;
        public const string INVOCATION_NAME = "Nachhilfe";

        public Function()
        {
            _httpClient = new HttpClient();
        }

        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            var resultText = "";

            //********************************************* State
            Object StateObject;
            if (input.Session.Attributes == null)
            {
                input.Session.Attributes = new Dictionary<string, object>();
                input.Session.Attributes.Add("State", eStates.Initial.ToString());
            }
            else if (!input.Session.Attributes.Any())
            {
                input.Session.Attributes.Add("State", eStates.Initial.ToString());
            }

            var x = input.Session.Attributes.TryGetValue("State", out StateObject);
            eStates State = eStates.Initial;
            if (x == true)
            {
                State = (eStates)Enum.Parse(typeof(eStates), StateObject.ToString());
            }
            //********************************************* 

            var requestType = input.GetRequestType();

            if (requestType == typeof(IntentRequest))
            {
                return DoIntentRequest(input, ref resultText, State);
            }
            else if (requestType == typeof(LaunchRequest))
            {
                return MakeSkillResponse("Welches Fach möchtest du üben?", false, input.Session.Attributes);
            }
            else if (requestType == typeof(SessionEndedRequest))
            {
                return MakeSkillResponse("Anwendung wurde unerwartet beendet", true, input.Session.Attributes);
            }
            else
            {
                return MakeSkillResponse($"Unbekannter requestType", true, input.Session.Attributes);
            }
        }

        private SkillResponse DoIntentRequest(SkillRequest input, ref string resultText, eStates State)
        {
            var intentRequest = input.Request as IntentRequest;

            switch (intentRequest.Intent.Name)
            {
                case "SubjectChooser":
                    if (State != eStates.Initial)
                    {
                        break;
                    }

                    var resValueSubject = intentRequest.Intent.Slots["Subject"].Value;

                    if (resValueSubject == "mathe")
                    {
                        input.Session.Attributes.Add("Subject", resValueSubject);
                        input.Session.Attributes["State"] = eStates.SubjectChosser.ToString();
                        resultText = "Für welche Klasse möchtest du üben";
                    }
                    else
                    {
                        input.Session.Attributes["State"] = eStates.Initial.ToString();
                        resultText = "Schulfach " + resValueSubject + " wird noch nicht unterstützt ";
                        resultText += "<break time = '1s' /> Welches Fach möchtest du üben?";
                    }

                    break;
                case "ClassChooser":
                    if (State != eStates.SubjectChosser)
                    {
                        break;
                    }

                    var resValueClass = intentRequest.Intent.Slots["Class"].Value;
                    //if (resValueClass == "klasse 1")
                    //{
                    input.Session.Attributes["State"] = eStates.ClassChooser.ToString();
                    input.Session.Attributes.Add("Class", resValueClass);
                    resultText = "<break time = '1s' /> Las uns anfange!";
                    resultText += DoNewMathExercise(input);
                    //}
                    //else
                    //{
                    //    input.Session.Attributes["State"] = eStates.SubjectChosser.ToString();
                    //    resultText = resValueClass + " wird noch nicht unterstützt ";
                    //    resultText += "<break time = '1s' /> Für welche Klasse möchtest du üben?";
                    //}


                    break;
                case "UserResponseMathe":
                    if (State != eStates.ClassChooser && State != eStates.Result)
                    {
                        break;
                    }
                    input.Session.Attributes["State"] = eStates.Result.ToString();

                    var resValueMath = intentRequest.Intent.Slots["Number"].Value;

                    Object res;
                    var cast = input.Session.Attributes.TryGetValue("MathObject", out res);
                    if (!cast)
                    {
                        resultText = "Fehler von uns ";
                    }
                    else
                    {


                        if (res.ToString() == resValueMath)
                        {

                            if (input.Session.Attributes.Keys.Contains("CountCorrect"))
                            {
                                var CountCorrectExercises = Convert.ToInt32(input.Session.Attributes["CountCorrect"]);
                                input.Session.Attributes["CountCorrect"] = CountCorrectExercises + 1;
                            }
                            else
                            {
                                input.Session.Attributes.Add("CountCorrect", 1);
                            }

                            resultText = "Richtig ";
                        }
                        else
                        {

                            if (input.Session.Attributes.Keys.Contains("CountFalse"))
                            {
                                var CountFalseExercises = Convert.ToInt32(input.Session.Attributes["CountFalse"]);
                                input.Session.Attributes["CountFalse"] = CountFalseExercises + 1;
                            }
                            else
                            {
                                input.Session.Attributes.Add("CountFalse", 1);
                            }

                            resultText = "Falsch die korrekte Antwort ist " + res.ToString();
                        }

                        Object ExcersiceCounterObject;
                        var resb = input.Session.Attributes.TryGetValue("ExcersiceCounter", out ExcersiceCounterObject);
                        if (resb)
                        {
                            int ExcersiceCounter = Convert.ToInt32(ExcersiceCounterObject);
                            if (ExcersiceCounter >= 3)
                            {
                                int positiv = 0;
                                int negativ = 0;
                            
                                if (input.Session.Attributes.Keys.Contains("CountCorrect"))
                                {
                                    positiv = Convert.ToInt32(input.Session.Attributes["CountCorrect"]);                                    
                                }
                                if (input.Session.Attributes.Keys.Contains("CountFalse"))
                                {
                                    negativ = Convert.ToInt32(input.Session.Attributes["CountFalse"]);                                   
                                }
                                int gesamt = positiv + negativ;
                                var Statistik = $" <break time = '0.5s' /> Du hast {positiv} von {gesamt} richtig beantwortet";

                                return MakeSkillResponse(resultText + "<break time = '0.5s' /> Super wir sind fertig" + Statistik, true, input.Session.Attributes);
                            }
                            else
                            {
                                resultText += DoNewMathExercise(input);
                            }
                        }
                    }
                    break;

                default:
                    return MakeSkillResponse("Beenden", true, input.Session.Attributes);
                    break;

            }
            return MakeSkillResponse(resultText, false, input.Session.Attributes);
        }

        private static string DoNewMathExercise(SkillRequest input)
        {
            var Math = new MathAdditionExerciseProvider(new Random(), 2, 1, 10);
            var e = Math.NextExercise();

            if (input.Session.Attributes.Keys.Contains("MathObject"))
            {
                input.Session.Attributes["MathObject"] = e.Solution();
            }
            else
            {
                input.Session.Attributes.Add("MathObject", e.Solution());
            }

            if (input.Session.Attributes.Keys.Contains("ExcersiceCounter"))
            {
                var excersiceCounterValue = Convert.ToInt32(input.Session.Attributes["ExcersiceCounter"]);
                input.Session.Attributes["ExcersiceCounter"] = excersiceCounterValue + 1;
            }
            else
            {
                input.Session.Attributes.Add("ExcersiceCounter", 1);
            }


            return "<break time = '0.5s' />" + e.GetQuestion();
        }

        private SkillResponse MakeSkillResponse(string outputSpeech, bool shouldEndSession, Dictionary<string, object> sessionAttributes, string repromptText = "Repromt")
        {
            var response = new ResponseBody
            {
                ShouldEndSession = shouldEndSession,

                OutputSpeech = new SsmlOutputSpeech
                {
                    Ssml = "<speak>" + outputSpeech + "</speak>"
                }

            };

            //     response.Response.OutputSpeech.Ssml = "<break ...\>"
            //response.Response.OutputSpeech.Ssml = output.ToString();
            //     response.Response.OutputSpeech.Type = "SSML";

            if (repromptText != null)
            {
                response.Reprompt = new Reprompt() { OutputSpeech = new PlainTextOutputSpeech() { Text = repromptText } };
            }

            var d = new Dictionary<string, object>();

            var skillResponse = new SkillResponse
            {
                Response = response,
                SessionAttributes = sessionAttributes,
                Version = "1.0"
            };

            return skillResponse;
        }
    }
}