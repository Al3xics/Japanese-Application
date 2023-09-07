using System.Windows.Controls;

namespace Japanese.Utilities
{
    /// <summary>
    /// Manages the application's main frame, allowing access to it from different parts of the application.
    /// </summary>
    class FrameManager
    {
        /// <summary>
        /// Gets or sets the main frame of the application.
        /// </summary>
        public static Frame MainFrame { get; private set; } = null!;

        /// <summary>
        /// Initializes the main frame of the application.
        /// </summary>
        /// <param name="mainFrame">The main frame control to initialize.</param>
        public static void InitializeMainFrame(Frame mainFrame)
        {
            MainFrame = mainFrame;
        }
    }
}
