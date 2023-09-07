using Japanese.ViewModel;
using System.Windows.Controls;

namespace Japanese.View
{
    /// <summary>
    /// Represents the user interface for displaying Hiragana characters.
    /// </summary>
    public partial class HiraganaPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the HiraganaPage class.
        /// </summary>
        public HiraganaPage()
        {
            InitializeComponent();

            if (DataContext is HiraganaPageViewModel viewModel)
            {
                viewModel.InitializeNextStack();
            }
        }
    }
}
