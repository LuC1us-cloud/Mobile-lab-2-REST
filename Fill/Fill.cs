namespace Client;

using System.Net.Http;

using NLog;

using ServiceReference;


/// <summary>
/// Fill
/// </summary>
class Fill
{
    /// <summary>
    /// Logger for this class.
    /// </summary>
    private Logger log = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Configure loggin subsystem.
    /// </summary>
    private void ConfigureLogging()
    {
        var config = new NLog.Config.LoggingConfiguration();

        var console =
            new NLog.Targets.ConsoleTarget("console")
            {
                Layout = @"${date:format=HH\:mm\:ss}|${level}| ${message} ${exception}"
            };
        config.AddTarget(console);
        config.AddRuleForAllLevels(console);

        LogManager.Configuration = config;
    }

    /// <summary>
    /// Program body.
    /// </summary>
    private void Run()
    {
        ConfigureLogging();

        //main loop
        while (true)
        {
            try
            {
                //connect to server
                var service = new FillService("http://127.0.0.1:5000", new HttpClient());

                //test service
                var rnd = new Random();

                while (true)
                {
                    var canAdd = service.CanAddLiquid();

                    var liquidToAdd = rnd.Next(1, 20);

                    if (canAdd)
                    {
                        log.Info($"Generated amount to add: {liquidToAdd}");
                        var addedLiquid = service.AddLiquid(liquidToAdd);
                        log.Info($"Amount of liquid added: {addedLiquid}");
                        log.Info("\n");
                    }
                    else
                    {
                        log.Info("I cannot add any more liquid");
                        log.Info("\n");
                    }
                    log.Info("---");

                    Thread.Sleep(2000);
                }
            }
            catch (Exception e)
            {
                //log exceptions
                log.Error(e, "Unhandled exception caught. Restarting.");

                //prevent console spamming
                Thread.Sleep(2000);
            }
        }
    }

    /// <summary>
    /// Program entry point.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    static void Main(string[] args)
    {
        var self = new Fill();
        self.Run();
    }
}