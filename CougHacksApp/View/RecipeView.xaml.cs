using CougHacksApp.Model;
using CougHacksApp.View;
using CougHacksApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CougHacksApp
{
    /// <summary>
    /// Interaction logic for RecipeView.xaml
    /// </summary>
    public partial class RecipeView : Window
    {
        private RecipeViewModel recipe;
        private Profile profile;
        public RecipeView(RecipeViewModel recipe, Profile profile)
        {
            InitializeComponent();
            this.recipe = recipe;
            this.DataContext = recipe;
            this.profile = profile;
        }

        public void HistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            RecipeListView recipeList = new RecipeListView(recipe,profile, false);
            recipeList.Show();
        }

        public void FavoriteBtn_Click(object sender, RoutedEventArgs e)
        {
            RecipeListView recipeList = new RecipeListView(recipe,profile);
            recipeList.Show();
        }

    }
}
