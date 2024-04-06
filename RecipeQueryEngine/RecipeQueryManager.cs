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

        public async Task<JArray> SearchRecipesAsync(List<string> ingredients, bool requireAllIngredients)
        {
            string url = "https://api.edamam.com/search?";

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
                    HttpResponseMessage response = await _httpClient.GetAsync($"{url}{queryUrl}");
                    response.EnsureSuccessStatusCode(); // Throw an exception if the request fails

                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(responseBody);
                    JArray hits = (JArray)json["hits"];
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

                return combinedHits;
            }

            url += $"&app_id={AppId}&app_key={AppKey}";

            HttpResponseMessage mainResponse = await _httpClient.GetAsync(url);
            mainResponse.EnsureSuccessStatusCode(); // Throw an exception if the request fails

            string mainResponseBody = await mainResponse.Content.ReadAsStringAsync();
            JObject mainJson = JObject.Parse(mainResponseBody);
            return (JArray)mainJson["hits"];
        }
    }
}
