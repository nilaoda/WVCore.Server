using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WVCore.Server
{
    internal class Util
    {
        public static string ConvertToJson(object o)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new JsonStringEnumConverter() }
            };
            return JsonSerializer.Serialize(o, options);
        }

        public static string BytesToHex(byte[] data, string split = "")
        {
            return BitConverter.ToString(data).Replace("-", split);
        }
    }
}
