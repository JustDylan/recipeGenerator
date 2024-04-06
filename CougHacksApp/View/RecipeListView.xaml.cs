using CougHacksApp.ViewModel;
using CougHacksApp.Model;
using System.Windows;
using System.Collections.ObjectModel;

namespace CougHacksApp.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RecipeListView : Window
    {
        public Profile profile;
        public ObservableCollection<RecipeViewModel> Recipes { get; set; }
        private RecipeViewModel prevRec;

        private RecipeViewModel selectedItem;

        public RecipeViewModel SelectedItem
        {
            get { return selectedItem; }
            set 
            { 
                selectedItem = value;
            }
        }


        public RecipeListView(RecipeViewModel rec,Profile profile, bool isFav = true)
        {
            InitializeComponent();

            this.prevRec = rec;
            this.profile = profile;

            if(isFav) 
            {
                this.Title = "Favorite Recipes";
                AddFavBtn.Visibility = Visibility.Visible; 
            }
            else
            {
                this.Title = "History";
            }

            this.Recipes = new ObservableCollection<RecipeViewModel>();

            if(isFav)
            {
                foreach (Recipe recipe in this.profile.FavRec)
                {
                    RecipeViewModel newRecipe = new RecipeViewModel(recipe);
                    this.Recipes.Add(newRecipe);
                }
            }else
            {
                foreach (Recipe recipe in this.profile.HistoryRec)
                {
                    RecipeViewModel newRecipe = new RecipeViewModel(recipe);
                    this.Recipes.Add(newRecipe);
                }
            }

            this.DataContext = this;
            

        }

        private void AddFavBtn_Click(object sender, RoutedEventArgs e)
        {
            //this.profile.AddFavorite(prevRec);
            this.Recipes.Add(prevRec);
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedItem != null) 
            {
                RecipeView recipeView = new RecipeView(SelectedItem, profile);
                recipeView.Show();
                this.Close();
            }
        }
    }
}
