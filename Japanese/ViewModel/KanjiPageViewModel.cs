using Japanese.Model;
using Japanese.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Japanese.ViewModel
{
    /// <summary>
    /// Represents the ViewModel for the Kanji Page, handling Kanji display and navigation.
    /// </summary>
    class KanjiPageViewModel : ViewModelBase
    {
        /* -------------------- Variable -------------------- */
        private Stack<string> Next_Stack = new();
        private Stack<string> Previous_Stack = new();
        private int cpt;
        private int TotalKanji;
        private Dictionary<string, KanjiData> imageToKanjiData = new();
        private string imageName = string.Empty;
        private bool isEmpty = false;


        /* -------------------- Command -------------------- */
        /// <summary>
        /// Command to navigate to the Home Page.
        /// </summary>
        public RelayCommand HomePageCommand => new(execute => HomePageAction());

        /// <summary>
        /// Command to navigate to Kanji Details Page.
        /// </summary>
        public RelayCommand KanjiDetailsCommand => new(execute => KanjiDetailsAction());

        /// <summary>
        /// Command to add a folder.
        /// </summary>
        public RelayCommand AddFolderCommand => new(execute => AddFolderAction());

        /// <summary>
        /// Command to go back to the previous Kanji.
        /// </summary>
        public RelayCommand PreviousCommand => new(execute => PreviousAction());

        /// <summary>
        /// Command to go back to the next Kanji.
        /// </summary>
        public RelayCommand NextCommand => new(execute => NextAction());

        /// <summary>
        /// Command to update the total number of Kanji.
        /// </summary>
        public RelayCommand UpdateNumberOfKanjiCommand => new(execute => UpdateNumberOfKanji());


        /* -------------------- Properties -------------------- */
        /// <summary>
        /// Indicates whether the Kanji checkbox is checked.
        /// </summary>
        private bool _isKanjiChecked;
        public bool IsKanjiChecked
        {
            get { return _isKanjiChecked; }
            set
            {
                _isKanjiChecked = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Indicates whether the Hiragana checkbox is checked.
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
        /// Indicates whether the Pronunciation checkbox is checked.
        /// </summary>
        private bool _isPronunciationChecked;

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
        /// Indicates whether the Translation checkbox is checked.
        /// </summary>
        private bool _isTranslationChecked = true;

        public bool IsTranslationChecked
        {
            get { return _isTranslationChecked; }
            set
            {
                _isTranslationChecked = value;
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
                IsKanjiChecked = !IsKanjiChecked;
                IsHiraganaChecked = !IsHiraganaChecked;
                IsPronunciationChecked = !IsPronunciationChecked;
                IsTranslationChecked = !IsTranslationChecked;
                IsNbOfStrokesChecked = !IsNbOfStrokesChecked;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// An 'ImageSource' property representing the currently displayed Kanji character's image.
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
        /// A property to display the hiragana of the Kanji character.
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

        /// <summary>
        /// A property to display the pronunciation of the Kanji character.
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
        /// A property to display the translation of the Kanji character.
        /// </summary>
        private string _translationText = string.Empty;

        public string TranslationText
        {
            get { return _translationText; }
            set
            {
                _translationText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// A property to display the number of strokes of the Kanji character.
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
        /// A property to display the number of Kanji already done.
        /// </summary>
        private string _kanjiText = string.Empty;

        public string KanjiText
        {
            get { return _kanjiText; }
            set
            {
                _kanjiText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// A property to display diffrent set of kanji possible (all kanji, 10 set kanji, 20 set kanji, ...)
        /// </summary>
        private ComboBoxItem _selectedComboBoxItem = new();

        public ComboBoxItem SelectedComboBoxItem
        {
            get { return _selectedComboBoxItem; }
            set
            {
                _selectedComboBoxItem = value;
                OnPropertyChanged();
                HandleComboBoxItemSelection();
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
        /// Navigates to the Kanji Details Page.
        /// </summary>
        private void KanjiDetailsAction()
        {
            FrameManager.MainFrame.Navigate(new Uri("/View/KanjiDetailsPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Navigates to the Add Folder Page.
        /// </summary>
        public void AddFolderAction()
        {
            FrameManager.MainFrame.Navigate(new Uri("/View/AddFolderPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Go back to the previous Kanji item.
        /// </summary>
        private void PreviousAction()
        {
            if (!isEmpty)
            {
                if (Previous_Stack.Count > 1)
                {
                    cpt--;
                    string previousImage = Previous_Stack.Pop();
                    Next_Stack.Push(imageName);
                    LoadImage(previousImage);
                    imageName = previousImage;

                    if (imageToKanjiData.TryGetValue(previousImage, out var data))
                    {
                        HiraganaText = data.Hiragana;
                        PronunciationText = data.Pronunciation;
                        TranslationText = data.Translation;
                        NbOfStrokesText = data.NumberOfStrokes.ToString();
                    }

                    UpdateNumberOfKanjiCommand.Execute(null);
                }
                else
                {
                    MessageBox.Show("You can't go back anymore. If you want to continue, click the button 'Next'.");
                }
            }
        }

        /// <summary>
        /// Go to the next Kanji item.
        /// </summary>
        private void NextAction()
        {
            if (!isEmpty)
            {
                if (Next_Stack.Count > 0)
                {
                    cpt++;
                    string nextImage = Next_Stack.Pop();
                    Previous_Stack.Push(imageName);
                    LoadImage(nextImage);
                    imageName = nextImage;

                    if (imageToKanjiData.TryGetValue(nextImage, out var data))
                    {
                        HiraganaText = data.Hiragana;
                        PronunciationText = data.Pronunciation;
                        TranslationText = data.Translation;
                        NbOfStrokesText = data.NumberOfStrokes.ToString();
                    }

                    UpdateNumberOfKanjiCommand.Execute(null);
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("You have reached the end. Do you want to restart ?", "Restart ?", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        Next_Stack.Clear();
                        Previous_Stack.Clear();

                        Dictionary<string, int> comboBoxToKanjiCount = new()
                        {
                            { "Set of 10 Kanji", 10 },
                            { "Set of 20 Kanji", 20 },
                            { "Set of 30 Kanji", 30 },
                            { "Set of 40 Kanji", 40 },
                            { "Set of 50 Kanji", 50 }
                        };

                        ComboBoxItem selectedItem = SelectedComboBoxItem;

                        if (selectedItem != null)
                        {
                            string selectedText = selectedItem.Content.ToString()!; // can't be null here

                            if (selectedText == "Set of all Kanji")
                            {
                                InitializeNextStack();
                            }
                            else if (comboBoxToKanjiCount.TryGetValue(selectedText, out int numberOfKanjiToInitialize))
                            {
                                InitializeNextStack(numberOfKanjiToInitialize);
                            }
                            else
                            {
                                MessageBox.Show("No set selected !");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the next stack of Kanji items.
        /// </summary>
        public void InitializeNextStack()
        {
            Next_Stack.Clear();
            Previous_Stack.Clear();

            if (Images.GetRandomImageList(@"./Images/Kanji/").Count != 0 &&
                JsonData.LoadDataFromJson<KanjiData>(@"./Images/Kanji.json") != null)
            {
                List<string> randomImageList = Images.GetRandomImageList(@"./Images/Kanji/");
                List<KanjiData>? kanjiDataList = JsonData.LoadDataFromJson<KanjiData>(@"./Images/Kanji.json");

                foreach (string image in randomImageList)
                {
                    KanjiData? kanjiData = JsonData.SearchData(kanjiDataList!, image); // 'kanjiDataList' can't be null here
                    if (kanjiData != null)
                    {
                        imageToKanjiData[image] = kanjiData;
                        Next_Stack.Push(image);
                    }
                    else
                    {
                        MessageBox.Show($"[KanjiPage] : Data for image '{image}' not found.");
                        return;
                    }
                }
                string nextImage = Next_Stack.Pop();
                Previous_Stack.Push(nextImage);
                LoadImage(nextImage);
                imageName = nextImage;

                if (imageToKanjiData.TryGetValue(nextImage, out var data))
                {
                    HiraganaText = data.Hiragana;
                    PronunciationText = data.Pronunciation;
                    TranslationText = data.Translation;
                    NbOfStrokesText = data.NumberOfStrokes.ToString();
                }

                cpt = 1;
                TotalKanji = Next_Stack.Count;
                UpdateNumberOfKanjiCommand.Execute(null);
            }
            else
            {
                MessageBox.Show($"No images and json files found.\nPlease add them by clicking on '+' at the bottom right.");
                isEmpty = true;
                return;
            }
        }

        /// <summary>
        /// Initializes the next stack of Kanji items with a specified number of Kanji to initialize.
        /// </summary>
        /// <param name="numberOfKanjiToInitialize">The number of Kanji items to initialize.</param>
        public void InitializeNextStack(int numberOfKanjiToInitialize)
        {
            Next_Stack.Clear();
            Previous_Stack.Clear();

            if (numberOfKanjiToInitialize > 0)
            {
                if (Images.GetRandomImageList(@"./Images/Kanji/").Count != 0 &&
                    JsonData.LoadDataFromJson<KanjiData>(@"./Images/Kanji.json") != null)
                {
                    List<string> randomImageList = Images.GetRandomImageList(@"./Images/Kanji/");
                    List<KanjiData>? kanjiDataList = JsonData.LoadDataFromJson<KanjiData>(@"./Images/Kanji.json");

                    foreach (string image in randomImageList.Take(numberOfKanjiToInitialize))
                    {
                        KanjiData? kanjiData = JsonData.SearchData(kanjiDataList!, image); // 'kanjiDataList' can't be null here
                        if (kanjiData != null)
                        {
                            imageToKanjiData[image] = kanjiData;
                            Next_Stack.Push(image);
                        }
                        else
                        {
                            MessageBox.Show($"[KanjiPage] : Data for image '{image}' not found.");
                            return;
                        }
                    }
                    string nextImage = Next_Stack.Pop();
                    Previous_Stack.Push(nextImage);
                    LoadImage(nextImage);
                    imageName = nextImage;

                    if (imageToKanjiData.TryGetValue(nextImage, out var data))
                    {
                        HiraganaText = data.Hiragana;
                        PronunciationText = data.Pronunciation;
                        TranslationText = data.Translation;
                        NbOfStrokesText = data.NumberOfStrokes.ToString();
                    }

                    cpt = 1;
                    TotalKanji = Next_Stack.Count;
                    UpdateNumberOfKanjiCommand.Execute(null);
                }
                else
                {
                    MessageBox.Show($"No images and json files found.\nPlease add them by clicking on '+' at the bottom right.");
                    isEmpty = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Updates the display of the current Kanji item count.
        /// </summary>
        public void UpdateNumberOfKanji()
        {
            KanjiText = $"{cpt}/{TotalKanji+1}";
        }

        /// <summary>
        /// Loads an image based on the provided image relative path.
        /// </summary>
        /// <param name="imageRelativePath">The relative path of the image to load.</param>
        private void LoadImage(string imageRelativePath)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relativeImagePath = $"Images/Kanji/{imageRelativePath}";
            string absoluteImagePath = Path.Combine(baseDirectory, relativeImagePath);

            try
            {
                CurrentImage = new BitmapImage(new Uri(absoluteImagePath, UriKind.Absolute));
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message);
            }
        }

        /// <summary>
        /// Handles the selection of a ComboBox item for Kanji set.
        /// </summary>
        public void HandleComboBoxItemSelection()
        {
            if (SelectedComboBoxItem is ComboBoxItem selectedItem)
            {
                string selectedText = selectedItem.Content.ToString()!; // can't be null here

                switch (selectedText)
                {
                    case "Set of all Kanji":
                        if (TotalKanji != imageToKanjiData.Count)
                        {
                            InitializeNextStack();
                        }
                        break;

                    case "Set of 10 Kanji":
                        if (TotalKanji != 10)
                        {
                            InitializeNextStack(10);
                        }
                        break;

                    case "Set of 20 Kanji":
                        if (TotalKanji != 20)
                        {
                            InitializeNextStack(20);
                        }
                        break;

                    case "Set of 30 Kanji":
                        if (TotalKanji != 30)
                        {
                            InitializeNextStack(30);
                        }
                        break;

                    case "Set of 40 Kanji":
                        if (TotalKanji != 40)
                        {
                            InitializeNextStack(40);
                        }
                        break;

                    case "Set of 50 Kanji":
                        if (TotalKanji != 50)
                        {
                            InitializeNextStack(50);
                        }
                        break;

                    default:
                        MessageBox.Show("No set selected !");
                        break;
                }
            }
        }
    }
}
