﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SearchLUIS.Models;


namespace SearchLUIS.Dialogs
{
    [Serializable]

    public class SearchDialog : IDialog<object>

    {

        private string searchText = "";



        public SearchDialog(string facet)

        {

            searchText = facet;

        }



        public async Task StartAsync(IDialogContext context)

        {

            ISearchIndexClient indexClientForQueries = CreateSearchIndexClient();
            
            // For more examples of calling search with SearchParameters, see
            // https://github.com/Azure-Samples/search-dotnet-getting-started/blob/master/DotNetHowTo/DotNetHowTo/Program.cs.  
            DocumentSearchResult results = await indexClientForQueries.Documents.SearchAsync(searchText);

            await SendResults(context, results);

        }



        private async Task SendResults(IDialogContext context, DocumentSearchResult results)

        {

            var message = context.MakeMessage();
            if (results.Results.Count == 0)
            {

                await context.PostAsync("There were no results found for \"" + searchText + "\".");

                context.Done<object>(null);

            }

            else

            {

                SearchHitStyler searchHitStyler = new SearchHitStyler();

                searchHitStyler.Apply(

                    ref message,

                    "Here are the results that I found:",

                    results.Results.Select(r => ResultMapper.ToSearchHit(r)).ToList().AsReadOnly());



                await context.PostAsync(message);

                context.Done<object>(null);

            }

        }



        private ISearchIndexClient CreateSearchIndexClient()

        {

            string searchServiceName = ConfigurationManager.AppSettings["SearchDialogsServiceName"];

            string queryApiKey = ConfigurationManager.AppSettings["SearchDialogsServiceKey"];

            string indexName = ConfigurationManager.AppSettings["SearchDialogsIndexName"];



            SearchIndexClient indexClient = new SearchIndexClient(searchServiceName, indexName, new SearchCredentials(queryApiKey));

            return indexClient;

        }



        [Serializable]

        public class SearchHitStyler : PromptStyler

        {

            public void Apply<T>(ref IMessageActivity message, string prompt, IReadOnlyList<T> options, IReadOnlyList<string> descriptions = null, string speak = null)

            {

                var hits = options as IList<SearchHit>;

                if (hits != null)

                {

                    var cards = hits.Select(h => new HeroCard
                    {
                        Title = h.StoreName,
                        Subtitle = h.StoreRetailBusinessManager,
                        Text = h.RetailStoreAddress
                    });


                    message.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                    message.Attachments = cards.Select(c => c.ToAttachment()).ToList();

                    message.Text = prompt;
                    message.Speak = speak;

                }

                else

                {

                    base.Apply<T>(ref message, prompt, options, descriptions,speak);

                }

            }

        }



    }
}