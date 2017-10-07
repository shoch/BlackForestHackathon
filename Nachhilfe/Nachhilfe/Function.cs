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
            var x = input.Session.Attributes.TryGetValue("State", out StateObject);
            eStates State = eStates.Initial;
            if (x == true)
            {
                State = (eStates)StateObject;
            }
            //********************************************* 


            var requestType = input.GetRequestType();

            if (requestType == typeof(IntentRequest))
            {
                return DoIntentRequest(input, ref resultText, State);
            }
            else if (requestType == typeof(LaunchRequest))
            {
                return DoLaunchRequest();
            }
            else if (requestType == typeof(SessionEndedRequest))
            {
                return MakeSkillResponse("Anwendung unerwartet beendet", true);
            }
            else
            {
                return MakeSkillResponse(
                        $"Unbekannter requestType",
                        true);
            }
        }

        private SkillResponse DoLaunchRequest()
        {
            return MakeSkillResponse("Welches Fach willst du üben?", false);
        }

        private SkillResponse DoIntentRequest(SkillRequest input, ref string resultText, eStates State)
        {
            var intentRequest = input.Request as IntentRequest;



            switch (intentRequest.Intent.Name)
            {
                case "SubjectChooser":
                    input.Session.Attributes["State"] = eStates.SubjectChosser;
                    var resValueSubject = intentRequest.Intent.Slots["Subject"].Value;
                    input.Session.Attributes.Add("Subject", resValueSubject);
                    resultText = "Für welche Klasse möchtest du üben.";
                    break;
                case "ClassChooser":
                    input.Session.Attributes["State"] = eStates.ClassChooser;
                    var resValueClass = intentRequest.Intent.Slots["Class"].Value;
                    input.Session.Attributes.Add("Class", resValueClass);
                    resultText = "Gut dann legen wir los.";
                    //resultText += "5 +2";
                    break;
                default:
                    break;

            }
            return MakeSkillResponse(resultText, false);
        }

        private SkillResponse MakeSkillResponse(string outputSpeech, bool shouldEndSession, string repromptText = "Repromt")
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



            var skillResponse = new SkillResponse
            {
                Response = response,
                Version = "1.0"
            };
            return skillResponse;
        }


        public async Task<SkillResponse> SubjectChooser(SkillRequest input, ILambdaContext context)
        {
            return MakeSkillResponse("Für welches Fach möchtest du üben?", false);
        }
    }
}