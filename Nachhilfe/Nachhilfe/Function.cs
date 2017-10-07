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
                return MakeSkillResponse("Für welches Fach möchtest du üben?", false, input.Session.Attributes);
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
                    input.Session.Attributes["State"] = eStates.SubjectChosser.ToString();
                    var resValueSubject = intentRequest.Intent.Slots["Subject"].Value;
                    input.Session.Attributes.Add("Subject", resValueSubject);
                    resultText = "Für welche Klasse möchtest du üben";
                    break;
                case "ClassChooser":
                    if (State != eStates.SubjectChosser)
                    {
                        break;
                    }
                    input.Session.Attributes["State"] = eStates.ClassChooser.ToString();
                    var resValueClass = intentRequest.Intent.Slots["Class"].Value;
                    input.Session.Attributes.Add("Class", resValueClass);
                    resultText = "Gut dann legen wir los ";
                    // todo in abhäng. von Class anderer Provider
                    resultText += DoNewMathExercise(input);

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
                            resultText = "Richtig ";                           
                        }
                        else
                        {
                            resultText = "Falsch die korrekte Antwort ist " + res.ToString();                         
                        }
                       
                        Object ExcersiceCounterObject;
                        var resb = input.Session.Attributes.TryGetValue("ExcersiceCounter", out ExcersiceCounterObject);
                        if (resb)
                        {
                            int ExcersiceCounter = Convert.ToInt32(ExcersiceCounterObject);
                            if (ExcersiceCounter >= 3)
                            {
                                return MakeSkillResponse("Super wir sind fertig", true, input.Session.Attributes);
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


            return e.GetQuestion();
        }

        private SkillResponse MakeSkillResponse(string outputSpeech, bool shouldEndSession, Dictionary<string, object> sessionAttributes, string repromptText = "Repromt")
        {
            var response = new ResponseBody
            {
                ShouldEndSession = shouldEndSession,

                OutputSpeech = new PlainTextOutputSpeech { Text = outputSpeech }
            };

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