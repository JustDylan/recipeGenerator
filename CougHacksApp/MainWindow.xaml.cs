using CougHacksApp.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Msagl.WpfGraphControl;
using Microsoft.Msagl.Drawing;
using CougHacksApp.Model;

namespace CougHacksApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GraphViewer graphViewer = new GraphViewer();

        private Profile profile;
        
        private IngredientViewModel ingredientVM;

        public MainWindow()
        {
            InitializeComponent();
            graphViewer.BindToPanel(this.RGraphView);
            //this.CreateGraph(null, null);
            this.Loaded += (a, b) => CreateGraph(null, null);
            //CreateGraph(null, null);
            //this.Show();
            //AutomaticGraphLayoutControl test = new AutomaticGraphLayoutControl();
            //this.CreateGraph(null, null);

            this.profile = new Profile();
            RecipeViewModel recipeVM = new RecipeViewModel();
            RecipeView recipeView = new RecipeView(recipeVM,this.profile);
            recipeView.ShowDialog();
        }

        public void Graph_Changed(object sender, EventArgs e)
        {
            if (sender is Recipe rec)
            {
                //RecipeViewModel recipeVM = new RecipeViewModel(rec.Label,rec.Ingredients,rec.Url);
                //RecipeView recipeView = new RecipeView(recipeVM);
                //RecipeViewModel recipeVM = new RecipeViewModel();
                //RecipeView recipeView = new RecipeView(recipeVM);
                //recipeView.Show();
            }

            
        }

        public void CreateGraph(object sender, ExecutedRoutedEventArgs ex)
        {
            
            Graph graph = new Graph();
            graph.AddEdge("A", "B");
            graph.AddEdge("B", "A");
            var e = graph.AddNode("test");
            e.UserData = new List<string>(new [] { "test", "test2"});
            graph.AddEdge("test", "A");
            
            e.Attr.VisualsChanged += (o, e) => { if (((NodeAttr)o).LineWidth == 2) { MessageBox.Show(o.ToString() + "   " + ((NodeAttr)o).Styles.GetType().ToString()); } };
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

            graphViewer.Graph = graph;
            this.ingredientVM = new IngredientViewModel();
            this.DataContext =this.ingredientVM;
        }

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            string query = (sender as TextBox).Text;

            if (string.IsNullOrWhiteSpace(query))
            {
                SuggestionsPopup.IsOpen = false;
                return;
            }

            var filteredSuggestions = this.ingredientVM.AvailableIngredients.Where(s => s.StartsWith(query, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (filteredSuggestions.Any())
            {
                SuggestionsListBox.ItemsSource = filteredSuggestions;
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
                // Optionally, move focus back to the text box
                //SearchTextBox.Focus();
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
    }
}