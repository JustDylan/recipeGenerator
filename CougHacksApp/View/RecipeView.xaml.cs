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
        public RecipeView(RecipeViewModel recipe)
        {
            InitializeComponent();
            this.recipe = recipe;
            this.DataContext = recipe;
        }



    }
}
