using System;

namespace SystemZarzadzaniaKontenerowcami
{
    public enum CargoType
    {
        Regular,
        Hazardous
    }
    
    public class LiquidContainer : Container, IHazardNotifier
    {
        private CargoType CargoCategory { get; set; }

        public LiquidContainer(int cargoWeight, int height, int ownWeight, int depth, int maxLoadCapacity)
            : base("L", cargoWeight, height, ownWeight, depth,  maxLoadCapacity)
        {
            
        }

        public void SendNotification()
        {
            Console.WriteLine($"🚨 ALERT: Dangerous operation on container.");
        }

        public new  void LoadCargo(int weight, CargoType cargoType)
        {
            int availableCapacity = CargoCategory == CargoType.Regular
                ? (int)(MaxLoadCapacity * 0.9)
                : (int)(MaxLoadCapacity * 0.5);

            if (CargoWeight + weight > availableCapacity)
            {
                SendNotification();
                throw new OverfillException($"Attempting to load {weight} kg into container {SerialNumber}. Maximum allowed mass: {MaxLoadCapacity} kg");
                
            }
            
            base.LoadCargo(weight);
        }

        public override string ToString()
        {
            return base.ToString()+
                   $", Cargo category: {this.CargoCategory.ToString()}";
        }
    }
}