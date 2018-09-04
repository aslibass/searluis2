using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Linq;


namespace SearchLUIS.Dialogs
{
    [LuisModel("b68b7ac3-1880-460e-9605-0c5d8dc1a2f9", "5c226b04f1044ae88ad7442ccde33eea")]
    [Serializable]
    public class RootDialog : DispatchDialog<object>
    {

        public async Task Hello(IDialogContext context, IActivity activity)
        {
            await context.PostAsync("Hello from RegEx!  I am a API Bot.  I can search for store info and answer faqs'.");
        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, IActivity activity)
        {
            // Launch help dialog with button menu  
            List<string> choices = new List<string>(new string[] { "Tell me about a Store", "I have a general question" });
            PromptDialog.Choice<string>(context, ResumeAfterChoice,
                new PromptOptions<string>("How can I help you?", options: choices));
        }

        private async Task ResumeAfterChoice(IDialogContext context, IAwaitable<string> result)
        {
            string choice = await result;

            switch (choice)
            {
                case "Tell me about a Store":
                     PromptDialog.Text(context, ResumeAfterSearchTopicClarification,
                         "What Info about stores do you want?");
                    break;
                case "I have a general question":
                    await SearchFAQ(context, null);
                    break;
                default:
                    await context.PostAsync("I'm sorry. I didn't understand you.");
                    break;
            }
        }

        private async Task ResumeAfterSearchTopicClarification(IDialogContext context, IAwaitable<string> result)
        {
            string searchTerm = await result;
            context.Call(new SearchDialog(searchTerm), ResumeAfterSearchDialog);
        }

        private async Task ResumeAfterSearchDialog(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Done searching store info");
        }

        [LuisIntent("FAQ")]
        public async Task SearchFAQ(IDialogContext context, LuisResult result)
        {
            PromptDialog.Text(context, ResumeAfterFAQClarification,
                   "What is your general question?");
        }

        private async Task ResumeAfterFAQClarification(IDialogContext context, IAwaitable<string> result)
        {
            string searchTerm = await result;
            var postBody = $"{{\"question\": \"{searchTerm}\"}}";
            using (WebClient client = new WebClient())
            {
                //Set the encoding to UTF8
                client.Encoding = System.Text.Encoding.UTF8;
                //Add the subscription key header
                client.Headers.Add("Ocp-Apim-Subscription-Key", "7ab9a4eb-fb57-4476-9b7d-4b41ef4a0cd2");
                client.Headers.Add("Content-Type", "application/json");
                var answer = JsonConvert.DeserializeObject<QnAMakerResult>(
                    client.UploadString("https://vjqna.azurewebsites.net/qnamaker/knowledgebases/f26b640c-e49f-474a-a299-7ac23deb1266/generateAnswer", postBody));
                if (answer.answers.FirstOrDefault().score > 0.2)
                {
                    string response = answer.answers.FirstOrDefault().answer;
                    await context.PostAsync(response);

                    //await context.SayAsync(text:answer.answers.FirstOrDefault().answer, speak:answer.answers.FirstOrDefault().answer);
                }
                else
                {
                    //await context.PostAsync($"Sorry, I did not understand '{result.Query}'. Type 'help' if you need assistance.");
                    string response = $"Sorry, I did not understand '{result}'. Type 'help' if you need assistance.";
                    await context.PostAsync(response);
                }


            }
        }

            

        public class Answer
        {
            public string answer { get; set; }
            public List<string> questions { get; set; }
            public double score { get; set; }
        }

        public class QnAMakerResult
        {
            public List<Answer> answers { get; set; }
        }

    }



}
