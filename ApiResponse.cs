using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVCore.Server
{
    class ChallengeResonse
    {
        public string ChallengeBase64 { get; set; }
    }

    internal class ApiResponse
    {
        public string PSSH { get; set; }
        public List<string> Keys { get; set; }
    }
}
