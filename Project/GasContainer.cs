namespace CW2;

public class GasContainer : Container, IHazardNotifier
{
    public double Pressure { get; private set; }

    public GasContainer(double pressure, double height, double depth, double netWeight) : base("G", height, depth, netWeight)
    {
        Pressure = pressure;
        MaxCapacity = 8000;
    }
    
    public override void LoadCargo(double weight)
    {
        if (CargoWeight + weight > MaxCapacity)
        {
            NotifyDangerousOperation(SerialNumber);
            throw new OverfillException("Cannot load more cargo, container overfill!");
        }
        CargoWeight += weight;
        Console.WriteLine($"Loaded {weight}kg into {SerialNumber}");
    }

    public override void UnloadCargo()
    {
        CargoWeight *= 0.05;
        Console.WriteLine($"Unloaded {SerialNumber}");
    }
    
    public override string GetInfo()
    {
        return (base.GetInfo() + $" | {Pressure}atm");
    }
}