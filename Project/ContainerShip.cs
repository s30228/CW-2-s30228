namespace CW2;

public class ContainerShip
{
    public List<Container> Containers { get; private set; } = new();
    public string ShipName { get; private set; }
    public double MaxSpeed { get; }
    public int MaxContainerCount { get; }
    public double MaxWeightCapacity { get; } // 1 t = 1000 kg

    public ContainerShip(string shipName, double maxSpeed, int maxContainerCount, double maxWeightCapacity)
    {
        ShipName = shipName;
        MaxSpeed = maxSpeed;
        MaxContainerCount = maxContainerCount;
        MaxWeightCapacity = maxWeightCapacity;
    }
    
    private void PrintMessage(string message) => Console.Write(message);
    private void WaitForEnter() { PrintMessage("Press Enter to return..."); Console.ReadLine(); }

    public bool TooMuchContainersCheck()
    {
        if (Containers.Count >= MaxContainerCount)
        {
            PrintMessage("Cannot load more containers: ship at full capacity!");
            WaitForEnter();
            return true;
        }
        return false;
    }

    public bool TooHeavyContainersCheck(Container container)
    {
        double totalWeight = Containers.Sum(c => c.CargoWeight + c.NetWeight) + container.CargoWeight + container.NetWeight;
        if (totalWeight > MaxWeightCapacity * 1000)
        {
            PrintMessage("Cannot load: Exceeds ship weight capacity!");
            WaitForEnter();
            return true;
        }
        return false;
    }

    public bool ContainerNotFoundCheck(Container container)
    {
        if (container == null)
        {
            PrintMessage("Container not found!");
            WaitForEnter();
            return true;
        }
        return false;
    }
    
    public void AddContainer()
    {
        try
        {
            Console.Clear();
        
            if (TooMuchContainersCheck()) return;
        
            PrintMessage("Enter container type (L/G/C): ");
            string type = Console.ReadLine();
            if (type != "L" && type != "G" && type != "C")
            {
                PrintMessage("Invalid container type!");
                WaitForEnter();
                return;
            }
            PrintMessage("Enter container height (cm): ");
            double height = double.Parse(Console.ReadLine());
            PrintMessage("Enter container depth (cm): ");
            double depth = double.Parse(Console.ReadLine());
            PrintMessage("Enter container net weight (kg): ");
            double weigth = double.Parse(Console.ReadLine());
        
            double totalWeight = Containers.Sum(c => c.CargoWeight + c.NetWeight) + weigth;
            if (totalWeight > MaxWeightCapacity * 1000)
            {
                PrintMessage("Cannot load: Exceeds ship weight capacity!\n");
                WaitForEnter();
                return;
            }
        
            switch (type)
            {
                case "L":
                {
                    PrintMessage("Is it hazardous? [Y/N]: ");
                    bool hazardous = Console.ReadLine().ToLower() == "y";
                    Containers.Add(new LiquidContainer(hazardous, height, depth, weigth));
                    PrintMessage("Liquid container created!\n");
                    break;
                }
                case "G":
                {
                    PrintMessage("Enter container pressure (atm): ");
                    double pressure = double.Parse(Console.ReadLine());
                    Containers.Add(new GasContainer(pressure, height, depth, weigth));
                    PrintMessage("Gas container created!\n");
                    break;
                }
                case "C":
                {
                    PrintMessage("Enter container temperature (C): ");
                    double temperature = double.Parse(Console.ReadLine());
                    Containers.Add(new RefrigeratedContainer(temperature, height, depth, weigth));
                    PrintMessage("Refrigerated container created!");
                    break;
                }
            }
            WaitForEnter();
        }
        catch (Exception e)
        {
            PrintMessage(e.Message + "\n");
            WaitForEnter();
        }
    }

    public void LoadContainer()
    {
        Console.Clear();
        if (!Containers.Any()) { PrintMessage("No containers available.\n"); WaitForEnter(); return; }
        PrintMessage($"Containers on this ship: {GetContainersSerial()}\n");
        PrintMessage("Enter serial number of container to load: ");
        string serialNumber = Console.ReadLine();
        var container = Containers.FirstOrDefault(c => c.SerialNumber == serialNumber);
        
        if (ContainerNotFoundCheck(container)) return;
        
        PrintMessage("Enter cargo weight (kg): ");
        double weight = double.Parse(Console.ReadLine());
        
        try
        {
            if (container is RefrigeratedContainer refrigeratedContainer)
            {
                PrintMessage("Enter product: ");
                string product = Console.ReadLine();
                refrigeratedContainer.LoadCargo(product, weight);
            }
            else
            {
                container.LoadCargo(weight);
            }
        }
        catch (Exception e)
        {
            PrintMessage(e.Message + "\n");
        }
        WaitForEnter();
    }
    
