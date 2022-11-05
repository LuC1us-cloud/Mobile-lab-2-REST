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

    /// <summary>
    /// Check if liquid can be added to the capacity
    /// </summary>
    /// <returns>boolean</returns>
    [HttpGet]
    [Route("canAddLiquid")]
    public ActionResult<bool> CanAddLiquid()
    {
        lock (logic)
        {
            return logic.CanAddLiquid();
        }
    }

    /// <summary>
    /// Check if liquid can be subtracted from the capacity
    /// </summary>
    /// <returns>boolean</returns>
	[HttpGet]
    [Route("canSubtractLiquid")]
    public ActionResult<bool> CanSubtractLiquid()
    {
        lock (logic)
        {
            return logic.CanSubtractLiquid();
        }
    }

    /// <summary>
    /// Add liquid to the capacity
    /// </summary>
    /// <param name="amount">Amount of liquid to add</param>
    /// <returns>Amount of liquid to be added</returns>
    [HttpPost]
    [Route("addLiquid/{amount}")]
    public ActionResult<int> AddLiquid([FromRoute] int amount)
    {
        lock (logic)
        {
            return logic.AddLiquid(amount);
        }
    }

    /// <summary>
    /// Subtract liquid from the capacity
    /// </summary>
    /// <param name="amount">Amount of liquid to subtract</param>
    /// <returns>Amount of liquid to be subtracted</returns>
	[HttpPost]
    [Route("subtractLiquid/{amount}")]
    public ActionResult<int> SubtractLiquid([FromRoute] int amount)
    {
        lock (logic)
        {
            return logic.SubtractLiquid(amount);
        }
    }

    // palikta kaip pavyzdys

    // [HttpGet]
    // [Route("AddLiteral/{left}")]
    // public ActionResult<int> AddLiteral([FromRoute] int left, [FromQuery] int right)
    // {
    //     lock (logic)
    //     {
    //         return logic.AddLiteral(left, right);
    //     }
    // }
}