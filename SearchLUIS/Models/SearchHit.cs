using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchLUIS.Models
{

    [Serializable]

    public class SearchHit

    {

        public SearchHit()

        {

            this.PropertyBag = new Dictionary<string, object>();

        }

        public string id { get; set; }

        public string rid { get; set; }

        public string StoreNumber { get; set; }

        public string StoreName { get; set; }

        public string StoreType { get; set; }

        public string StoreStatus { get; set; }

        public string StoreRetailBusinessManager { get; set; }

        public string StoreStateManager { get; set; }

        public string StoreFranchiseGroup { get; set; }

        public string RetailStoreStreet { get; set; }

        public string RetailStoreCity { get; set; }

        public string RetailStoreState { get; set; }

        public string RetailStorePostcode { get; set; }

        public string StoreLocation { get; set; }

        public string StorePrecinct { get; set; }

        public string StoreAreaPopulation { get; set; }

        public string StoreAreaMedianAge { get; set; }

        public string StoreAreaMedianHholdIncome { get; set; }

        public string StoreGrading { get; set; }

        public string StoreBayCount { get; set; }

        public string TotalTenancysqm { get; set; }

        public string RetailAreasqm { get; set; }

        public string RetailStoreAddress { get; set; }

        public IDictionary<string, object> PropertyBag { get; set; }

    }
}