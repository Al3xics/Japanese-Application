using Japanese.Utilities;
using System;

namespace Japanese.ViewModel
{
    /// <summary>
    /// This class represents the ViewModel for the "HomePage" in the application.
    /// It is responsible for navigation commands.
    /// </summary>
    class HomePageViewModel
    {
        /* -------------------- Command -------------------- */
        /// <summary>
        /// Gets the command to navigate to the Kanji page.
        /// </summary>
        public RelayCommand KanjiPageCommand => new(execute => KanjiPageAction());

        /// <summary>
        /// Gets the command to navigate to the Hiragana page.
        /// </summary>
        public RelayCommand HiraganaPageCommand => new(execute => HiraganaPageAction());

        /// <summary>
        /// Gets the command to navigate to the Katakana page.
        /// </summary>
        public RelayCommand KatakanaPageCommand => new(execute => KatakanaPageAction());


        /* -------------------- Method -------------------- */
        /// <summary>
        /// Navigates to the Kanji page when the KanjiPageCommand is executed.
        /// </summary>
        private void KanjiPageAction()
        {
            FrameManager.MainFrame.Navigate(new Uri("/View/KanjiPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Navigates to the Hiragana page when the HiraganaPageCommand is executed.
        /// </summary>
        private void HiraganaPageAction()
        {
            FrameManager.MainFrame.Navigate(new Uri("/View/HiraganaPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Navigates to the Katakana page when the KatakanaPageCommand is executed.
        /// </summary>
        private void KatakanaPageAction()
        {
            FrameManager.MainFrame.Navigate(new Uri("/View/KatakanaPage.xaml", UriKind.Relative));
        }
    }
}
