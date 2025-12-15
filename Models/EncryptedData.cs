using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CookieCloudClientDotnet.Models
{
    public class EncryptedData
    {
        [JsonPropertyName("encrypted")]
        public string? Encrypted { get; set; }
    }

    [JsonSerializable(typeof(EncryptedData))]
    public partial class EncryptedDataContext : JsonSerializerContext;
}
