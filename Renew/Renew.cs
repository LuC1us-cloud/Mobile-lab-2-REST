namespace Client;

using System.Net.Http;

using NLog;

using ServiceReference;


/// <summary>
/// Renew
/// </summary>
class Renew
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
                var service = new RenewService("http://127.0.0.1:5000", new HttpClient());

                //test service
                var rnd = new Random();

                while (true)
                {
                    log.Info("--------------------");
                    Thread.Sleep(10000);

                    var booksIds = service.GetWornOutBooks();
                    log.Info($"Found {booksIds.Count} worn out books");

                    if (booksIds.Count == 0)
                    {
                        log.Info("No worn out books found");
                        continue;
                    }

                    List<Book> books = new List<Book>();
                    foreach (var id in booksIds)
                    {
                        var book = service.TakeBook(id);
                        books.Add(book);
                        log.Info($"Took book {id} and inspected it");
                        service.ReturnBook(book);
                    }

                    float budget = service.GetLibraryBudget();
                    log.Info($"Library budget: {budget.ToString("0.00")}$");
                    if (budget == 0)
                    {
                        log.Info("No budget left");
                        continue;
                    }

                    Book mostWornOutBook = books.First();
                    foreach (var book in books)
                    {
                        if (book.Wear > mostWornOutBook.Wear)
                        {
                            mostWornOutBook = book;
                        }
                    }

                    log.Info($"Most worn out book: Id {mostWornOutBook.Id}, ({mostWornOutBook.Wear.ToString("0.00")})");

                    float repairPrice = mostWornOutBook.RepairPrice * mostWornOutBook.Wear;
                    float repairAmount = budget / repairPrice;
                    float repairAmountPercentage = repairAmount * 100;
                    log.Info($"Available repair amount: {(repairAmountPercentage).ToString("0.00")}% of {mostWornOutBook.Wear.ToString("0.00")} | Full repair price: {repairPrice.ToString("0.00")}$");

                    if (repairAmountPercentage > 100) repairAmountPercentage = 100;

                    float wearToRepair = mostWornOutBook.Wear * repairAmountPercentage / 100;

                    log.Info($"Repairing {wearToRepair.ToString("0.00")} wear");

                    service.RepairBook(mostWornOutBook.Id, wearToRepair);

                    Thread.Sleep(10000);
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
        var self = new Renew();
        self.Run();
    }
}