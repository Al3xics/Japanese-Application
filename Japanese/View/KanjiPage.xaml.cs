using Japanese.ViewModel;
using System.Windows.Controls;

namespace Japanese.View
{
    /// <summary>
    /// Represents the user interface for displaying Kanji characters.
    /// </summary>
    public partial class KanjiPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the KanjiPage class.
        /// </summary>
        public KanjiPage()
        {
            InitializeComponent();

            if (DataContext is KanjiPageViewModel viewModel)
            {
                viewModel.InitializeNextStack();
            }
        }
    }
}
