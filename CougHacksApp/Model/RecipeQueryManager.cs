namespace CougHacksApp.Model
{
    using System.Security.AccessControl;
    using System.Security.Policy;
    using System.Text;
    using System.Threading.Tasks;
    using Npgsql;

    public class RecipeQueryManager
    {
        //TODO store database connect as instance member when constructed.
        static readonly string connectionString =
            "Host=localhost;Username=postgres;Password=1;Database=recipedatabase";

        public async Task<List<Recipe>> GetRecipesFromIngredientsAsync(List<string> ingredients, int minMatchIngredients = 2)
        {
            var recipes = new List<Recipe>();

            // Construct the SQL query dynamically based on the ingredients list
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT r.id ");
            queryBuilder.Append("FROM recipes AS r ");
            queryBuilder.Append("INNER JOIN ingredients AS i ON r.id = i.recipe_id ");
            queryBuilder.Append("WHERE LOWER(i.fooditem) LIKE ANY ( ARRAY[");

            string selectedFoodItems = string.Empty;

            // Append each ingredient to the query
            for (int i = 0; i < ingredients.Count; i++)
            {
                selectedFoodItems += "'% " + ingredients[i].ToLower() + "%'," + " '" + ingredients[i].ToLower() + "%'" +
                    ((i < ingredients.Count - 1) ? ", " : string.Empty);
            }

            queryBuilder.Append(selectedFoodItems);
            queryBuilder.Append("]) GROUP BY r.id HAVING COUNT(r.id) >= " + minMatchIngredients + " LIMIT 40");

            // Execute the query
            await using var dataSource = NpgsqlDataSource.Create(connectionString);
            await using var command = dataSource.CreateCommand(queryBuilder.ToString());
            await using var reader = command.ExecuteReaderAsync().Result;

            // Read the results and construct Recipe instances
            while (reader.ReadAsync().Result)
            {
                int recipeId = reader.GetInt32(0);

                Recipe recipe = GetRecipeByIdAsync(recipeId, dataSource).Result;
                recipes.Add(recipe);
            }

            reader.Close();

            return recipes;
        }

        public async Task<Recipe> GetRecipeByIdAsync(int recipeId)
        {
            await using var dataSource = NpgsqlDataSource.Create(connectionString);
            return this.GetRecipeByIdAsync(recipeId, dataSource).Result;
        }

        private async Task<Recipe> GetRecipeByIdAsync(int recipeId, NpgsqlDataSource dataSource)
        {
            var recipe = new Recipe();
            recipe.FoodItems = new HashSet<string>();
            recipe.Ingredients = new List<string>();

            await using var command = dataSource.CreateCommand("SELECT r.title, i.fooditem, r.link FROM recipes AS r INNER JOIN ingredients AS i ON r.id = i.recipe_id WHERE r.id = @RecipeId;");
            command.Parameters.AddWithValue("RecipeId", recipeId);

            await using var reader = command.ExecuteReaderAsync().Result;

            while (reader.ReadAsync().Result)
            {
                recipe.ID = recipeId;
                recipe.Label = reader.GetString(0);
                recipe.FoodItems.Add(reader.GetString(1));
                recipe.Url = reader.GetString(2);
            }

            reader.Close();

            // query food items that belong the the recipe
            string foodItemQuery =
                "SELECT i.fooditem, i.ingredient_name " +
                "FROM ingredients as i " +
                "WHERE i.recipe_id = " + recipeId;

            command.CommandText = foodItemQuery;
            await using var foodItemReader = command.ExecuteReaderAsync().Result;

            HashSet<string> foodItems = new HashSet<string>();
            List<string> ingredientList = new List<string>();

            while (foodItemReader.ReadAsync().Result)
            {
                foodItems.Add(foodItemReader.GetString(0));
                ingredientList.Add(foodItemReader.GetString(1));
            }
            foodItemReader.Close();

            recipe.FoodItems = foodItems;
            recipe.Ingredients = ingredientList;

            return recipe;
        }
    }
}
