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
            this.LoadDemo();
        }

        public void AddIngredients(string ingredient)
        {
            if(!this.SelectedIngredients.Contains(ingredient)) 
            {
                this.SelectedIngredients.Add(ingredient);
            }
        }

        private void LoadDemo()
        {
            List<string> Ingredients = new List<string>() { "garlic", "onion","gluten-free soy sauce",
                                                            "chilly powder", "soy sauce", "honey",
                                                            "sunflower oil",
                                                            "black peppercorns","sugar", "Apple",
                                                            "Apricot", "Banana", "Orange", 
                                                            "Mango", "Pear", "Peach", "Grape", "Plum"};
            foreach(string item in Ingredients) 
            {
                this.CommonIngredients.Add(item);

                this.AvailableIngredients.Add(item);
            }
        }
    }
}
