using Japanese.Model;
using Japanese.Utilities;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Forms = System.Windows.Forms;

namespace Japanese.ViewModel
{
    /// <summary>
    /// This class represents the ViewModel for the "AddFolderPage" in the application.
    /// It is responsible for handling user interactions and data related to adding folders containing kanji images.
    /// </summary>
    class AddFolderPageViewModel : ViewModelBase
    {
        /* -------------------- Variable -------------------- */
        private string selectedFolderPath = string.Empty;
        private string selectedJSONPath = string.Empty;

        /* -------------------- Command -------------------- */
        /// <summary>
        /// Navigates back to the "KanjiPage".
        /// </summary>
        public RelayCommand GoBackCommand => new(execute => GoBackAction());

        /// <summary>
        /// Navigates to the "AddKanjiPage".
        /// </summary>
        public RelayCommand SwitchCommand => new(execute => SwitchAction());

        /// <summary>
        /// Initiates the folder selection process.
        /// </summary>
        public RelayCommand SelectFolderCommand => new(execute => SelectFolderAction());

        /// <summary>
        /// Initiates the JSON file selection process.
        /// </summary>
        public RelayCommand SelectJSONCommand => new(execute => SelectJSONAction());

        /// <summary>
        /// Saves kanji data to JSON and copies images.
        /// </summary>
        public RelayCommand SaveCommand => new(execute => SaveAction());

        /// <summary>
        /// Deletes selected kanji data items.
        /// </summary>
        public RelayCommand DeleteCommand => new(execute => DeleteAction(execute));


        /* -------------------- Properties -------------------- */
        /// <summary>
        /// An ObservableCollection of KanjiData objects that holds the list of kanji images in the selected folder.
        /// </summary>
        private ObservableCollection<KanjiData> _kanjiList = new();

