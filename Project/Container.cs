namespace CW2;

public abstract class Container
{
    private static int _counter = 1; // serial number generator

    public string SerialNumber { get; private set; }
    public double CargoWeight { get; set; }
    public double Height { get; set; }
    public double Depth { get; set; }
    public double NetWeight { get; set; }
    public double MaxCapacity { get; set; }

    public Container(string type, double height, double depth, double netWeight)
    {
        SerialNumber = GenerateSerialNumber(type);
        CargoWeight = 0;
        Height = height;
        Depth = depth;
        NetWeight = netWeight;
    }

    private string GenerateSerialNumber(string type)
    {
        return $"KON-{type}-{_counter++}";
    }

    public abstract void LoadCargo(double weight);

    public abstract void UnloadCargo();
    
    public void NotifyDangerousOperation(string containerNumber)
    {
        Console.WriteLine($"WARNING: Dangerous operation attempted on container {containerNumber}. Please check immediately!");
    }

    public virtual string GetInfo()
    {
        return $"{SerialNumber}: {CargoWeight}kg/{MaxCapacity}kg | {Height}cm | {Depth}cm | {NetWeight}kg";
    }
}