using CougHacksApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
