namespace CW2;

public class LiquidContainer : Container, IHazardNotifier
{
    public bool IsHazardous { get; set; }

    public LiquidContainer(bool isHazardous, double height, double depth, double netWeight) : base("L", height, depth, netWeight)
    {
        IsHazardous = isHazardous;
        MaxCapacity = 12000;
    }

    public override void LoadCargo(double weight)
    {
        double maxLoad = IsHazardous ? MaxCapacity * 0.5 : MaxCapacity * 0.9;
        if (CargoWeight + weight > maxLoad)
        {
            // Reporting a dangerous operation
            NotifyDangerousOperation(SerialNumber);
            throw new OverfillException("Cannot load more cargo, container overfill or dangerous operation!");
        }
        CargoWeight += weight;
        Console.WriteLine($"Loaded {weight}kg into {SerialNumber}");
    }

    public override void UnloadCargo()
    {
        CargoWeight = 0;
        Console.WriteLine($"Unloaded {SerialNumber}");
    }
    
    public override string GetInfo()
    {
        var hazardous = IsHazardous ? "Hazardous" : "Non hazardous";
        return (base.GetInfo() + $" | {hazardous}");
    }
}