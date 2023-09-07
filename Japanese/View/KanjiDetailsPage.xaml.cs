using Japanese.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Japanese.View
{
    /// <summary>
    /// Represents the user interface for displaying details of a Kanji character.
    /// </summary>
    public partial class KanjiDetailsPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the KanjiDetailsPage class.
        /// </summary>
        public KanjiDetailsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the mouse double-click event on the KanjiListView to view the selected Kanji character's image.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void KanjiListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (KanjiListView.SelectedItem is KanjiData selectedKanji)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Kanji", selectedKanji.ImageName);
                if (File.Exists(imagePath))
                {
                    ProcessStartInfo startInfo = new()
                    {
                        FileName = imagePath,
                        UseShellExecute = true
                    };
                    Process.Start(startInfo);
                }
                else
                {
                    MessageBox.Show("The image file was not found.");
                }
            }
        }
    }
}
