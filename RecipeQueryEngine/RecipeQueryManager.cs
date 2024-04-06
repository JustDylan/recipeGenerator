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

        /// <summary>
        /// Displays the recipes to the console (for testing).
        /// </summary>
        /// <param name="recipes"> The list of recipies to display. </param>
        public void DisplayRecipes(List<Recipe> recipes)
        {
            foreach (var recipe in recipes)
            {
                recipe.Display();
            }
        }

        /// <summary>
        /// Uses the Edamam API to search for a list of recipies that use the given list of ingredients.
        /// </summary>
        /// <param name="ingredients"> Will be used in the recipies that are returned. </param>
        /// <param name="requireAllIngredients"> If the list of recipies have to use all the ingredients given or not. </param>
        /// <returns> A list of recipe instances that used the given ingredients. </returns>
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

        /// <summary>
        /// Creates a list of recipe instances out of the JSON array.
        /// </summary>
        /// <param name="recipes"> The recipies to turn into recipe instances. </param>
        /// <returns> Returns a list of recipe class instances. </returns>
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
                    ImageUrl = hit["recipe"]["image"].ToString(),
                    Url = hit["recipe"]["url"].ToString()
                };
                extractedRecipes.Add(recipe);
            }
            return extractedRecipes;
        }

        /// <summary>
        /// Extracts the ingredients, i.e. '1/3 cup flour' from the JToken recipe, and turns them into strings.
        /// </summary>
        /// <param name="ingredients"> The ingredients attribute of the recipe from the API. </param>
        /// <returns> Returns strings of the ingredients. </returns>
        private List<String> ExtractIngredients(JToken ingredients)
        {
            List<string> extractedIngredients = new List<string>();
            foreach (var ingredient in ingredients)
            {
                extractedIngredients.Add(ingredient["text"].ToString());
            }
            return extractedIngredients;

        }

        /// <summary>
        /// Turns the fooditems from the JToken into a dictionary.
        /// </summary>
        /// <param name="fooditems"> The fooditems from the JSON recipe. </param>
        /// <returns> Returns a dictionary of fooditems and their quantity. </returns>
        private List<string> ExtractFoodItems(JToken fooditems)
        {
            List<String> extractedIngredients = new List<String>();
            foreach (var fooditem in fooditems)
            {
                string ingredientName = fooditem["foodCategory"].ToString();
                extractedIngredients.Add(ingredientName);
            }
            return extractedIngredients;
        }
    }
}
