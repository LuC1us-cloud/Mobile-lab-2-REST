namespace Library;

public class Book
{
    public int Id { get; set; } // Id (Primary key)
    public float LoanDuration { get; set; } // LoanDuration
    public bool Taken { get; set; } = false; // Indicates if the book is taken or not
    public DateTime LoanTime { get; set; } // Time when the book was taken
    public float Wear { get; set; } = 0f; // Indicates how much the book is worn
    public float RepairPrice { get; set; } = 1f; // Price of the book per 1 wear to repair

    public float GetRepairPrice()
    {
        return RepairPrice * Wear;
    }

    public float GetRepairAmount(float money)
    {
        return money / GetRepairPrice();
    }
}

public interface IService
{

    Book TakeBook(int id);

    void ReturnBook(Book book);

    void RepairBook(int id, float wear);

    int GetLibraryCapacity();
    List<int> GetWornOutBooks();
    float GetLibraryBudget();
}