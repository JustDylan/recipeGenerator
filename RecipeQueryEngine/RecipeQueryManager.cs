namespace RecipeQueryEngine
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    public class RecipeQueryManager
    {
        private const string AppId = "16d65f0a";
        private const string AppKey = "6ed317d144816d26818653de32091b04";
        private HttpClient _httpClient;

        public RecipeQueryManager()
        {
            _httpClient = new HttpClient();
        }

        public class Recipe
        {
            public string Label { get; set; }
            public Dictionary<string, double> FoodItems { get; set; } // Food item name -> Count
            public List<string> Ingredients { get; set; }
            public string ImageUrl { get; set; }

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

        public void DisplayRecipes(List<Recipe> recipes)
        {
            foreach (var recipe in recipes)
            {
                recipe.Display();
            }
        }

        public async Task<List<Recipe>> SearchRecipesAsync(List<string> ingredients, bool requireAllIngredients)
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

        private List<Recipe> ExtractRecipes(JArray recipes)
        {
            List<Recipe> extractedRecipes = new List<Recipe>();
            foreach (var hit in recipes)
            {
                Recipe recipe = new Recipe
                {
                    Label = hit["recipe"]["label"].ToString(),
                    FoodItems = ExtractFoodItems(hit["recipe"]["ingredients"]),
                    Ingredients = ExtractIngredients(hit["recipe"]["ingredients"]),
                    ImageUrl = hit["recipe"]["image"].ToString()
                };
                extractedRecipes.Add(recipe);
            }
            return extractedRecipes;
        }
        private List<String> ExtractIngredients(JToken ingredients)
        {
            List<string> extractedIngredients = new List<string>();
            foreach (var ingredient in ingredients)
            {
                extractedIngredients.Add(ingredient["text"].ToString());
            }
            return extractedIngredients;

        }

    
        private Dictionary<string, double> ExtractFoodItems(JToken fooditems)
        {
            Dictionary<string, double> extractedIngredients = new Dictionary<string, double>();
            foreach (var fooditem in fooditems)
            {
                string ingredientName = fooditem["foodCategory"].ToString();
                double ingredientCount = double.Parse(fooditem["quantity"].ToString()); // Or whatever attribute represents the count

                // Check if the fooditem already exists in the dictionary
                if (extractedIngredients.ContainsKey(ingredientName))
                {
                    // If the fooditem exists, add the count to its existing value
                    extractedIngredients[ingredientName] += ingredientCount;
                }
                else
                {
                    // If the fooditem does not exist, add it to the dictionary with its count
                    extractedIngredients.Add(ingredientName, ingredientCount);
                }
            }
            return extractedIngredients;
        }
    }
}
