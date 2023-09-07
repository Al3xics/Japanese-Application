using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Japanese.Utilities
{
    /// <summary>
    /// A base class for ViewModels that implements INotifyPropertyChanged for data binding.
    /// </summary>
    class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for a specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed (automatically populated).</param>
        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
