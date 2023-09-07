using Japanese.Model;
using Japanese.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Japanese.ViewModel
{
    /// <summary>
    /// This class represents the ViewModel for the "AddKanjiPage" in the application.
    /// It handles user interactions and data related to adding individual kanji characters.
    /// </summary>
    class AddKanjiPageViewModel : ViewModelBase
    {
        /* -------------------- Variable -------------------- */
        private string selectedFilePath = string.Empty;


        /* -------------------- Command -------------------- */
        /// <summary>
        /// Navigates back to the "KanjiPage".
        /// </summary>
        public RelayCommand GoBackCommand => new(execute => GoBackAction());

        /// <summary>
        /// Navigates to the "AddFolderPage".
        /// </summary>
        public RelayCommand SwitchCommand => new(execute => SwitchAction());

        /// <summary>
        /// Initiates the image selection process.
        /// </summary>
        public RelayCommand SelectImageCommand => new(execute => SelectImageAction());

        /// <summary>
        /// Saves the entered kanji data and image.
        /// </summary>
        public RelayCommand SaveCommand => new(execute => SaveAction());


        /* -------------------- Properties -------------------- */
        /// <summary>
        /// An 'ImageSource' property that represents the currently selected image.
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
        /// A property for entering the hiragana representation of the kanji.
        /// </summary>
        private string _hiraganaTextBox = string.Empty;

        public string HiraganaTextBox
        {
            get { return _hiraganaTextBox; }
            set
            {
                _hiraganaTextBox = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// A property for entering the pronunciation of the kanji.
        /// </summary>
        private string _pronunciationTextBox = string.Empty;

        public string PronunciationTextBox
        {
            get { return _pronunciationTextBox; }
            set
            {
                _pronunciationTextBox = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// A property for entering the translation of the kanji.
        /// </summary>
        private string _translationTextBox = string.Empty;

        public string TranslationTextBox
        {
            get { return _translationTextBox; }
            set
            {
                _translationTextBox = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// A property for entering the number of strokes of the kanji.
        /// </summary>
        private string _nbOfStrokesTextBox = string.Empty;

        public string NbOfStrokesTextBox
        {
            get { return _nbOfStrokesTextBox; }
            set
            {
                _nbOfStrokesTextBox = value;
                OnPropertyChanged();
            }
        }


        /* -------------------- Method -------------------- */
        /// <summary>
        /// Navigates the user back to the "KanjiPage".
        /// </summary>
        private void GoBackAction()
        {
            FrameManager.MainFrame.Navigate(new Uri("/View/KanjiPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Navigates the user to the "AddFolderPage".
        /// </summary>
        private void SwitchAction()
        {
            FrameManager.MainFrame.Navigate(new Uri("/View/AddFolderPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Allows the user to select an image file for the kanji character.
        /// </summary>
        private void SelectImageAction()
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Images (*.jpg, *.jpeg, *.png, *.gif, *.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files (*.*)|*.*",
                FilterIndex = 0
            };

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                selectedFilePath = openFileDialog.FileName;
                CurrentImage = new BitmapImage(new Uri(selectedFilePath));
            }
        }

        /// <summary>
        /// Saves the entered kanji data to JSON and copies the selected image.
        /// </summary>
        private void SaveAction()
        {
            if (IsFormValid())
            {
                if (int.TryParse(NbOfStrokesTextBox, out int numberOfStrokes))
                {
                    KanjiData kanjiData = new()
                    {
                        ImageName = Path.GetFileName(selectedFilePath),
                        Hiragana = HiraganaTextBox,
                        Pronunciation = PronunciationTextBox,
                        Translation = TranslationTextBox,
                        NumberOfStrokes = numberOfStrokes
                    };

                    Images.CopyImagesToKanjiFolder(new List<string> { selectedFilePath });
                    JsonData.AddKanjiDataToKanjiJson(new List<KanjiData> { kanjiData });

                    CurrentImage = new BitmapImage(new Uri("/Images/Graphismes/No_Image.png", UriKind.Relative));
                    selectedFilePath = string.Empty;

                    HiraganaTextBox = string.Empty;
                    PronunciationTextBox = string.Empty;
                    TranslationTextBox = string.Empty;
                    NbOfStrokesTextBox = string.Empty;
                }
                else
                {
                    MessageBox.Show("Please enter a valid number for 'Number Of Strokes'.");
                }
            }
            else
            {
                MessageBox.Show("Please complete all fields and select an image.");
            }
        }

        /// <summary>
        /// Checks if the form data is valid before saving.
        /// </summary>
        /// <returns>True if the form data is valid, false otherwise.</returns>
        private bool IsFormValid()
        {
            if (string.IsNullOrEmpty(HiraganaTextBox) || string.IsNullOrEmpty(PronunciationTextBox) ||
                string.IsNullOrEmpty(TranslationTextBox) || string.IsNullOrEmpty(NbOfStrokesTextBox))
            {
                return false;
            }

            if (string.IsNullOrEmpty(selectedFilePath))
            {
                return false;
            }

            return true;
        }
    }
}
