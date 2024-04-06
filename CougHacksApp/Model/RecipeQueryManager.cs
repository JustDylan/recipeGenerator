namespace RecipeQueryEngine
{
    using System.Text;
    using System.Threading.Tasks;
    using Npgsql;

    public class RecipeQueryManager
    {
        public async Task<List<Recipe>> GetRecipesFromIngredientsAsync(List<string> ingredients)
        {
            var recipes = new List<Recipe>();
            var connectionString = "Host=localhost;Username=postgres;Password=1;Database=recipedatabase";

            // Construct the SQL query dynamically based on the ingredients list
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT r.id, r.title, r.link ");
            queryBuilder.Append("FROM recipes AS r ");
            queryBuilder.Append("INNER JOIN ingredients AS i ON r.id = i.recipe_id ");
            queryBuilder.Append("WHERE i.fooditem IN ( ");

            string selectedFoodItems = string.Empty;
            // Append each ingredient to the query
            for (int i = 0; i < ingredients.Count; i++)
            {
                selectedFoodItems += "'" + ingredients[i] + "'" +
                    ((i < ingredients.Count - 1) ? ", " : string.Empty);
            }

            queryBuilder.Append(selectedFoodItems);
            queryBuilder.Append(") GROUP BY r.id HAVING COUNT(r.id) >= 2");

            // Execute the query
            await using var dataSource = NpgsqlDataSource.Create(connectionString);
            await using var command = dataSource.CreateCommand(queryBuilder.ToString());
            await using var reader = await command.ExecuteReaderAsync();

            // Read the results and construct Recipe instances
            while (await reader.ReadAsync())
            {
                int recipeId = reader.GetInt32(0);
                var recipeLabel = reader.GetString(1);
                var url = reader.GetString(2);

                // Check if the recipe already exists in the list
                var existingRecipe = recipes.FirstOrDefault(r => r.Label == recipeLabel);
                if (existingRecipe == null)
                {
                    // query food items that belong the the recipe
                    string foodItemQuery =
                        "SELECT i.fooditem, i.ingredient_name " +
                        "FROM ingredients as i " +
                        "WHERE i.recipe_id = " + recipeId;

                    await using var foodItemCommand = dataSource.CreateCommand(foodItemQuery);
                    await using var foodItemReader = await foodItemCommand.ExecuteReaderAsync();

                    HashSet<string> foodItems = new HashSet<string>();
                    List<string> ingredientList = new List<string>();

                    while (await foodItemReader.ReadAsync())
                    {
                        foodItems.Add(foodItemReader.GetString(0));
                        ingredientList.Add(foodItemReader.GetString(1));
                    }

                        // If the recipe doesn't exist, create a new Recipe instance
                        existingRecipe = new Recipe
                    {
                        ID = recipeId,
                        Label = recipeLabel,
                        Url = url,
                        FoodItems = foodItems,
                        Ingredients = ingredientList
                        };
                    recipes.Add(existingRecipe);
                }
            }

            return recipes;
        }
    }
}
