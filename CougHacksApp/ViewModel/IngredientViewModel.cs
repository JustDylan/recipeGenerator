using CefSharp.DevTools.Database;
using CougHacksApp.MVVM;
using System.Collections.ObjectModel;

namespace CougHacksApp.ViewModel
{
    internal class IngredientViewModel : ViewModelBase
    {
        public ObservableCollection<string> SelectedIngredients { get; set; }

        public ObservableCollection<string> CommonIngredients { get; set; }

        public ObservableCollection<string> AvailableIngredients { get; set; }

        public IngredientViewModel()
        {
            this.SelectedIngredients = new ObservableCollection<string>();
            this.AvailableIngredients = new ObservableCollection<string>();
            this.CommonIngredients = new ObservableCollection<string>();
        }

        public void AddIngredients(string ingredient)
        {
            if(!this.SelectedIngredients.Contains(ingredient)) 
            {
                this.SelectedIngredients.Add(ingredient);
            }
        }
    }
}
