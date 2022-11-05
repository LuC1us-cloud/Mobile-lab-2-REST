namespace Library;

using Microsoft.AspNetCore.Mvc;


/// <summary>
/// Service. Class must be marked public, otherwise ASP.NET core runtime will not find it.
/// </summary>
[Route("/service")]
[ApiController]
public class ServiceController : ControllerBase
{
    /// <summary>
    /// Service logic. Use a singleton instance, since controller is instance-per-request.
    /// </summary>
    private static readonly ServiceLogic logic = new ServiceLogic();

    [HttpGet]
    [Route("takeBook/{id}")]
    public ActionResult<Book> TakeBook([FromRoute] int id)
    {
        lock (logic)
        {
            return logic.TakeBook(id);
        }
    }

    [HttpPost]
    [Route("returnBook")]
    public ActionResult ReturnBook([FromBody] Book book)
    {
        lock (logic)
        {
            logic.ReturnBook(book);
            return Ok();
        }
    }

    [HttpGet]
    [Route("repairBook/{id}/{wear}")]
    public ActionResult RepairBook([FromRoute] int id, [FromRoute] float wear)
    {
        lock (logic)
        {
            logic.RepairBook(id, wear);
            return Ok();
        }
    }

    [HttpGet]
    [Route("getLibraryCapacity")]
    public ActionResult<int> GetLibraryCapacity()
    {
        lock (logic)
        {
            return logic.GetLibraryCapacity();
        }
    }

    [HttpGet]
    [Route("getWornOutBooks")]
    public ActionResult<Book[]> GetWornOutBooks()
    {
        lock (logic)
        {
            return logic.GetWornOutBooks();
        }
    }

    [HttpGet]
    [Route("getLibraryBudget")]
    public ActionResult<float> GetLibraryBudget()
    {
        lock (logic)
        {
            return logic.GetLibraryBudget();
        }
    }
}