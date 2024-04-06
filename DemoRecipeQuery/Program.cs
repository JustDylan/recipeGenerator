using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RecipeQueryEngine;

namespace RecipeQueryConsoleApp
{
    public class Recipe
    {
        public string Label { get; set; }
        public List<string> Ingredients { get; set; }
        public string ImageUrl { get; set; }

        public void Display()
        {
            Console.WriteLine($"Recipe: {Label}");
            Console.WriteLine("Ingredients:");
            foreach (var ingredient in Ingredients)
            {
                Console.WriteLine($"- {ingredient}");
            }
            Console.WriteLine($"Image URL: {ImageUrl}");
            Console.WriteLine();
        }
    }

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

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static List<string> ParseIngredients(string input)
        {
            string[] ingredientsArray = input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> ingredients = new List<string>();
            foreach (string ingredient in ingredientsArray)
            {
                ingredients.Add(ingredient.Trim());
            }
            return ingredients;
        }

        private static async Task<List<Recipe>> SearchRecipesAsync(List<string> ingredients, bool requireAllIngredients)
        {
            using (var httpClient = new HttpClient())
            {
                string url = "https://api.edamam.com/search?";
                JArray hits;

                if (requireAllIngredients)
                {
                    url += $"q={string.Join(",", ingredients)}";
                }
                else
                {
                    List<JToken> allHits = new List<JToken>();

                    foreach (string ingredient in ingredients)
                    {
                        string queryUrl = $"q={ingredient}&app_id={AppId}&app_key={AppKey}";
                        HttpResponseMessage response = await httpClient.GetAsync($"{url}{queryUrl}");
                        response.EnsureSuccessStatusCode(); // Throw an exception if the request fails

                        string responseBody = await response.Content.ReadAsStringAsync();
                        JObject json = JObject.Parse(responseBody);
                        hits = (JArray)json["hits"];
                        allHits.AddRange(hits);
                    }

                    // Remove duplicates from the combined results
                    HashSet<string> uniqueRecipeLabels = new HashSet<string>();
                    JArray combinedHits = new JArray();
                    foreach (var hit in allHits)
                    {
                        string recipeLabel = hit["recipe"]["label"].ToString();
                        if (!uniqueRecipeLabels.Contains(recipeLabel))
                        {
                            uniqueRecipeLabels.Add(recipeLabel);
                            combinedHits.Add(hit);
                        }
                    }

                    return ExtractRecipes(combinedHits);
                }

                url += $"&app_id={AppId}&app_key={AppKey}";

                HttpResponseMessage mainResponse = await httpClient.GetAsync(url);
                mainResponse.EnsureSuccessStatusCode(); // Throw an exception if the request fails

                string mainResponseBody = await mainResponse.Content.ReadAsStringAsync();
                JObject mainJson = JObject.Parse(mainResponseBody);
                hits = (JArray)mainJson["hits"];
                return ExtractRecipes(hits);
            }
        }

        private static List<Recipe> ExtractRecipes(JArray recipes)
        {
            List<Recipe> extractedRecipes = new List<Recipe>();
            foreach (var hit in recipes)
            {
                Recipe recipe = new Recipe
                {
                    Label = hit["recipe"]["label"].ToString(),
                    Ingredients = ExtractIngredients(hit["recipe"]["ingredients"]),
                    ImageUrl = hit["recipe"]["image"].ToString()
                };
                extractedRecipes.Add(recipe);
            }
            return extractedRecipes;
        }

        private static List<string> ExtractIngredients(JToken ingredients)
        {
            List<string> extractedIngredients = new List<string>();
            foreach (var ingredient in ingredients)
            {
                extractedIngredients.Add(ingredient["text"].ToString());
            }
            return extractedIngredients;
        }

        private static void DisplayRecipes(List<Recipe> recipes)
        {
            foreach (var recipe in recipes)
            {
                recipe.Display();
            }
        }
    }
}