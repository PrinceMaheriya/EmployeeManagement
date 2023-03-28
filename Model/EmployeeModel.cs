using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WpfApp1
{
  
    public partial class EmployeeModel
    {
        [JsonProperty(PropertyName = "code")]
        public long Code { get; set; }

        [JsonProperty(PropertyName = "meta")]
        public Meta Meta { get; set; }

        [JsonProperty(PropertyName = "data")]
        public List<Datum> Data { get; set; }
    }

    public partial class SingleEmplyee
    {
        [JsonProperty(PropertyName = "code")]
        public long Code { get; set; }

        [JsonProperty(PropertyName = "meta")]
        public Meta Meta { get; set; }

        [JsonProperty(PropertyName = "data")]
        public Datum Data { get; set; }
    }
    public partial class Datum
    {
        [JsonProperty(PropertyName = "id")]
        public long? Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
    }

    public partial class Meta
    {
        [JsonProperty(PropertyName = "pagination")]
        public Pagination Pagination { get; set; }
    }

    public partial class Pagination
    {
        [JsonProperty(PropertyName = "total")]
        public long Total { get; set; }

        [JsonProperty(PropertyName = "pages")]
        public long Pages { get; set; }

        [JsonProperty(PropertyName = "page")]
        public long Page { get; set; }

        [JsonProperty(PropertyName = "limit")]
        public long Limit { get; set; }
    }


}
