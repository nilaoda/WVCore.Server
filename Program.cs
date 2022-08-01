using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using NLog.Config;
using NLog.Targets;
using NLog;
using ProtoBuf;

namespace WVCore.Server
{
    internal class Program
    {
        static FileInfo? ClientIdFile = null;
        static FileInfo? PrivateKeyFile = null;

        static Program()
        {
            if (!File.Exists("device_client_id_blob"))
                throw new FileNotFoundException("找不到device_client_id_blob文件");
            if (!File.Exists("device_private_key"))
                throw new FileNotFoundException("找不到device_private_key文件");

            ClientIdFile = new FileInfo("device_client_id_blob");
            PrivateKeyFile = new FileInfo("device_private_key");
        }

        static WVApi GetWVApi()
        {
            return new WVApi(ClientIdFile, PrivateKeyFile);
        }

        /**
         * Test PSSH: AAAAp3Bzc2gAAAAA7e+LqXnWSs6jyCfc1R0h7QAAAIcSEFF0U4YtQlb9i61PWEIgBNcSEPCTfpp3yFXwptQ4ZMXZ82USEE1LDKJawVjwucGYPFF+4rUSEJAqBRprNlaurBkm/A9dkjISECZHD0KW1F0Eqbq7RC4WmAAaDXdpZGV2aW5lX3Rlc3QiFnNoYWthX2NlYzViZmY1ZGM0MGRkYzlI49yVmwY=
         * Server: https://cwip-shaka-proxy.appspot.com/no_auth
         */
        static void Main(string[] args)
        {
            InitLog();
            var logger = LogManager.GetCurrentClassLogger();
            logger.Debug("Log Inited.");

            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();

            builder.Services.AddCors(p =>
            {
                p.AddDefaultPolicy(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
            });

            logger.Info("The app started!");
            logger.Info("Listening: http://0.0.0.0:18888");

            var app = builder.Build();
            app.UseCors();
            app.Urls.Add("http://0.0.0.0:18888");
            app.MapGet("/wvapi", () => "Please Use POST!");
            app.MapPost("/wvapi", async (HttpRequest request) => await RequestHandler.HandleCommon(request, GetWVApi()));
            app.MapPost("/getchallenge", async (HttpRequest request) => await RequestHandler.HandleChallenge(request, GetWVApi()));
            app.MapPost("/getkeys", async (HttpRequest request) => await RequestHandler.HandleKeys(request, GetWVApi()));
            app.Run();
        }

        static void InitLog()
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);
            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            consoleTarget.Layout = "${longdate} [${level:uppercase=true}] - ${message} ${exception:format=tostring}";
            fileTarget.Layout = "${longdate} [${level:uppercase=true}] [${logger}] - ${message} ${exception:format=tostring}";
            fileTarget.FileName = "${basedir}/logs/WVCore.${shortdate}.log";

            var rule1 = new LoggingRule("*", NLog.LogLevel.Info, consoleTarget);
            config.LoggingRules.Add(rule1);

            if (!Environment.GetCommandLineArgs().Any(s => s.Contains("nolog"))) 
            {
                var rule2 = new LoggingRule("*", NLog.LogLevel.Debug, fileTarget);
                config.LoggingRules.Add(rule2);
            }

            LogManager.Configuration = config;
        }
    }
}