using Japanese.Model;
using Japanese.Utilities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Japanese.ViewModel
{
    /// <summary>
    /// ViewModel class for managing the Katakana page.
    /// </summary>
    class KatakanaPageViewModel : ViewModelBase
    {
        /* -------------------- Variable -------------------- */
        private Stack<string> Next_Stack = new();
        private Stack<string> Previous_Stack = new();
        private int cpt;
        private int TotalKatakana;
        private Dictionary<string, KatakanaData> imageToKatakanaData = new();
        private string imageName = string.Empty;


        /* -------------------- Command -------------------- */
        /// <summary>
        /// Command to navigate to the Home Page.
        /// </summary>
        public RelayCommand HomePageCommand => new(execute => HomePageAction());

        /// <summary>
        /// Command to navigate to the previous Katakana item.
        /// </summary>
        public RelayCommand PreviousCommand => new(execute => PreviousAction());

        /// <summary>
        /// Command to navigate to the next Katakana item.
        /// </summary>
        public RelayCommand NextCommand => new(execute => NextAction());

        /// <summary>
        /// Command to update the display of the current Katakana item count.
        /// </summary>
        public RelayCommand UpdateNumberOfKatakanaCommand => new(execute => UpdateNumberOfKatakana());


        /* -------------------- Properties -------------------- */
        /// <summary>
        /// Indicates whether the Katakana checkbox is checked.
        /// </summary>
        private bool _isKatakanaChecked;

        public bool IsKatakanaChecked
        {
            get { return _isKatakanaChecked; }
            set
            {
                _isKatakanaChecked = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Indicates whether the Pronunciation checkbox is checked.
        /// </summary>
        private bool _isPronunciationChecked = true;

        public bool IsPronunciationChecked
        {
            get { return _isPronunciationChecked; }
            set
            {
                _isPronunciationChecked = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Indicates whether the Number of Strokes checkbox is checked.
        /// </summary>
        private bool _isNbOfStrokesChecked;

        public bool IsNbOfStrokesChecked
        {
            get { return _isNbOfStrokesChecked; }
            set
            {
                _isNbOfStrokesChecked = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Indicates whether all the checkboxes are checked.
        /// </summary>
        private bool _toggleAllChecked;

        public bool ToggleAllChecked
        {
            get { return _toggleAllChecked; }
            set
            {
                _toggleAllChecked = value;
                IsKatakanaChecked = !IsKatakanaChecked;
                IsPronunciationChecked = !IsPronunciationChecked;
                IsNbOfStrokesChecked = !IsNbOfStrokesChecked;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// An 'ImageSource' property representing the currently displayed Katakana character's image.
        /// </summary>
        private ImageSource _currentImage = new BitmapImage(new Uri("/Images/Graphismes/No_Image.png", UriKind.Relative));

        public ImageSource CurrentImage
        {
            get { return _currentImage; }
            set
            {
                _currentImage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// A property to display the pronunciation of the Katakana character.
        /// </summary>
        private string _pronunciationText = string.Empty;

        public string PronunciationText
        {
            get { return _pronunciationText; }
            set
            {
                _pronunciationText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// A property to display the number of strokes of the Katakana character.
        /// </summary>
        private string _nbOfStrokesText = string.Empty;

        public string NbOfStrokesText
        {
            get { return _nbOfStrokesText; }
            set
            {
                _nbOfStrokesText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///  A property to display the number of Katakana already done.
        /// </summary>
        private string _katakanaText = string.Empty;

        public string KatakanaText
        {
            get { return _katakanaText; }
            set
            {
                _katakanaText = value;
                OnPropertyChanged();
            }
        }


        /* -------------------- Method -------------------- */
        /// <summary>
        /// Navigates to the Home Page.
        /// </summary>
        private void HomePageAction()
        {
            FrameManager.MainFrame.Navigate(new Uri("/View/HomePage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Navigates to the previous Katakana item.
        /// </summary>
        private void PreviousAction()
        {
            if (Previous_Stack.Count > 1)
            {
                cpt--;
                string previousImage = Previous_Stack.Pop();
                Next_Stack.Push(imageName);
                LoadImage(previousImage);
                imageName = previousImage;

                if (imageToKatakanaData.TryGetValue(previousImage, out var data))
                {
                    PronunciationText = data.Pronunciation;
                    NbOfStrokesText = data.NumberOfStrokes.ToString();
                }

                UpdateNumberOfKatakanaCommand.Execute(null);
            }
            else
            {
                MessageBox.Show("You can't go back anymore. If you want to continue, click the button 'Next'.");
            }
        }

        /// <summary>
        /// Navigates to the next Katakana item.
        /// </summary>
        private void NextAction()
        {
            if (Next_Stack.Count > 0)
            {
                cpt++;
                string nextImage = Next_Stack.Pop();
                Previous_Stack.Push(imageName);
                LoadImage(nextImage);
                imageName = nextImage;

                if (imageToKatakanaData.TryGetValue(nextImage, out var data))
                {
                    PronunciationText = data.Pronunciation;
                    NbOfStrokesText = data.NumberOfStrokes.ToString();
                }

                UpdateNumberOfKatakanaCommand.Execute(null);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("You have reached the end. Do you want to restart ?", "Restart ?", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    Next_Stack.Clear();
                    Previous_Stack.Clear();

                    InitializeNextStack();
                }
            }
        }

        /// <summary>
        /// Initializes the stack of next Katakana items.
        /// </summary>
        public void InitializeNextStack()
        {
            List<string> randomImageList = Images.GetRandomImageList(@"./Images/Katakana/");
            List<KatakanaData>? katakanaDataList = JsonData.LoadDataFromJson<KatakanaData>(@"./Images/Katakana.json");
            if (katakanaDataList == null)
            {
                MessageBox.Show("[KatakanaPage] : Error loading JSON data.");
                return;
            }

            foreach (string image in randomImageList)
            {
                KatakanaData? katakanaData = JsonData.SearchData(katakanaDataList, image);
                if (katakanaData != null)
                {
                    imageToKatakanaData[image] = katakanaData;
                    Next_Stack.Push(image);
                }
                else
                {
                    MessageBox.Show($"[KatakanaPage] : Data for image '{image}' not found.");
                    return;
                }
            }
            string nextImage = Next_Stack.Pop();
            Previous_Stack.Push(nextImage);
            LoadImage(nextImage);
            imageName = nextImage;

            if (imageToKatakanaData.TryGetValue(nextImage, out var data))
            {
                PronunciationText = data.Pronunciation;
                NbOfStrokesText = data.NumberOfStrokes.ToString();
            }

            cpt = 1;
            TotalKatakana = Next_Stack.Count;
            UpdateNumberOfKatakanaCommand.Execute(null);
        }

        /// <summary>
        /// Updates the display of the current Katakana item count.
        /// </summary>
        public void UpdateNumberOfKatakana()
        {
            KatakanaText = $"{cpt}/{TotalKatakana + 1}";
        }

        /// <summary>
        /// Loads an image based on the provided image relative path.
        /// </summary>
        /// <param name="imageRelativePath">The relative path of the image to load.</param>
        private void LoadImage(string imageRelativePath)
        {
            CurrentImage = new BitmapImage(new Uri($"/Images/Katakana/{imageRelativePath}", UriKind.Relative));
        }
    }
}
