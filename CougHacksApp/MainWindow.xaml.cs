using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CougHacksApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
                CreateTag(selectedText);
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

        private void CreateTag(string tagText)
        {
            // StackPanel to hold the label and button
            StackPanel tagPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(5)
            };

            // Label for the tag text
            Label tagLabel = new Label
            {
                Content = tagText,
                Margin = new Thickness(2)
            };

            // Button to remove the tag, with "X" as content
            Button deleteButton = new Button
            {
                Content = "X",
                Margin = new Thickness(2),
                Padding = new Thickness(2),
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 10 // Adjust as needed
            };
            deleteButton.Click += (sender, e) =>
            {
                TagsPanel.Children.Remove(tagPanel); // Remove the entire StackPanel
            };

            // Add the label and button to the StackPanel
            tagPanel.Children.Add(tagLabel);
            tagPanel.Children.Add(deleteButton);

            // Add the StackPanel to the TagsPanel
            TagsPanel.Children.Add(tagPanel); ;
        }

        private void TagButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                TagsPanel.Children.Remove(button); // Remove the tag from the display
            }
        }
    }
}