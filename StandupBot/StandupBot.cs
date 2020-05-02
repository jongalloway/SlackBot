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

namespace StandupBot
{
    public static class StandupBot
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

            string message = $"Slack webhook triggered";
            log.LogInformation(requestBody);

            var dialog = new StandupBotModels.Dialog.RootObject();
            dialog.blocks = new StandupBotModels.Dialog.Block[]
            { new StandupBotModels.Dialog.Block
                {
                    type = "section",
                    text = new StandupBotModels.Dialog.Text
                    {
                        type = "mrkdwn",
                        text = "It worked!!!"
                    },
                        accessory = new StandupBotModels.Dialog.Accessory {
                            type = "button",
                            text = new StandupBotModels.Dialog.Text
                            {
                                type = "plain_text",
                                text = "Celebrate"
                            },
                            value = "submit"
                        }
                }
            };

            await SendSlackMessage(dialog, log);
        }


        [FunctionName(nameof(MessageTeamTimer))]
        public static async Task MessageTeamTimer(
           [TimerTrigger("0 */1 * * * *")] TimerInfo myTimer,
           ILogger log)
        {
            var dialog = new StandupBotModels.Dialog.RootObject();
            dialog.blocks = new StandupBotModels.Dialog.Block[]
            { new StandupBotModels.Dialog.Block
                {
                    type = "section",
                    text = new StandupBotModels.Dialog.Text
                    {
                        type = "mrkdwn",
                        text = "Good morning, slacker! It's standup time!!!"
                    },
                        accessory = new StandupBotModels.Dialog.Accessory {
                            type = "button",
                            text = new StandupBotModels.Dialog.Text
                            {
                                type = "plain_text",
                                text = "Submit"
                            },
                            value = "submit"
                        }
                }
        };

            await SendSlackMessage(dialog, log);
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
            return name + ": " +
                System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
