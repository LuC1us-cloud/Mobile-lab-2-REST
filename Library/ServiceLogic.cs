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

    public bool CanSubtractLiquid()
    {
        if (Library.capacity > Library.upperBound)
        {
            Library.clientIsActive = true;
            return true;
        }
        return false;
    }
    public bool CanAddLiquid()
    {
        if (Library.capacity < Library.lowerBound)
        {
            Library.clientIsActive = true;
            return true;
        }
        return false;
    }

    public int SubtractLiquid(int amount)
    {
        if (Library.capacity - amount < Library.upperBound)
        {
            var startingCapacity = Library.capacity;
            log.Info($"Capacity before subtracting: {startingCapacity}");
            int overpumped = Library.upperBound - (startingCapacity - amount);
            Library.capacity = Library.upperBound;
            log.Info($"Capacity after subtracting: {Library.capacity}");
            log.Info($"Amount of liquid thrown away: {overpumped}");
            log.Info("\n");
            return startingCapacity - Library.upperBound;
        }
        else
        {
            log.Info("Capacity before subtracting: " + Library.capacity);
            Library.capacity -= amount;
            log.Info($"Capacity after subtracting: {Library.capacity}");
            log.Info("\n");
            return amount;
        }
    }

    public int AddLiquid(int amount)
    {
        if (Library.capacity + amount > Library.lowerBound)
        {
            var startingCapacity = Library.capacity;
            log.Info($"Capacity before adding: {startingCapacity}");
            int overfilled = (startingCapacity + amount) - Library.lowerBound;
            Library.capacity = Library.lowerBound;
            log.Info($"Capacity after adding: {Library.capacity}");
            log.Info($"Amount of liquid thrown away: {overfilled}");
            return Library.lowerBound - startingCapacity;
        }
        else
        {
            log.Info("Capacity before adding: " + Library.capacity);
            Library.capacity += amount;
            log.Info($"Capacity after adding: {Library.capacity}");
            return amount;
        }
    }
}