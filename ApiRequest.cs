using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVCore.Server
{
    class ChallengeRequest
    {
        public string PSSH { get; set; }
        public string CertBase64 { get; set; }
    }

    class KeyRequest
    {
        public string PSSH { get; set; }
        public string ChallengeBase64 { get; set; }
        public string LicenseBase64 { get; set; }
    }

    internal class ApiRequest
    {
        public string PSSH { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string LicenseUrl { get; set; }
    }
}
