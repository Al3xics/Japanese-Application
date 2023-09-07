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
    /// This class represents the ViewModel for the "HiraganaPage" in the application.
    /// It manages the display of hiragana characters and their associated data.
    /// </summary>
    class HiraganaPageViewModel : ViewModelBase
    {
        /* -------------------- Variable -------------------- */
        private Stack<string> Next_Stack = new();
        private Stack<string> Previous_Stack = new();
        private int cpt;
        private int TotalHiragana;
        private Dictionary<string, HiraganaData> imageToHiraganaData = new();
        private string imageName = string.Empty;


        /* -------------------- Command -------------------- */
        /// <summary>
        /// Navigates to the "HomePage".
        /// </summary>
        public RelayCommand HomePageCommand => new(execute => HomePageAction());

        /// <summary>
        /// Displays the previous hiragana character.
        /// </summary>
        public RelayCommand PreviousCommand => new(execute => PreviousAction());

        /// <summary>
        /// Displays the next hiragana character.
        /// </summary>
        public RelayCommand NextCommand => new(execute => NextAction());

        /// <summary>
        /// Updates the display of the current hiragana character's position.
        /// </summary>
        public RelayCommand UpdateNumberOfHiraganaCommand => new(execute => UpdateNumberOfHiragana());


        /* -------------------- Properties -------------------- */
        /// <summary>
        /// A boolean property to check/uncheck hiragana display.
        /// </summary>
        private bool _isHiraganaChecked;

        public bool IsHiraganaChecked
        {
            get { return _isHiraganaChecked; }
            set
            {
                _isHiraganaChecked = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// A boolean property to check/uncheck pronunciation display.
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
        /// A boolean property to check/uncheck the number of strokes display.
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
        /// A boolean property to toggle the display of all properties.
        /// </summary>
        private bool _toggleAllChecked;

        public bool ToggleAllChecked
        {
            get { return _toggleAllChecked; }
            set
            {
                _toggleAllChecked = value;
                IsHiraganaChecked = !IsHiraganaChecked;
                IsPronunciationChecked = !IsPronunciationChecked;
                IsNbOfStrokesChecked = !IsNbOfStrokesChecked;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// An 'ImageSource' property representing the currently displayed hiragana character's image.
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
        /// A property to display the pronunciation of the hiragana character.
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
        /// A property to display the number of strokes of the hiragana character.
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
        /// A property to display the current hiragana character's position.
        /// </summary>
        private string _hiraganaText = string.Empty;

        public string HiraganaText
        {
            get { return _hiraganaText; }
            set
            {
                _hiraganaText = value;
                OnPropertyChanged();
            }
        }


        /* -------------------- Method -------------------- */
        /// <summary>
        /// Navigates the user to the "HomePage".
        /// </summary>
        private void HomePageAction()
        {
            FrameManager.MainFrame.Navigate(new Uri("/View/HomePage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Displays the previous hiragana character.
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

                if (imageToHiraganaData.TryGetValue(previousImage, out var data))
                {
                    PronunciationText = data.Pronunciation;
                    NbOfStrokesText = data.NumberOfStrokes.ToString();
                }

                UpdateNumberOfHiraganaCommand.Execute(null);
            }
            else
            {
                MessageBox.Show("You can't go back anymore. If you want to continue, click the button 'Next'.");
            }
        }

        /// <summary>
        /// Displays the next hiragana character or offers to restart.
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

                if (imageToHiraganaData.TryGetValue(nextImage, out var data))
                {
                    PronunciationText = data.Pronunciation;
                    NbOfStrokesText = data.NumberOfStrokes.ToString();
                }

                UpdateNumberOfHiraganaCommand.Execute(null);
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
        /// Initializes the stack of hiragana characters to display and loads the first character.
        /// </summary>
        public void InitializeNextStack()
        {
            List<string> randomImageList = Images.GetRandomImageList(@"./Images/Hiragana/");
            List<HiraganaData>? hiraganaDataList = JsonData.LoadDataFromJson<HiraganaData>(@"./Images/Hiragana.json");
            if (hiraganaDataList == null)
            {
                MessageBox.Show("[HiraganaPage] : Error loading JSON data.");
                return;
            }

            foreach (string image in randomImageList)
            {
                HiraganaData? hiraganaData = JsonData.SearchData(hiraganaDataList, image);
                if (hiraganaData != null)
                {
                    imageToHiraganaData[image] = hiraganaData;
                    Next_Stack.Push(image);
                }
                else
                {
                    MessageBox.Show($"[HiraganaPage] : Data for image '{image}' not found.");
                    return;
                }
            }
            string nextImage = Next_Stack.Pop();
            Previous_Stack.Push(nextImage);
            LoadImage(nextImage);
            imageName = nextImage;

            if (imageToHiraganaData.TryGetValue(nextImage, out var data))
            {
                PronunciationText = data.Pronunciation;
                NbOfStrokesText = data.NumberOfStrokes.ToString();
            }

            cpt = 1;
            TotalHiragana = Next_Stack.Count;
            UpdateNumberOfHiraganaCommand.Execute(null);
        }

        /// <summary>
        /// Updates the display of the current hiragana character's position.
        /// </summary>
        public void UpdateNumberOfHiragana()
        {
            HiraganaText = $"{cpt}/{TotalHiragana + 1}";
        }

        /// <summary>
        /// Loads and displays a hiragana character's image.
        /// </summary>
        /// <param name="imageRelativePath">The relative path to the hiragana character's image file.</param>
        private void LoadImage(string imageRelativePath)
        {
            CurrentImage = new BitmapImage(new Uri($"/Images/Hiragana/{imageRelativePath}", UriKind.Relative));
        }
    }
}
