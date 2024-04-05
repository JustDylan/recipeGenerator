using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CougHacksApp.MVVM
{
    /// <summary>
    /// Base view model that fires Property Changed events as needed
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