        public ObservableCollection<KanjiData> KanjiList
        {
            get { return _kanjiList; }
            set { _kanjiList = value; }
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
        /// Navigates the user to the "AddKanjiPage".
        /// </summary>
        private void SwitchAction()
        {
            FrameManager.MainFrame.Navigate(new Uri("/View/AddKanjiPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Opens a folder browser dialog to select a folder containing kanji images and adds them to the 'KanjiList'.
        /// </summary>
        private void SelectFolderAction()
        {
            Forms.FolderBrowserDialog dialog = new();
            Forms.DialogResult result = dialog.ShowDialog();

            if (result == Forms.DialogResult.OK && !string.IsNullOrEmpty(dialog.SelectedPath))
            {
                selectedFolderPath = dialog.SelectedPath;
                string[] fileNames = Directory.GetFiles(selectedFolderPath);

                foreach (string fileName in fileNames)
                {
                    string imageName = Path.GetFileName(fileName);

                    if (!KanjiList.Any(kanji => kanji.ImageName == imageName))
                    {
                        KanjiList.Add(new KanjiData { ImageName = imageName });
                    }
                }
            }
        }

        /// <summary>
        /// Allows the user to select a JSON file to import additional kanji data and updates existing kanji data in the 'KanjiList'.
        /// </summary>
        private void SelectJSONAction()
        {
            if (selectedFolderPath != string.Empty)
            {
                OpenFileDialog openFileDialog = new()
                {
                    Filter = "JSON Files (*.json) | *.json",
                    FilterIndex = 0,
                };

                bool? result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    selectedJSONPath = openFileDialog.FileName;

                    List<KanjiData>? kanjiDataList = JsonData.LoadDataFromJson<KanjiData>(selectedJSONPath);

                    if (kanjiDataList != null)
                    {
                        foreach (var jsonKanjiData in kanjiDataList)
                        {
                            if (!string.IsNullOrEmpty(jsonKanjiData.ImageName))
                            {
                                var existingKanjiData = KanjiList.FirstOrDefault(k => k.ImageName == jsonKanjiData.ImageName);

                                if (existingKanjiData != null)
                                {
                                    UpdateExistingKanjiData(existingKanjiData, jsonKanjiData);
                                }
                                else
                                {
                                    AddNewKanjiWithMissingImage(jsonKanjiData);
                                }
                            }
                            else
                            {
                                AddNewKanjiWithMissingImageNameInJSON(jsonKanjiData);
                            }
                        }

                        foreach (var existingKanjiData in KanjiList)
                        {
                            CheckAndMarkMissingKanjiData(existingKanjiData, kanjiDataList);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a folder before choosing a JSON file.");
            }
        }

        /// <summary>
        /// Updates existing kanji data with data from a JSON file.
        /// </summary>
        /// <param name="existingKanjiData">Represents an existing KanjiData object in the list of kanji characters.</param>
        /// <param name="jsonKanjiData">Represents a KanjiData object obtained from a JSON file.</param>
        private void UpdateExistingKanjiData(KanjiData existingKanjiData, KanjiData jsonKanjiData)
        {
            bool isDataComplete = !string.IsNullOrEmpty(jsonKanjiData.Hiragana) && !string.IsNullOrEmpty(jsonKanjiData.Pronunciation) &&
                                  !string.IsNullOrEmpty(jsonKanjiData.Translation) && jsonKanjiData.NumberOfStrokes > 0;

            existingKanjiData.Hiragana = jsonKanjiData.Hiragana;
            existingKanjiData.Pronunciation = jsonKanjiData.Pronunciation;
            existingKanjiData.Translation = jsonKanjiData.Translation;
            existingKanjiData.NumberOfStrokes = jsonKanjiData.NumberOfStrokes;

            if (isDataComplete)
            {
                existingKanjiData.MissingElement = false;
                existingKanjiData.ErrorMessage = string.Empty;
            }
            else
            {
                List<string> missingProperties = new();

                if (string.IsNullOrEmpty(jsonKanjiData.Hiragana))
                {
                    missingProperties.Add("Hiragana");
                }
                if (string.IsNullOrEmpty(jsonKanjiData.Pronunciation))
                {
                    missingProperties.Add("Pronunciation");
                }
                if (string.IsNullOrEmpty(jsonKanjiData.Translation))
                {
                    missingProperties.Add("Translation");
                }
                if (jsonKanjiData.NumberOfStrokes <= 0)
                {
                    missingProperties.Add("Number of Strokes");
                }

                existingKanjiData.MissingElement = true;
                if (existingKanjiData.ErrorMessage.Length > 0 && existingKanjiData.ErrorMessage[^1] != '\n')
                {
                    existingKanjiData.ErrorMessage += "\n";
                }
                existingKanjiData.ErrorMessage += $"Data missing in JSON : {string.Join(", ", missingProperties)}";
            }
        }

        /// <summary>
        /// Adds new kanji data to the list for images missing in the folder.
        /// </summary>
        /// <param name="jsonKanjiData">The KanjiData object containing information about the kanji character.</param>
        private void AddNewKanjiWithMissingImage(KanjiData jsonKanjiData)
        {
            KanjiList.Add(new KanjiData
            {
                Hiragana = jsonKanjiData.Hiragana,
                Pronunciation = jsonKanjiData.Pronunciation,
                Translation = jsonKanjiData.Translation,
                NumberOfStrokes = jsonKanjiData.NumberOfStrokes,
                MissingElement = true,
                ErrorMessage = $"Missing image in folder : '{jsonKanjiData.ImageName}'"
            });
        }

        /// <summary>
        /// Adds new kanji data to the list for images with missing names in the JSON file.
        /// </summary>
        /// <param name="jsonKanjiData">The KanjiData object containing information about the kanji character.</param>
        private void AddNewKanjiWithMissingImageNameInJSON(KanjiData jsonKanjiData)
        {
            KanjiList.Add(new KanjiData
            {
                Hiragana = jsonKanjiData.Hiragana,
                Pronunciation = jsonKanjiData.Pronunciation,
                Translation = jsonKanjiData.Translation,
                NumberOfStrokes = jsonKanjiData.NumberOfStrokes,
                MissingElement = true,
                ErrorMessage = $"Missing image name in JSON file."
            });
        }

        /// <summary>
        /// Checks for missing kanji data and marks it as missing in the 'KanjiList'.
        /// </summary>
        /// <param name="existingKanjiData">The 'KanjiData' object representing an existing kanji character in the application's data.</param>
        /// <param name="kanjiDataList">A list of 'KanjiData' objects representing the kanji data loaded from a JSON file.</param>
        private void CheckAndMarkMissingKanjiData(KanjiData existingKanjiData, List<KanjiData> kanjiDataList)
        {
            if (!kanjiDataList.Any(jsonKanjiData => jsonKanjiData.ImageName == existingKanjiData.ImageName))
            {
                if (string.IsNullOrEmpty(existingKanjiData.Hiragana) || string.IsNullOrEmpty(existingKanjiData.Pronunciation) ||
                    string.IsNullOrEmpty(existingKanjiData.Translation) || existingKanjiData.NumberOfStrokes <= 0)
                {
                    List<string> missingProperties = new();

                    if (string.IsNullOrEmpty(existingKanjiData.Hiragana))
                    {
                        missingProperties.Add("Hiragana");
                    }
                    if (string.IsNullOrEmpty(existingKanjiData.Pronunciation))
                    {
                        missingProperties.Add("Pronunciation");
                    }
                    if (string.IsNullOrEmpty(existingKanjiData.Translation))
                    {
                        missingProperties.Add("Translation");
                    }
                    if (existingKanjiData.NumberOfStrokes <= 0)
                    {
                        missingProperties.Add("Number of Strokes");
                    }

                    existingKanjiData.MissingElement = true;
                    if (existingKanjiData.ErrorMessage.Length > 0 && existingKanjiData.ErrorMessage[^1] != '\n')
                    {
                        existingKanjiData.ErrorMessage += "\n";
                    }
                    existingKanjiData.ErrorMessage += $"Data missing in JSON : {string.Join(", ", missingProperties)}";
                }
            }
        }

        /// <summary>
        /// Saves kanji data to a JSON file and copies image files to the kanji folder.
        /// </summary>
        private void SaveAction()
        {
            List<KanjiData> kanjiDataList = new();
            List<KanjiData> failedToAdd = new();
            List<string> imagePathsToCopy = new();

            foreach (var kanjiData in KanjiList)
            {
                if (!kanjiData.MissingElement && !kanjiDataList.Any(k => k.ImageName == kanjiData.ImageName))
                {
                    kanjiDataList.Add(new KanjiData
                    {
                        ImageName = kanjiData.ImageName,
                        Hiragana = kanjiData.Hiragana,
                        Pronunciation = kanjiData.Pronunciation,
                        Translation = kanjiData.Translation,
                        NumberOfStrokes = kanjiData.NumberOfStrokes
                    });

                    imagePathsToCopy.Add(Path.Combine(selectedFolderPath, kanjiData.ImageName));
                }
                else if (kanjiData.MissingElement)
                {
                    failedToAdd.Add(kanjiData);
                }
            }

            if (failedToAdd.Count > 0)
            {
                StringBuilder message = new("The following elements could not be added due to missing data:\n");
                foreach (var kanjiData in failedToAdd)
                {
                    string displayName = string.IsNullOrEmpty(kanjiData.ImageName) ? "unknown" : kanjiData.ImageName;
                    message.Append($"\t- {displayName}\n");
                }
                MessageBox.Show(message.ToString(), "Failed to Add Elements", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            JsonData.AddKanjiDataToKanjiJson(kanjiDataList);

            if (imagePathsToCopy.Count > 0)
            {
                Images.CopyImagesToKanjiFolder(imagePathsToCopy);

                foreach (var kanjiData in KanjiList.ToList())
                {
                    if (!kanjiData.MissingElement)
                    {
                        KanjiList.Remove(kanjiData);
                    }
                }
            }
        }

        /// <summary>
        /// Deletes selected kanji data items from the KanjiList.
        /// </summary>
        /// <param name="parameter">An object that represents the parameter passed to the 'DeleteAction' method.</param>
        private void DeleteAction(object parameter)
        {
            if (parameter is IList selectedItems && selectedItems.Count > 0)
            {
                foreach (var selectedItem in selectedItems.Cast<KanjiData>().ToList())
                {
                    KanjiList.Remove(selectedItem);
                }
            }
            else
            {
                MessageBox.Show("Please select one or more items.");
            }
        }
    }
}
