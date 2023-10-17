using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPExternalAccessNew.Entities
{
    public class SharePointList
    {
        [JsonProperty(PropertyName = "@odata.context")]
        public string odataContext { get; set; }
        public List<value> value { get; set; }
    }
}
