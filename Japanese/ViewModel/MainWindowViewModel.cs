using Japanese.Utilities;
using System.Windows;

namespace Japanese.ViewModel
{
    /// <summary>
    /// ViewModel class for managing the main window.
    /// </summary>
    class MainWindowViewModel
    {
        /* -------------------- Variable -------------------- */
        private Window window;


        /* -------------------- Command -------------------- */
        /// <summary>
        /// Command to minimize the main window.
        /// </summary>
        public RelayCommand MinimizeCommand => new(execute => MinimizeAction());

        /// <summary>
        /// Command to close the application.
        /// </summary>
        public RelayCommand CloseCommand => new(execute => CloseAction());


        /* -------------------- Constructor -------------------- */
        /// <summary>
        /// Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        /// <param name="window">The main window to be managed.</param>
        public MainWindowViewModel(Window window)
        {
            this.window = window;
        }


        /* -------------------- Method -------------------- */
        /// <summary>
        /// Minimizes the main window.
        /// </summary>
        private void MinimizeAction()
        {
            window.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Closes the application.
        /// </summary>
        private void CloseAction()
        {
            Application.Current.Shutdown();
        }
    }
}
