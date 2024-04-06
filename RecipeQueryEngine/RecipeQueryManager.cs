namespace RecipeQueryEngine
{
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    using Npgsql;

    public class RecipeQueryManager
    {
        public async Task<List<Recipe>> GetRecipesFromIngredientsAsync(List<string> ingredients)
        {
            var recipes = new List<Recipe>();
            var connectionString = "Host=localhost;Username=postgres;Password=1;Database=recipedatabase";

            // Construct the SQL query dynamically based on the ingredients list
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT r.recipe_name, i.fooditem, r.image_url, r.url ");
            queryBuilder.Append("FROM recipes r ");
            queryBuilder.Append("INNER JOIN ingredients i ON r.id = i.recipe_id ");
            queryBuilder.Append("WHERE i.fooditem IN (");

            // Append each ingredient to the query
            for (int i = 0; i < ingredients.Count; i++)
            {
                queryBuilder.Append($"'{ingredients[i]}'");
                if (i < ingredients.Count - 1)
                    queryBuilder.Append(", ");
            }

            queryBuilder.Append(")");

            // Execute the query
            await using var dataSource = NpgsqlDataSource.Create(connectionString);
            await using var command = dataSource.CreateCommand(queryBuilder.ToString());
            await using var reader = await command.ExecuteReaderAsync();

            // Read the results and construct Recipe instances
            while (await reader.ReadAsync())
            {
                var recipeLabel = reader.GetString(0);
                var foodItem = reader.GetString(1);
                var imageUrl = reader.GetString(2);
                var url = reader.GetString(3);

                // Check if the recipe already exists in the list
                var existingRecipe = recipes.FirstOrDefault(r => r.Label == recipeLabel);
                if (existingRecipe == null)
                {
                    // If the recipe doesn't exist, create a new Recipe instance
                    existingRecipe = new Recipe
                    {
                        Label = recipeLabel,
                        ImageUrl = imageUrl,
                        Url = url,
                        FoodItems = new List<string>()
                    };
                    recipes.Add(existingRecipe);
                }

                // Add food item to the recipe
                existingRecipe.FoodItems.Add(foodItem);
            }

            return recipes;
        }
    }
}
