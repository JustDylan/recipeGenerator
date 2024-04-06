using Npgsql;

namespace CougHacksApp.Model
{
    public class FoodItemQueryManager
    {
        string dbhost = "localhost";
        string dbusername = "postgres";
        static string dbpassword = "1004";
        string db = "recipedatabase";

        public async Task<List<string>> QueryForFoodItemsAsync(string input)
        {
            List<string> foodItems = new List<string>();
            var connectionString = "Host=localhost; Username=postgres; Password=1004; Database=recipedatabase";
            await using var dataSource = NpgsqlDataSource.Create(connectionString);

            // SQL query that selects from common_foods only
            string sql = @"
            SELECT foodname FROM common_foods 
            WHERE foodname ILIKE @input || '%'
            ORDER BY foodname
            LIMIT 10;";

            await using var command = dataSource.CreateCommand(sql);
            command.Parameters.AddWithValue("@input", input);

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var foodItem = reader.GetString(0); // Get the food item name
                foodItems.Add(foodItem);
            }

            return foodItems;
        }
    }
}
