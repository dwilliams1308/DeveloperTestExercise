using System;
using System.Collections.Generic;
using System.Linq;
using ThirdPartyTools;
using Serilog;
using System.Collections.ObjectModel;

namespace FileData
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                ConfigureLogging();

                Log.Debug("Starting program using args: {@args}", args);

                FileDataProcessor fileDataProcessor = new FileDataProcessor(args);
                // validate args
                if (!fileDataProcessor.HasValidArgs(out Collection<string> validationErrors))
                {
                    foreach(string validationError in validationErrors)
                    {
                        Console.WriteLine(validationError);
                    }

                    Log.Debug("Terminating due to invalid arguments");

                    return;
                }

                // Run actions against target file and log to the console
                List<string> results = fileDataProcessor.RunActions();
                foreach (var result in results)
                {
                    Console.WriteLine(result);
                }

                Log.Debug("Program ending normally");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program ending abnormally. logging details and then terminating.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
                .WriteTo.Console()
#endif
                .WriteTo.File("logging-.log")
                .CreateLogger();
        }
    }
}
