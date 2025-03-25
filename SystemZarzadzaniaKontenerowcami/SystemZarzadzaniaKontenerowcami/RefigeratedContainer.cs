using System;
using System.Collections.Generic;


namespace SystemZarzadzaniaKontenerowcami
{
    public enum Product
    {
        Bananas,
        Chocolate,
        Fish,
        Meat,
        IceCream,
        FrozenPizza,
        Cheese,
        Sausages,
        Butter,
        Eggs
    }

    public class RefrigeratedContainer : Container
    {
        private Dictionary<Product, double> products = new Dictionary<Product, double>
        {
            { Product.Bananas, 13.3 },
            { Product.Chocolate, 18.0 },
            { Product.Fish, 2.0 },
            { Product.Meat, -15.0 },
            { Product.IceCream, -15.0 },
            { Product.FrozenPizza, -30.0 },
            { Product.Cheese, 7.2 },
            { Product.Sausages, 5.0 },
            { Product.Butter, 20.5 },
            { Product.Eggs, 19.0 }
        };
        
        private Product? product { get; set; }
        private double temperature { get; set; }
        
        public RefrigeratedContainer(int cargoWeight, int height, int ownWeight, int depth, int maxLoadCapacity, Product product)
            : base("R", cargoWeight, height, ownWeight, depth,  maxLoadCapacity)
        {
            if (!products.ContainsKey(product))
            {
                Console.WriteLine($"The product {product} is not allowed to be transported in container {SerialNumber}");
            }
            this.product = product;
            this.temperature = products[product];
        }

        public void SendNotification(string message)
        {
            Console.WriteLine($"🚨 ALERT: Dangerous operation on container.");
        }

        public new void LoadCargo(int weight, Product productToLoad)
        {
            if (CargoWeight > 0)
            {
                if (product == productToLoad)
                {
                    base.LoadCargo(weight);
                }
                else
                {
                    Console.WriteLine("You cannot load two different products onto one container");
                    
                }
            }
            else
            {
                base.LoadCargo(weight);
            }
        }

        public override string ToString()
        {
            return base.ToString()+
                   $", Product type: {this.product.ToString()}"+
                   $", temperature: {this.temperature:F2} C";
        }
    }
}