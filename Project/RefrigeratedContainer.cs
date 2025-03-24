namespace CW2;

public class RefrigeratedContainer : Container
{
    public string Product { get; private set; }
    public double Temperature { get; private set; }
    private static readonly Dictionary<string, double> ProductTemperatures = new()
    {
        {"Bananas", 13.3},
        {"Chocolate", 18},
        {"Fish", 2},
        {"Meat", -15},
        {"Ice Cream", -18},
        {"Frozen Pizza", -30},
        {"Cheese", 7.2},
        {"Sausages", 5},
        {"Butter", 20.5},
        {"Eggs", 19}
    };
    
    public RefrigeratedContainer(double temperature, double height, double depth, double netWeight) : base("C", height, depth, netWeight)
    {
        MaxCapacity = 10000;
        Temperature = temperature;
    }
    
    public override void LoadCargo(double weight)  
    {
        throw new InvalidOperationException("Refrigerated containers require a product type.");
    }
    
    public void LoadCargo(string product, double weight)
    {
        if (!ProductTemperatures.ContainsKey(product))
        {
            throw new ArgumentException($"Product {product} is not supported.");
        }

        double requiredTemp = ProductTemperatures[product];

        if (Product != null && Product != product)
        {
            throw new InvalidOperationException($"Cannot mix products! This container already holds {Product}.");
        }
        
        if (Temperature.CompareTo(requiredTemp) != 0)
        {
            throw new InvalidOperationException($"Container temperature ({Temperature}C) is too warm or too cold for {product}.");
        }

        if (CargoWeight + weight > MaxCapacity)
        {
            throw new OverfillException("Cannot load more cargo, container overfill!");
        }

        Product = product;
        CargoWeight += weight;
        Console.WriteLine($"Loaded {product} ({weight}kg) into {SerialNumber}");
    }
    
    public override void UnloadCargo()
    {
        CargoWeight = 0;
        Console.WriteLine($"Unloaded {SerialNumber}");
    }

    public override string GetInfo()
    {
        string product = Product == null ? "Empty" : Product;
        return (base.GetInfo() + $" | {product} | {Temperature}C");
    }
}