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
        private static HttpClient _httpClient;
        public const string INVOCATION_NAME = "Nachhilfe";

        public Function()
        {
            _httpClient = new HttpClient();
        }

        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            var resultText = "";
           // input.Session
            var requestType = input.GetRequestType();
            if (requestType == typeof(IntentRequest))
            {
                var intentRequest = input.Request as IntentRequest;
                intentRequest.
                switch (intentRequest.Intent.Name)
                {
                    case "SubjectChooser":
                        resultText = intentRequest.Intent.Slots["Subject"].Value;
                        break;
                    case "UserResponseMathe":
                        resultText = intentRequest.Intent.Slots["Number"].Value;
                        break;
                    case "UebungsZeit":
                        resultText = intentRequest.Intent.Slots["Duration"].Value;
                        break;
                    default:
                        break;

                }
                return MakeSkillResponse(resultText, false);

            }
            else if (requestType == typeof(LaunchRequest))
            {
                return MakeSkillResponse("Welches Fach willst du üben?", false);
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
           return MakeSkillResponse("Welches Fach willst du üben?", false);
        }
    }
}