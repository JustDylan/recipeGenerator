using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RecipeQueryEngine;

namespace RecipeQueryConsoleApp
{

    class Program
    {
        private const string AppId = "16d65f0a";
        private const string AppKey = "6ed317d144816d26818653de32091b04";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter a food item to search:");
            string searchTerm = Console.ReadLine();

            try
            {
                // Create an instance of FoodItemQueryManager
                FoodItemQueryManager foodItemQueryManager = new FoodItemQueryManager();

                // Call the SearchFoodItems method
                List<string> foodItems = await foodItemQueryManager.SearchFoodItemsAsync(searchTerm);

                // Display the retrieved food items
                Console.WriteLine("\nFood items found:");
                foreach (var foodItem in foodItems)
                {
                    Console.WriteLine(foodItem);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Enter a list of ingredients separated by commas:");
            string input = Console.ReadLine();
            List<string> ingredients = FoodItemQueryManager.ParseIngredients(input);
            try
            {
                RecipeQueryManager recipeQueryManager = new RecipeQueryManager();
                var recipesWithAllIngredients = await recipeQueryManager.SearchRecipesAsync(ingredients, true);
                var simplerRecipes = await recipeQueryManager.SearchRecipesAsync(ingredients, false);
                Console.WriteLine("Recipes that use all of the given ingredients:");
                recipeQueryManager.DisplayRecipes(recipesWithAllIngredients);

                Console.WriteLine("\nSimpler recipes that use only a few from the given list:");
                recipeQueryManager.DisplayRecipes(simplerRecipes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}