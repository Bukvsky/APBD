using System;
using System.Collections.Generic;

namespace SystemZarzadzaniaKontenerowcami
{
    public class ContainerShip
    {
        private string name { get; set; }
        private double maxKnots { get; set; }
        private int maxContainerCapacity { get; set; }
        private double maxCargoWeight { get; set; }
        public List<Container> Containers { get; set; }

        public ContainerShip(string name, double maxKnots, int maxContainerCapacity, double maxCargoWeight)
        {
            this.name = name;
            this.maxKnots = maxKnots;
            this.maxContainerCapacity = maxContainerCapacity;
            this.maxCargoWeight = maxCargoWeight;
            Containers = new List<Container>();
        }

        public double  sumCargoWeight()
        {
            int sumWeight = 0;
            foreach (Container container in Containers)
            {
                sumWeight += container.CargoWeight;
            }
            return sumWeight/1000;
        }
        public void UnloadContainer(string serialNumber)
        {
            var container = Containers.FirstOrDefault(c => c.SerialNumber == serialNumber);
            container?.UnloadCargo();
        }
        public void addContainer(Container container)
        {

            if (Containers.Count < maxContainerCapacity)
            {
                if (container.CargoWeight + sumCargoWeight() <= maxCargoWeight*1000)
                {
                    Containers.Add(container);
                }
                else
                {
                    throw new InvalidOperationException("Ship exceeds maximum cargo weight");
                }
            }
            else
            {
                throw new InvalidOperationException("Ship cannot contain more containers");
            }
            
         
        }

        public void LoadContainers(List<Container> containers)
        {
            foreach( Container container in containers)
            {
                addContainer(container);
            }
        }
        

        public void RemoveContainer(string serialNumber)
        {
            Container containerToRemove = Containers.Find(c => c.SerialNumber == serialNumber);
            if (containerToRemove != null)
            {
                Containers.Remove(containerToRemove);
                Console.WriteLine($"Container {serialNumber} has been removed from ship {this.name}.");
                
            }
            else
            {
                Console.WriteLine($"Container with serial number {serialNumber} not found on ship {this.name}.");
            }
        }
        
        public void DisplayContainers()
        {
            Console.WriteLine($"\n=== Containers on ship '{name}' ===");
    
            if (Containers.Count == 0)
            {
                Console.WriteLine("No containers loaded.");
                return;
            }
    
            foreach (var container in Containers)
            {   
                
                Console.WriteLine(container.ToString());
            }

            double currentWeight = sumCargoWeight();
            Console.WriteLine($"\nTotal cargo weight: {currentWeight:F2} / {maxCargoWeight} tons " +
                              $"({currentWeight/maxCargoWeight*100:F1}% of capacity)");
        }

        public void ReplaceContainer(string serialNumber, Container newContainer)
        {
            if (newContainer == null)
            {
                Console.WriteLine("Error: New container cannot be null.");
                return;
            }

            int index = Containers.FindIndex(c => c.SerialNumber == serialNumber);
    
            if (index == -1)
            {
                Console.WriteLine($"Error: Container with serial number '{serialNumber}' not found on ship '{name}'.");
                return;
            }
            
            double currentTotalWeight = sumCargoWeight();
            double oldContainerWeight = Containers[index].getContainerWeight() / 1000;
            double newContainerWeight = newContainer.getContainerWeight() / 1000;
            double newTotalWeight = currentTotalWeight - oldContainerWeight + newContainerWeight;

            if (newTotalWeight <= maxCargoWeight)
            {
                Containers[index] = newContainer;
                Console.WriteLine($"Success: Container {serialNumber} replaced with {newContainer.SerialNumber} " +
                                  $"on ship '{name}'. New total weight: {newTotalWeight:F2}/{maxCargoWeight} tons.");
            }
            else
            {
                Console.WriteLine($"Error: Cannot replace container {serialNumber}. " +
                                  $"New container would exceed max load capacity by " +
                                  $"{newTotalWeight - maxCargoWeight:F2} tons.");
            }
        }
               
            
            
            
        public void DisplayContainerInfo(string serialNumber)
        {
            var container = Containers.Find(c => c.SerialNumber == serialNumber);
            if (container != null)
            {
                container.ToString();
            }
            else
            {
                Console.WriteLine($"Container with serial number {serialNumber} not found on ship {name}.");
            }
        }
        public void DisplayShipInfo()
        {
            Console.WriteLine($"Ship {name}:");
            Console.WriteLine($"  Max Load Capacity: {maxCargoWeight} tons");
            Console.WriteLine($"  Current Load: {sumCargoWeight()} tons");
            Console.WriteLine("Containers on board:");
            DisplayContainers();
            Console.WriteLine("\n");
        }
    }
            
            
        }
    
