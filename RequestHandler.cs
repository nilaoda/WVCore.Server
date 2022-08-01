using Microsoft.AspNetCore.Http;
using NLog;

namespace WVCore.Server
{
    internal class RequestHandler
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        public static async Task<IResult> HandleCommon(HttpRequest request, WVApi wvApi)
        {
            try
            {
                var apiReqeust = await request.ReadFromJsonAsync<ApiRequest>();
                logger.Info(request.Path + " ==> " + Util.ConvertToJson(apiReqeust));
                var keys = await WVUtil.GetKeysAsync(apiReqeust.PSSH, apiReqeust.LicenseUrl, apiReqeust.Headers, wvApi);
                var apiResponse = new ApiResponse()
                {
                    PSSH = apiReqeust.PSSH,
                    Keys = keys
                };
                logger.Info(request.Path + " <== " + Util.ConvertToJson(apiResponse));
                return Results.Ok(apiResponse);
            }
            catch (Exception)
            {
                return Results.Problem("Error");
            }
        }

        public static async Task<IResult> HandleChallenge(HttpRequest request, WVApi wvApi)
        {
            try
            {
                var challengeReqeust = await request.ReadFromJsonAsync<ChallengeRequest>();
                logger.Info(request.Path + " ==> " + Util.ConvertToJson(challengeReqeust));
                var challenge = wvApi.GetChallenge(challengeReqeust.PSSH, challengeReqeust.CertBase64, false, false);
                var challengeResponse = new ChallengeResonse()
                {
                    ChallengeBase64 = Convert.ToBase64String(challenge)
                };
                logger.Info(request.Path + " <== " + Util.ConvertToJson(challengeResponse));
                return Results.Ok(challengeResponse);
            }
            catch (Exception)
            {
                return Results.Problem("Error");
            }
        }

        public static async Task<IResult> HandleKeys(HttpRequest request, WVApi wvApi)
        {
            try
            {
                var keyReqeust = await request.ReadFromJsonAsync<KeyRequest>();
                logger.Info(request.Path + " ==> " + Util.ConvertToJson(keyReqeust));
                var pssh = wvApi.ProvideLicense(keyReqeust.LicenseBase64, keyReqeust.ChallengeBase64);
                var keys = wvApi.GetKeys().Select(k => k.ToString()).ToList();
                var apiResponse = new ApiResponse()
                {
                    PSSH = keyReqeust.PSSH,
                    Keys = keys
                };
                logger.Info(request.Path + " <== " + Util.ConvertToJson(apiResponse));
                return Results.Ok(apiResponse);
            }
            catch (Exception)
            {
                return Results.Problem("Error");
            }
        }
    }
}
