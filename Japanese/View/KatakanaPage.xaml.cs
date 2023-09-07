using Japanese.ViewModel;
using System.Windows.Controls;

namespace Japanese.View
{
    /// <summary>
    /// Represents the user interface for displaying Katakana characters.
    /// </summary>
    public partial class KatakanaPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the KatakanaPage class.
        /// </summary>
        public KatakanaPage()
        {
            InitializeComponent();

            if (DataContext is KatakanaPageViewModel viewModel)
            {
                viewModel.InitializeNextStack();
            }
        }
    }
}
