using CougHacksApp.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CougHacksApp.ViewModel
{
    
    public class RecipeViewModel : ViewModelBase
    {
        private string name;

        public string Name
        {
            get { return name; }
            set 
            { 
                name = value;
                this.OnPropertyChanged();
            }
        }

        private string recipeList;

        public string RecipeList
        {
            get { return recipeList; }
            set 
            { 
                recipeList = value;
                this.OnPropertyChanged();
            }
        }


        private List<string> ingredients;

        public List<string> Ingredients
        {
            get 
            { 
                return ingredients;
            }
            set 
            { 
                ingredients = value;
                OnPropertyChanged();
            }
        }

        private string linkURL;
        public string LinkURL
        {
            get
            {
                return linkURL;
            }
            set 
            {
                linkURL = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Recipe broadcaster.
        /// </summary>
        /// <param name="ingredients">List of ingredients (with quantity).</param>
        /// <param name="linkURL">URL link</param>
        public RecipeViewModel(string name, List<string> ingredients, string linkURL)
        {
            this.Name = name;
            this.linkURL = linkURL;
            this.ingredients = ingredients;

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"{this.Name} recipe:");
            stringBuilder.AppendLine();
            foreach(string ingredient in ingredients)
            {
                stringBuilder.Append($"- {ingredient}");
            }
        }
    }
}
