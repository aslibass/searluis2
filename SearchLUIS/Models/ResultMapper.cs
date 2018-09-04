using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Search.Models;

namespace SearchLUIS.Models
{
    public class ResultMapper

    {

        public static SearchHit ToSearchHit(SearchResult hit)

        {

            return new SearchHit
            {
                StoreName = (string)hit.Document["StoreName"],
                StoreRetailBusinessManager = (string)hit.Document["StoreRetailBusinessManager"],
                RetailStoreAddress = GetInfo(hit)



            };

            /*
            object Tags;

            if (hit.Document.TryGetValue("Tags", out Tags))

            {

                searchHit.PropertyBag.Add("Tags", Tags);

            }



            object NumFaces;

            if (hit.Document.TryGetValue("NumFaces", out NumFaces))

            {

                searchHit.PropertyBag.Add("NumFaces", NumFaces);

            }



            object Faces;

            if (hit.Document.TryGetValue("Faces", out Faces))

            {

                searchHit.PropertyBag.Add("Faces", Faces);

            }



            return searchHit;
            */

        }

        private static string GetInfo(SearchResult result)
        {

            var street = result.Document["RetailStoreStreet"];
            var city = result.Document["RetailStoreCity"];
            var state = result.Document["RetailStoreState"];
            var postcode = result.Document["RetailStorePostcode"];
            var stateManager = result.Document["StoreStateManager"];
            var population = result.Document["StoreAreaPopulation"];
            var medianAge = result.Document["StoreAreaMedianAge"];
            var area = result.Document["RetailAreasqm"];
            var bays = result.Document["StoreBayCount"];


            return $"This store serves a population of {population} whose median age is {medianAge}. It has an Retail area of {area} sqm with {bays} bays. It is located at {street}, {city}, {state}, {postcode}. For escalations contact the statemanager viz {stateManager}";
        }

    }

}
