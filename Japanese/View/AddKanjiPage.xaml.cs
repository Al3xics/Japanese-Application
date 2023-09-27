using Japanese.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Japanese.View
{
    /// <summary>
    /// Represents the user interface for adding a new Kanji character.
    /// </summary>
    public partial class AddKanjiPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the AddKanjiPage class.
        /// </summary>
        public AddKanjiPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the drop event on the Image element.
        /// Handles dropping an image file onto the Image element.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments containing information about the drop.</param>
        private void Image_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length == 1)
                {
                    if (DataContext is AddKanjiPageViewModel viewModel)
                    {
                        viewModel.HandleImageDrop(files[0]);
                    }
                }
                else
                {
                    MessageBox.Show("Please drop only one image at a time.", "Multiple Files Dropped", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}
