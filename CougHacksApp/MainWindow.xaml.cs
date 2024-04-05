using CougHacksApp.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CougHacksApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IngredientViewModel ingredientVM;

        public MainWindow()
        {
            InitializeComponent();
            this.ingredientVM = new IngredientViewModel();
            this.DataContext =this.ingredientVM;
        }

        private List<string> _suggestions = new List<string>
        {
            "Apple","Apricot", "Banana", "Orange", "Mango", "Pear", "Peach", "Grape", "Plum"
        };

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            string query = (sender as TextBox).Text;

            if (string.IsNullOrWhiteSpace(query))
            {
                SuggestionsPopup.IsOpen = false;
                return;
            }

            var filteredSuggestions = _suggestions.Where(s => s.StartsWith(query, StringComparison.InvariantCultureIgnoreCase)).ToList();

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
    }
}