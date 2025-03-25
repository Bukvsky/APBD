using System;

namespace SystemZarzadzaniaKontenerowcami
{
    public abstract class Container
    {

        public int CargoWeight { get; set; }
        public int Height { get; private set; }
        public int OwnWeight { get; private set; }
        public int Depth { get; private set; }
        public int MaxLoadCapacity { get; set; }
        public string SerialNumber { get; private set; }
        private static int counter = 1;

        public Container(string type, int cargoWeight, int height, int ownWeight, int depth, int maxLoadCapacity)
        {
            if (cargoWeight > maxLoadCapacity)
            {
                throw new ArgumentException("Cargo weight cannot exceed the maximum load capacity!");
            }

            this.CargoWeight = cargoWeight;
            this.Height = height;
            this.OwnWeight = ownWeight;
            this.Depth = depth;
            this.SerialNumber = $"KON-{type}-{counter++}";
            this.MaxLoadCapacity = maxLoadCapacity;
        }

        public void LoadCargo(int weight)
        {
            if (weight > MaxLoadCapacity)
            {
                throw new OverflowException("Maximum load capacity exceeded!");
            }

            CargoWeight += weight;
        }

        public void UnloadCargo()
        {
            CargoWeight = 0;
        }

        public int getContainerWeight(){
            return CargoWeight+OwnWeight; 
    }


    public override string ToString()
        {
            return $"- {SerialNumber}: {GetType().Name}, " +
                   $"Cargo: {CargoWeight}kg, " +
                   $"Total weight: {getContainerWeight()/1000:F2} tons";
        }
    }
}