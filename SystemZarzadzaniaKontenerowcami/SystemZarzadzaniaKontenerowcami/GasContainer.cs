using System;

namespace SystemZarzadzaniaKontenerowcami
{
    public class GasContainer : Container, IHazardNotifier
    {
        public int Pressure { get; set; }
        
        public GasContainer(int cargoWeight, int height, int ownWeight, int depth, int maxLoadCapacity, int pressure ) 
            : base("G", cargoWeight, height, ownWeight, depth, maxLoadCapacity)
        {
            Pressure = pressure;
        }
        public void SendNotification()
        {
            Console.WriteLine($"🚨 ALERT: Dangerous operation on container.");
        }

     

        public new void UnloadCargo()
        {
            int remainingCargo = (int)(MaxLoadCapacity * 0.05);
            CargoWeight = remainingCargo;
            Console.WriteLine($"Container {SerialNumber} unloaded. Remaining cargo: {remainingCargo}");
        }

        public override string ToString()
        {
            return base.ToString()+
                   $", Pressure: {Pressure}";
        }
    }
}