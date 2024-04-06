using CougHacksApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace CougHacksApp.Model
{
    public class Profile
    {
        public string Name { get; set; }
        public List<Recipe> HistoryRec {  get; set; }

        public List<Recipe> FavRec { get; set; }

        public List<string> Ingredient {  get; set; }

        /// <summary>
        /// Profile constructor.
        /// </summary>
        /// <param name="name"></param>
        public Profile(string name = "Guest") 
        {
            this.Name = name;
            this.HistoryRec = new List<Recipe>();
            this.FavRec = new List<Recipe>();
            this.Ingredient = new List<string>();
        }

        public void AddFavorite(Recipe recipe)
        {
            if(!this.FavRec.Contains(recipe))
            {
                this.FavRec.Add(recipe);
            }
        }

        public void RemoveFavorite(Recipe recipe) 
        {
            if(this.FavRec.Contains(recipe))
            {
                this.FavRec.Remove(recipe);
            }
        }

        public void AddHistory(Recipe recipe)
        {
            if(!this.HistoryRec.Contains(recipe))
            {
                this.HistoryRec.Add(recipe);
            }
        }

        public void RemoveHistory(Recipe recipe)
        {
            if(this.HistoryRec.Contains(recipe))
            {
                this.HistoryRec.Remove(recipe);
            }
        }

        public void Save()
        {
            //Saves favorites to a machine readable file
            File.WriteAllText("favList.txt", string.Empty); // clear file
            using (StreamWriter favorites = new StreamWriter("favList.txt"))
                for (int i = 0; i < this.FavRec.Count; i++)
            {
                    favorites.WriteLine(this.FavRec[i].ID);
            }
        }

        public void Read()
        {
            //Read from file
            string importedID;
            using (StreamReader favorites = new StreamReader("favList.txt"))
                while((importedID = favorites.ReadLine()) != null)
                {
                    RecipeQueryManager query = new RecipeQueryManager();
                    AddFavorite(query.GetRecipeByIdAsync(int.Parse(importedID)).Result);
                }
            

        }
    }
}
