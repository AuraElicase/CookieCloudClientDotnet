using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CookieCloudClientDotnet.Models
{
    public class CookieData
    {
        [JsonPropertyName("cookie_data")]
        public string? Cookie { get; set; }

        [JsonPropertyName("local_storage_data")]
        public string? LocalStorage { get; set; }
    }

    [JsonSerializable(typeof(CookieData))]
    public partial class CookieDataContext : JsonSerializerContext;
}
