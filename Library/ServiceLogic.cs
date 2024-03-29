namespace Library;

using NLog;


/// <summary>
/// Service logic.
/// </summary>
class ServiceLogic : IService
{
    /// <summary>
    /// Logger for this class.
    /// </summary>
    private Logger log = LogManager.GetCurrentClassLogger();
    public Book TakeBook(int id)
    {
        log.Info($"");

        // Generate a random time for the book to be taken
        int durationToLoan = new Random().Next(4000, 8000);
        // Check if the book is available
        if (Library.books[id].Taken)
        {
            log.Info($"Book with id: {id} is not available");
            return null;
        }

        // If it is, set it to unavailable
        Library.books[id].Taken = true;
        // Set he books LoanTime to the time it was taken
        Library.books[id].LoanDuration = durationToLoan;
        // Set the time it was taken
        Library.books[id].LoanTime = DateTime.Now;
        log.Info($"Took: id {Library.books[id].Id}, for {durationToLoan} ms");
        // Return the book
        return Library.books[id];
    }

    public void ReturnBook(Book book)
    {
        log.Info($"");

        int bookId = book.Id;
        // Calculate the time the book was taken
        var timeTaken = DateTime.Now - Library.books[bookId].LoanTime;

        // if timeTaken exceeds the durationToLoan, money is added to the library
        if (timeTaken.TotalMilliseconds > Library.books[bookId].LoanDuration)
        {
            // Calculate the amount of money to add
            float moneyToAdd = (float)((timeTaken.TotalMilliseconds - Library.books[bookId].LoanDuration) / 1000);
            // Add the money to the library
            Library.money += moneyToAdd;
            log.Info($"Book {bookId} was {(int)(timeTaken.TotalMilliseconds - Library.books[bookId].LoanDuration)} ms late, added {moneyToAdd.ToString("0.00")}$ to library | Total money: {Library.money.ToString("0.00")}$");
        }

        // Calculate the wear the book has
        var wear = timeTaken.TotalMilliseconds / 100000;
        // Add the wear to the books wear
        Library.books[bookId].Wear += (float)wear;
        // Set the book to available
        Library.books[bookId].Taken = false;
        // Clear the loan time and duration
        Library.books[bookId].LoanDuration = 0;
        Library.books[bookId].LoanTime = DateTime.MinValue;

        log.Info($"Returned: id {bookId}, wear {Library.books[bookId].Wear.ToString("0.00")}, after {(int)timeTaken.TotalMilliseconds} ms");
    }
    public List<int> GetWornOutBooks()
    {
        log.Info($"");

        // Get all the books that are worn out
        List<int> wornOutBooks = Library.books.Where(b => b.Wear > 0 && !b.Taken).Select(b => b.Id).ToList();
        log.Info($"Found {wornOutBooks.Count} worn out books");
        return wornOutBooks;
    }

    public void RepairBook(int id, float wear)
    {
        log.Info($"");

        // Check if the book is available
        if (Library.books[id].Taken)
        {
            log.Info($"Book with id: {id} is not available");
            return;
        }

        // Check if the book is worn out
        if (Library.books[id].Wear == 0)
        {
            log.Info($"Book with id: {id} is not worn out");
            return;
        }

        // Check if the wear is more than the books wear
        if (wear > Library.books[id].Wear)
        {
            log.Info($"Wear is more than the books wear");
            return;
        }

        // Calculate the price of the repair
        float repairPrice = Library.books[id].RepairPrice * wear;
        // Check if the library has enough money
        if (Library.money < repairPrice)
        {
            log.Info($"Library does not have enough money to repair the book");
            return;
        }

        // Remove the money from the library
        Library.money -= repairPrice;
        // Remove the wear from the book
        float oldWear = Library.books[id].Wear;
        Library.books[id].Wear -= wear;
        log.Info($"Repaired: id {id}, wear {oldWear.ToString("0.00")} -> {Library.books[id].Wear.ToString("0.00")}, removed {repairPrice.ToString("0.00")}$ from library | Total money: {Library.money.ToString("0.00")}$");
    }

    public int GetLibraryCapacity()
    {
        return Library.capacity;
    }

    public float GetLibraryBudget()
    {
        return Library.money;
    }
}