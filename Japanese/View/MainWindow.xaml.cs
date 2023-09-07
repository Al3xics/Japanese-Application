using Japanese.Utilities;
using Japanese.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace Japanese
{
    /// <summary>
    /// Represents the main application window.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            FrameManager.InitializeMainFrame(Pages);
            DataContext = new MainWindowViewModel(this);
        }

        /// <summary>
        /// Handles the mouse left-button down event to allow dragging of the window.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