    public void UnloadContainer()
    {
        Console.Clear();
        if (!Containers.Any()) { PrintMessage("No containers available.\n"); WaitForEnter(); return; }
        PrintMessage($"Containers on this ship: {GetContainersSerial()}\n");
        PrintMessage("Enter serial number of container to unload: ");
        string serialNumber = Console.ReadLine();
        
        var container = Containers.FirstOrDefault(c => c.SerialNumber == serialNumber);
        if (ContainerNotFoundCheck(container)) return;
        
        container.UnloadCargo();
        
        WaitForEnter();
    }

    /*public void LoadContainers(List<Container> containers)
    {
        if (containers.Count > MaxContainerCount || (containers.Count + Containers.Count) > MaxContainerCount)
        {
            throw new InvalidOperationException("Cannot load more containers: ship at full capacity!");
        }

        foreach (var container in containers)
        {
            LoadContainer(container);
        }
    }*/

    public void RemoveContainer()
    {
        Console.Clear();
        if (!Containers.Any()) { PrintMessage("No containers available.\n"); WaitForEnter(); return; }
        PrintMessage($"Containers on this ship: {GetContainersSerial()}\n");
        PrintMessage("Enter container's serial number: ");
        string serialNumber = Console.ReadLine();
        
        var container = Containers.FirstOrDefault(c => c.SerialNumber == serialNumber);
        if (ContainerNotFoundCheck(container)) return;

        Containers.Remove(container);
        PrintMessage($"Removed container {container.SerialNumber}.\n");
        WaitForEnter();
    }

    public void ReplaceContainer()
    {
        Console.Clear();
        if (!Containers.Any()) { PrintMessage("No containers available.\n"); WaitForEnter(); return; }
        PrintMessage($"Containers on this ship: {GetContainersSerial()}\n");
        PrintMessage("Enter container's serial number you want to replace: ");
        string oldContainerSerial = Console.ReadLine();
        
        var oldContainer = Containers.FirstOrDefault(c => c.SerialNumber == oldContainerSerial);
        if (ContainerNotFoundCheck(oldContainer)) return;
        int prevCount = Containers.Count;  
        AddContainer();
        var newContainer = Containers.Count > prevCount ? Containers.Last() : null; 
        if (newContainer != null)
        {
            Containers.Remove(oldContainer);
            PrintMessage($"Replaced container {oldContainer.SerialNumber} with {newContainer.SerialNumber}.\n");
        }
        else
        {
            PrintMessage("Error: No container was added.\n");
        }
        WaitForEnter();
    }

    public void TransferContainer(ContainerShip to)
    {
        PrintMessage($"Containers on current ship: {GetContainersSerial()}\n");
        PrintMessage($"Containers on other ship: {to.GetContainersSerial()}\n");
        PrintMessage("Enter container's serial number you want to transfer: ");
        var serialNumber = Console.ReadLine();
        
        var container = Containers.FirstOrDefault(c => c.SerialNumber == serialNumber);
        if (ContainerNotFoundCheck(container)) return;
        if (to.TooMuchContainersCheck()) return;
        if (to.TooHeavyContainersCheck(container)) return;
        Containers.Remove(container);
        to.Containers.Add(container);
        PrintMessage($"Transfer of {container.SerialNumber} from {ShipName} to {to.ShipName} completed!\n");
        WaitForEnter();
    }

    public string GetInfo()
    {
        string info = $"{ShipName}: {MaxSpeed} knots | {Containers.Count}/{MaxContainerCount} containers | {MaxWeightCapacity}t\n";
        foreach (var container in Containers)
        {
            info += "\t" + container.GetInfo() + "\n";
        }
        return info;
    }

    public string GetContainersSerial()
    {
        return string.Join(", ", Containers.Select(c => c.SerialNumber));
    }
}