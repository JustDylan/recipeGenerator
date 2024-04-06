using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CougHacksApp.Model
{
    public class Recipe
    {
        public string Label { get; set; }
        public List<string> FoodItems { get; set; }
        public List<string> Ingredients { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }

        public void Display()
        {
            Console.WriteLine($"Recipe: {Label}");
            Console.WriteLine("FoodItems:");
            foreach (var ingredient in FoodItems)
            {
                Console.WriteLine($"- {ingredient}");
            }
            Console.WriteLine();
        }
    }
}
