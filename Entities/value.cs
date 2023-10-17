using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPExternalAccessNew.Entities
{
    public class value
    {
        [JsonProperty(PropertyName = "@odata.etag")]
        public string odataContext { get; set; }
        public string createdDateTime { get; set; }
        public string description { get; set; }
        public string eTag { get; set; }
        public string id { get; set; }
        public string lastModifiedDateTime { get; set; }
        public string name { get; set; }
        public string webUrl { get; set; }
        public string displayName { get; set; }

    }
}
