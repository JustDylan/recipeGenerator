using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CougHacksApp.Model
{
    public class FoodItemQueryManager
    {
        string dbhost = "localhost";
        string dbusername = "postgres";
        static string dbpassword = "1";
        string db = "recipedatabase";

        public async Task<List<string>> QueryForFoodItemsAsync(string input)
        {
            List<string> foodItems = new List<string>();
            var connectionString = "Host=localhost;Username=postgres;Password=1;Database=recipedatabase";
            await using var dataSource = NpgsqlDataSource.Create(connectionString);

            await using var command = dataSource.CreateCommand("SELECT fooditem FROM ingredients WHERE fooditem ILIKE '% " + input + "%' OR fooditem ILIKE '" + input + "%';");
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var foodItem = reader.GetString(0);
                foodItems.Add(foodItem);
            }

            return foodItems;
        }
    }
}
