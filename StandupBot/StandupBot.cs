using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using Slack.NetStandard;
using Slack.NetStandard.Messages;
using Slack.NetStandard.Messages.Elements;
using Slack.NetStandard.Messages.Blocks;
using Slack.NetStandard.Objects;

namespace StandupBot
{
    public static class StandupBotFunction
    {
        [FunctionName(nameof(MessageTeam))]
        public static async Task<IActionResult> MessageTeam(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string message = $"Slack message from webhook to {name}";
            var payload = new { text = message };
            var responseMessage =
                await SendSlackMessage(payload, log);

            return new OkObjectResult(responseMessage);
        }

        [FunctionName(nameof(FromSlack))]
        public static async Task FromSlack(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = req.Form["payload"]; //await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string triggerId = data.trigger_id.ToString();

            log.LogInformation(requestBody);

            var view = new View();
            view.Type = "modal";
            view.Title = "Daily Standup";
            view.Submit = new PlainText("Submit");
            view.Close = new PlainText("Close");
            view.Blocks = new IMessageBlock[]
                { new Input
                    {
                        Label = new PlainText("What did you accomplish yesterday"),
                        Element = new PlainTextInput
                        {
                            Multiline = true,
                            InitialValue = "1. \n2. \n3."
                        }
                    },
                    new Input
                    {
                        Label = new PlainText("What is your plan for today?"),
                        Element = new PlainTextInput
                        {
                            Multiline = true,
                            InitialValue = "1. \n2. \n3."
                        }
                    },
                    new Input
                    {
                        Label = new PlainText("Any blockers?"),
                        Element = new PlainTextInput()
                    }
                };

            await SendSlackMessage(view, log);
        }


        [FunctionName(nameof(MessageTeamTimer))]
        public static async Task MessageTeamTimer(
           [TimerTrigger("0 */1 * * * *")] TimerInfo myTimer,
           ILogger log)
        {
            var message = new Message();
            message.Blocks = new List<IMessageBlock>();
            message.Blocks.Add(new Section
            {
                Text = new PlainText("Good morning, slacker! It's standup time!!!"),
                Accessory = new Button
                {
                    Text = "Submit",
                    Value = "Submit"
                }
            });

            await SendSlackMessage(message, log);
        }


        public static async Task<string> SendSlackMessage(object message, ILogger log)
        {
            log.LogInformation("Sending slack message");

            using (var client = new HttpClient())
            {
                string url = GetEnvironmentVariable("SlackApi");
                client.BaseAddress = new Uri(url);
                var result = await client.PostAsJsonAsync(
                               "",
                               message);

                var response = await result.Content.ReadAsStringAsync();

                log.LogInformation(response);

                return response;
            }
        }

        public static string GetEnvironmentVariable(string name)
        {
            return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
