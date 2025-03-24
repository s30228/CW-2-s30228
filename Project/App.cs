namespace CW2;

// Tested by Krasnitskaya Maryia s30355 14c

class App
{
    static List<ContainerShip> Ships = new List<ContainerShip>();
    
    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("== Container Ship Management ==");
            Console.WriteLine("List of container ships: " + (Ships.Count == 0 ? "Lack" : Ships.Count + " ship(s)"));
            var containersCount = 0;
            foreach (var ship in Ships) { containersCount += ship.Containers.Count; }
            Console.WriteLine("List of containers: " + (containersCount == 0 ? "Lack" : containersCount + " container(s)"));
            Console.WriteLine("Possible actions:");
            Console.WriteLine("1. Add a container ship");
            Console.WriteLine("2. View ships and containers");
            Console.WriteLine("3. Manage containers");
            Console.WriteLine("4. Delete a ship");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");
            
            switch (Console.ReadLine())
            {
                case "1": AddShip(); break;
                case "2": ViewShips(); break;
                case "3": ManageContainers(); break;
                case "4": DeleteShip(); break;
                case "5": return;
                default: Console.WriteLine("Invalid option. Press Enter to continue..."); Console.ReadLine(); break;
            }
        }
    }
    
    static void AddShip()
    {
        try
        {
            Console.Clear();
            Console.Write("Enter ship name: ");
            string name = Console.ReadLine();
            if (Ships.Any(s => s.ShipName.Equals(name, StringComparison.OrdinalIgnoreCase))) {Console.WriteLine("Ship with this name already exists. Press Enter..."); Console.ReadLine(); return; }
            Console.Write("Enter max speed (knots): ");
            double maxSpeed = double.Parse(Console.ReadLine());
            Console.Write("Enter max container count: ");
            int maxContainers = int.Parse(Console.ReadLine());
            Console.Write("Enter max weight (tons): ");
            double maxWeight = double.Parse(Console.ReadLine());
            Ships.Add(new ContainerShip(name, maxSpeed, maxContainers, maxWeight));
            Console.WriteLine("Ship added! Press Enter to return...");
            Console.ReadLine();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Press Enter to return...");
            Console.ReadLine();
        }
    }
    
    static void ViewShips()
    {
        Console.Clear();
        if (Ships.Count == 0) { Console.WriteLine("No ships available."); }
        else
        {
            foreach (var ship in Ships)
            {
                Console.WriteLine(ship.GetInfo());
            }
        }
        Console.WriteLine("Press Enter to return...");
        Console.ReadLine();
    }
    
    static void ManageContainers()
    {
        Console.Clear();
        if (Ships.Count == 0) { Console.WriteLine("No ships available. Press Enter..."); Console.ReadLine(); return; }
        Console.Clear();
        Console.WriteLine("Select a ship by index:");
        for (int i = 0; i < Ships.Count; i++) { Console.WriteLine($"{i + 1}. {Ships[i].ShipName}"); }
        Console.Write("Enter ship number: ");
        int shipIndex = int.Parse(Console.ReadLine()) - 1;
        
        if (shipIndex < 0 || shipIndex >= Ships.Count) { Console.WriteLine("Invalid index. Press Enter..."); Console.ReadLine(); return; }
        
        Console.WriteLine("1. Add container"); //done
        Console.WriteLine("2. Load cargo"); //done
        Console.WriteLine("3. Unload cargo"); //done
        Console.WriteLine("4. Replace container"); //done
        Console.WriteLine("5. Transfer container"); //done
        Console.WriteLine("6. Remove container"); //done
        Console.Write("Choose: ");
        switch (Console.ReadLine())
        {
            case "1": Ships[shipIndex].AddContainer(); break;
            case "2": Ships[shipIndex].LoadContainer(); break;
            case "3": Ships[shipIndex].UnloadContainer(); break;
            case "4": Ships[shipIndex].ReplaceContainer(); break;
            case "5":
            {
                Console.Clear();
                if (Ships.Count == 1) { Console.WriteLine("Only one ship available. Press Enter..."); Console.ReadLine(); return; }
                var containersCount = 0;
                foreach (var shipp in Ships) { containersCount += shipp.Containers.Count; }
                if (containersCount == 0) { Console.WriteLine("No containers available. Press Enter..."); Console.ReadLine(); return; }
                Console.WriteLine($"Available ships: {GetShipsNames(shipIndex)}");
                Console.Write("Enter ship name to transfer to: ");
                var name = Console.ReadLine();
                if (!Ships.Any(s => s.ShipName.Equals(name, StringComparison.OrdinalIgnoreCase))) { Console.WriteLine("Invalid ship name. Press Enter to return..."); Console.ReadLine(); return; }
                var ship = Ships.FirstOrDefault(s => s.ShipName == name);
                Ships[shipIndex].TransferContainer(ship);
                break;
            }
            case "6": Ships[shipIndex].RemoveContainer(); break;
            default: Console.WriteLine("Invalid option. Press Enter..."); Console.ReadLine(); break;
        }
    }
    
    static void DeleteShip()
    {
        Console.Clear();
        if (Ships.Count == 0) { Console.WriteLine("No ships available. Press Enter..."); Console.ReadLine(); return; }
        Console.WriteLine("Select a ship to delete:");
        for (int i = 0; i < Ships.Count; i++) { Console.WriteLine($"{i + 1}. {Ships[i].ShipName}"); }
        Console.Write("Enter ship number: ");
        int shipIndex = int.Parse(Console.ReadLine()) - 1;
        
        if (shipIndex < 0 || shipIndex >= Ships.Count) { Console.WriteLine("Invalid choice. Press Enter..."); Console.ReadLine(); return; }
        
        if (Ships[shipIndex].Containers.Count > 0) { Console.WriteLine("Cannot delete a ship with containers! Press Enter..."); Console.ReadLine(); return; }
        
        Ships.RemoveAt(shipIndex);
        Console.WriteLine("Ship deleted. Press Enter...");
        Console.ReadLine();
    }

    public static string GetShipsNames(int excludeIndex)
    {
        return string.Join(", ", Ships
            .Where((s, i) => i != excludeIndex) // Exclude selected ship
            .Select(s => s.ShipName));
    }
}