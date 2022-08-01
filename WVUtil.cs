using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVCore.Widevine;

namespace WVCore.Server
{
    internal class WVUtil
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        public static async Task<List<string>> GetKeysAsync(string pssh, string licenseUrl, Dictionary<string, string>? headers, WVApi cdm)
        {
            var keyStrings = new List<string>();
            if (headers == null)
                headers = new Dictionary<string, string>();

            logger.Debug("get cert...");
            var resp1 = await HTTPUtil.PostDataAsync(licenseUrl, headers, new byte[] { 0x08, 0x04 });
            var certDataB64 = Convert.ToBase64String(resp1);
            logger.Debug("get challenge...");
            var challenge = cdm.GetChallenge(pssh, certDataB64, false, false);
            logger.Debug("get license...");
            var resp2 = await HTTPUtil.PostDataAsync(licenseUrl, headers, challenge);
            var licenseB64 = Convert.ToBase64String(resp2);
            //license传递给cdm
            cdm.ProvideLicense(licenseB64);
            logger.Debug("get keys...");
            List<ContentKey> keys = cdm.GetKeys();
            foreach (var k in keys)
            {
                keyStrings.Add(k.ToString());
            }
            return keyStrings;
        }
    }
}
