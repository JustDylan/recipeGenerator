using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeQueryEngine
{
    public class FoodItemQueryManager
    {
        private const string AppId = "4596fd8c";
        private const string AppKey = "465f8acbcbc044776b72ba3a68b72013";

        public async Task<List<string>> SearchFoodItemsAsync(string searchTerm)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = $"https://api.edamam.com/api/food-database/v2/parser?ingr={searchTerm}&app_id={AppId}&app_key={AppKey}";

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return ParseSearchResults(responseBody);
                }
                else
                {
                    // Handle unsuccessful response
                    Console.WriteLine($"Failed to fetch search results. Status code: {response.StatusCode}");
                    return new List<string>(); // Return an empty list
                }
            }
        }

        private List<string> ParseSearchResults(string responseBody)
        {
            List<string> foodItems = new List<string>();
            JObject json = JObject.Parse(responseBody);
            JArray hints = (JArray)json["hints"];
            foreach (var hint in hints)
            {
                string foodItem = hint["food"]["label"].ToString();
                foodItems.Add(foodItem);
            }
            return foodItems;
        }

        public static List<string> ParseIngredients(string input)
        {
            string[] ingredientsArray = input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> ingredients = new List<string>();
            foreach (string ingredient in ingredientsArray)
            {
                ingredients.Add(ingredient.Trim());
            }
            return ingredients;
        }
    }
}
