using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace anothermidtermBSdraft
{
    public class InventoryItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; } 
        public decimal Price { get; set; }
        public string ImagePath { get; set; }

        public InventoryItem(string name, string description, int stock, decimal price, string imagePath)
        {
            Name = name;
            Description = description;
            Stock = stock; 
            Price = price;
            ImagePath = imagePath;
        }

        public override string ToString()
        {
            return $"{Name} - {Description} - {Stock} - {Price}"; 
        }
    }
}
