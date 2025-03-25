using System;

namespace SystemZarzadzaniaKontenerowcami
{
  internal class Program
    {
        public static void Main(string[] args)
        {
            LiquidContainer liquidContainer = new LiquidContainer(
                0, 259, 2300, 606, 28000);
        
            GasContainer gasContainer = new GasContainer(
                0, 230, 2800, 590, 22000, 1);
        
            RefrigeratedContainer bananaContainer = new RefrigeratedContainer(
                0, 259, 3100, 606, 25000, Product.Bananas);
        
            ContainerShip everGreen = new ContainerShip(
                "EverGreen",
                20.4,
                200,
                5600);
            Console.WriteLine("\nEmpty Ship:");
            everGreen.DisplayShipInfo();
            try
            {
                liquidContainer.LoadCargo(10000, CargoType.Hazardous);
                gasContainer.LoadCargo(20000);
                bananaContainer.LoadCargo(21000, Product.Bananas);
                
                everGreen.addContainer(liquidContainer);
                everGreen.addContainer(gasContainer);
                everGreen.addContainer(bananaContainer);
            }
            catch (OverfillException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("\nBefore operation:");
            everGreen.DisplayShipInfo();
        
           
            everGreen.UnloadContainer(bananaContainer.SerialNumber);
            everGreen.RemoveContainer(gasContainer.SerialNumber);
        
            Console.WriteLine("\nAfter operation:");
            everGreen.DisplayShipInfo();
            
        
        }
        
        
    }
}