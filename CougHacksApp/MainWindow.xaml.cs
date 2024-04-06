using CougHacksApp.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Msagl.WpfGraphControl;
using Microsoft.Msagl.Drawing;
using CougHacksApp.Model;
using CougHacksApp.RecipeGraph;
using System.Collections.Specialized;
using CefSharp.DevTools.Emulation;

namespace CougHacksApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GraphViewer graphViewer = new GraphViewer();

        private FoodItemQueryManager foodItemQueryManager;

        private Profile profile;
        
        private IngredientViewModel ingredientVM;

        public MainWindow()
        {
            InitializeComponent();
            graphViewer.BindToPanel(this.RGraphView);
            foodItemQueryManager = new FoodItemQueryManager();
            //this.CreateGraph(null, null);
            this.Loaded += (a, b) => CreateGraph(null, null);
            //CreateGraph(null, null);
            //this.Show();
            //AutomaticGraphLayoutControl test = new AutomaticGraphLayoutControl();
            //this.CreateGraph(null, null);
            this.ingredientVM = new IngredientViewModel();
            this.DataContext = this.ingredientVM;
            this.profile = new Profile();
            //RecipeViewModel recipeVM = new RecipeViewModel();
            //RecipeView recipeView = new RecipeView(recipeVM,this.profile);
            //recipeView.ShowDialog();

            this.ingredientVM.SelectedIngredients.CollectionChanged += IngredientVM_SelectedIngredientsChanged;
        }

        private void IngredientVM_SelectedIngredientsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null)
            {
                return;
            }
        }

        public void Graph_Changed(object sender, EventArgs e)
        {
            if (sender is Recipe rec)
            {
                RecipeViewModel recipeVM = new RecipeViewModel(rec);
                RecipeView recipeView = new RecipeView(recipeVM, this.profile);
                recipeView.ShowDialog();
                this.profile.AddHistory(rec);
            }

            
        }

        public void CreateGraph(object sender, ExecutedRoutedEventArgs ex)
        {
            //this.graphViewer.Graph = new Graph();
            //CreateGraphFromIngredients(new List<string>(new [] { "apple", "banana", "berry", "coconut" }));
            /*
            Graph graph = new Graph();
            graph.AddEdge("A", "B");
            graph.AddEdge("B", "A");
            var e = graph.AddNode("test");
            e.UserData = new List<string>(new [] { "test", "test2"});
            graph.AddEdge("test", "A");
            
            e.Attr.VisualsChanged += (o, e) => { if (((NodeAttr)o).LineWidth == 2) { MessageBox.Show(o.ToString() + "   " + ((NodeAttr)o).Styles.GetType().ToString()); } };
            */
            //graphViewer.MouseDown += (o, e) => { MessageBox.Show(o.ToString()); };
            //graphViewer.ObjectUnderMouseCursor.
            //graphViewer.ObjectUnderMouseCursorChanged += (o, e) => { MessageBox.Show(o.ToString()); };
            /*
            var e = graph.AddEdge("4", "5");
            e.LabelText = "Some edge label";
            e.Attr.Color = Color.Red;
            e.Attr.LineWidth *= 2;
            graph.AddEdge("47", "58");
            graph.AddEdge("70", "71");
            //var tn = graph.AddNode("test");

            graph.AddEdge("test", "47");

            

            graph.Attr.LayerDirection = LayerDirection.LR;*/
            //graph.LayoutAlgorithmSettings.EdgeRoutingSettings.EdgeRoutingMode = EdgeRoutingMode.Rectilinear;

            //var global = (SugiyamaLayoutSettings)graph.LayoutAlgorithmSettings;
            //var local = (SugiyamaLayoutSettings)global.Clone();
            //local.Transformation = PlaneTransformation.Rotation(-Math.PI / 2);
            //global.ClusterSettings.Add(subgraph2, local);

        }

        private void CreateGraphFromIngredients(List<string> ingredients, int numMatching)
        {
            RecipeQueryManager recipeGetter = new RecipeQueryManager();
            List<Recipe> recipes = recipeGetter.GetRecipesFromIngredientsAsync(ingredients, numMatching).Result;

            GraphViewModel graph = GraphFactory.CreateGraph(recipes);

            graph.GraphNodeChanged += Graph_Changed;

            graph.AssignGraph(this.graphViewer);
        }

        private async void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            string query = (sender as TextBox).Text;

            if (string.IsNullOrWhiteSpace(query))
            {
                SuggestionsPopup.IsOpen = false;
                return;
            }

            //var filteredSuggestions = this.ingredientVM.AvailableIngredients.Where(s => s.StartsWith(query, StringComparison.InvariantCultureIgnoreCase)).ToList();
            List<string> filteredSuggestions = await foodItemQueryManager.QueryForFoodItemsAsync(query);

            if (filteredSuggestions.Any())
            {
                // Limit the number of suggestions to 5
                var limitedSuggestions = filteredSuggestions.Take(5).ToList();
                SuggestionsListBox.ItemsSource = limitedSuggestions;
                SuggestionsPopup.IsOpen = true;
            }
            else
            {
                SuggestionsPopup.IsOpen = false;
            }
        }

        private void SuggestionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SuggestionsListBox.SelectedItem != null)
            {
                // Get text box selection.
                string selectedText = SuggestionsListBox.SelectedItem.ToString();
                SearchTextBox.Text = "Search";
                this.ingredientVM.AddIngredients(selectedText);
                SuggestionsPopup.IsOpen = false;
                SuggestionsListBox.SelectedItem = null;
                // Optionally, move focus back to the text box
                //SearchTextBox.Focus();
                //TODO create graph
                
            }
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox searchBox = sender as TextBox;
            if (searchBox != null && searchBox.Text == "Search")
            {
                searchBox.Text = "";
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox searchBox = sender as TextBox;
            if (searchBox != null && string.IsNullOrWhiteSpace(searchBox.Text))
            {
                searchBox.Text = "Search";
            }
        }

        private void TagButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.DataContext is string tag)
            {
                var ingredientVM = DataContext as IngredientViewModel;
                ingredientVM.SelectedIngredients.Remove(tag);
            }
        }

        private void CommonIngredientBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.DataContext is string tag)
            {
                var ingredientVM = DataContext as IngredientViewModel;
                ingredientVM.AddIngredients(tag);
            }
        }

        private void Gen_Click(object sender, RoutedEventArgs e)
        {
            List<string> foodItems =
                    this.ingredientVM.SelectedIngredients.ToList();
            int numMatching = foodItems.Count > 2 ? 2 : 0;

            CreateGraphFromIngredients(foodItems, numMatching);
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) 
            {
                this.ingredientVM.AddIngredients(SearchTextBox.Text);
                this.SearchTextBox.Clear();
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            this.ingredientVM.SelectedIngredients.Clear();
        }
    }
}