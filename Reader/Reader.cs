namespace Client;

using System.Net.Http;

using NLog;

using ServiceReference;


/// <summary>
/// Reader
/// </summary>
class Reader
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
                var service = new ReaderService("http://127.0.0.1:5000", new HttpClient());

                //test service
                var rnd = new Random();

                while (true)
                {
                    log.Info("");
                    // takes a random book from the library
                    int bookNumber = rnd.Next(0, service.GetLibraryCapacity());
                    Book book = service.TakeBook(bookNumber);

                    // if the book is null, it means that it is already taken
                    if (book == null)
                    {
                        log.Info($"Book {bookNumber} is not available");
                        Thread.Sleep(2000);
                        continue;
                    }

                    // decides a random time to take reading the book
                    int timeToRead = rnd.Next(5000, 10000);
                    log.Info($"Reading book {bookNumber} for {timeToRead} ms");
                    Thread.Sleep(timeToRead);

                    // Returns the book
                    service.ReturnBook(book);
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
        var self = new Reader();
        self.Run();
    }
}