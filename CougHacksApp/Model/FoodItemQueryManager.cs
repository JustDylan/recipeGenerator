using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

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

            // Updated SQL query to only include items that start with the input
            // Corrected ILIKE pattern to use parameter placeholder correctly
            string sql = @"
            SELECT DISTINCT foodname 
            FROM common_foods
            WHERE foodname ILIKE @input || '%';"; // Items that start with the input

            await using var command = dataSource.CreateCommand(sql);
            // No need to call ToLower() if the database comparison is case-insensitive (ILIKE)
            command.Parameters.AddWithValue("@input", input);

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var foodItem = reader.GetString(0); // Assuming you want to maintain the original case from the database

                // Since ILIKE is used for case-insensitive search and DISTINCT is used in SQL,
                // the check for existing items in the list can be skipped
                foodItems.Add(foodItem);
            }

            return foodItems;
        }
    }
}
