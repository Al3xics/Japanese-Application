using Japanese.Model;
using Japanese.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Japanese.ViewModel
{
    /// <summary>
    /// Represents the ViewModel for the Kanji Details Page, handling Kanji data and actions.
    /// </summary>
    class KanjiDetailsPageViewModel : ViewModelBase
    {
        /* -------------------- Command -------------------- */
        /// <summary>
        /// Command to navigate back to the previous page.
        /// </summary>
        public RelayCommand GoBackCommand => new(execute => GoBackAction());

        /// <summary>
        /// Command to download Kanji data.
        /// </summary>
        public RelayCommand DownloadDataCommand => new(execute => DownloadDataAction());

        /// <summary>
        /// Command to delete selected Kanji data.
        /// </summary>
        public RelayCommand DeleteCommand => new(execute => DeleteAction(execute));


        /* -------------------- Constructor -------------------- */
        /// <summary>
        /// Initializes a new instance of the KanjiDetailsPageViewModel class.
        /// </summary>
        public KanjiDetailsPageViewModel()
        {
            List<KanjiData> jsonData = JsonData.LoadDataFromJson<KanjiData>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Kanji.json")) ?? new List<KanjiData>();

            _kanjiList = new(jsonData);
            _numberOfKanji = $"Total : {jsonData.Count}";
        }


        /* -------------------- Properties -------------------- */
        /// <summary>
        /// Search text for filtering Kanji data.
        /// </summary>
        private string _searchText = string.Empty;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged();
                UpdateKanjiFilter();
            }
        }

        /// <summary>
        /// Collection of Kanji in the application.
        /// </summary>
        private ObservableCollection<KanjiData> _kanjiList;

        public ObservableCollection<KanjiData> KanjiList
        {
            get { return _kanjiList; }
            set
            {
                _kanjiList = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Total number of Kanji in the collection.
        /// </summary>
        private string _numberOfKanji;

        public string NumberOfKanji
        {
            get { return _numberOfKanji; }
            set
            {
                _numberOfKanji = value;
                OnPropertyChanged();
            }
        }


        /* -------------------- Method -------------------- */
        /// <summary>
        /// Navigates back to the Kanji Page.
        /// </summary>
        private void GoBackAction()
        {
            FrameManager.MainFrame.Navigate(new Uri("/View/KanjiPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Allows you to download all Kanji images, and the 'Kanji.json' file.
        /// </summary>
        private void DownloadDataAction()
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(KanjiList);

            var filteredKanjiList = collectionView.Cast<KanjiData>().ToList();

            if (filteredKanjiList.Count > 0)
            {
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string destinationRootFolder = Path.Combine(dialog.SelectedPath, "Kanji Data");
                    string destinationImagesFolder = Path.Combine(destinationRootFolder, "Images");
                    Directory.CreateDirectory(destinationImagesFolder);

                    foreach (var kanjiData in filteredKanjiList)
                    {
                        string sourceImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Kanji", kanjiData.ImageName);
                        string destinationImagePath = Path.Combine(destinationImagesFolder, kanjiData.ImageName);

                        if (File.Exists(sourceImagePath))
                        {
                            File.Copy(sourceImagePath, destinationImagePath, true);
                        }
                    }

                    string kanjiJsonFilePath = Path.Combine(destinationRootFolder, "Kanji.json");
                    JsonData.SaveDataToJson(kanjiJsonFilePath, filteredKanjiList);

                    MessageBox.Show("Data downloaded successfully!");
                }
            }
            else
            {
                MessageBox.Show("No data to download.");
            }
        }

        /// <summary>
        /// Deletes selected kanji data items from the KanjiList.
        /// </summary>
        /// <param name="parameter">An object that represents the parameter passed to the 'DeleteAction' method.</param>
        private void DeleteAction(object parameter)
        {
            try
            {
                if (parameter is IList selectedItems && selectedItems.Count > 0)
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure you want to delete the selected items?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        string kanjiImagesDirectory = Path.Combine(baseDirectory, "Images", "Kanji");
                        string kanjiJsonFilePath = Path.Combine(baseDirectory, "Images", "Kanji.json");

                        List<KanjiData> kanjiList = JsonData.LoadDataFromJson<KanjiData>(kanjiJsonFilePath) ?? new List<KanjiData>();

                        foreach (var selectedItem in selectedItems.Cast<KanjiData>().ToList())
                        {
                            string imagePath = Path.Combine(kanjiImagesDirectory, selectedItem.ImageName);
                            if (File.Exists(imagePath))
                            {
                                File.Delete(imagePath);
                            }

                            KanjiData? kanjiToRemove = kanjiList.FirstOrDefault(kanji => kanji.ImageName == selectedItem.ImageName);
                            if (kanjiToRemove != null)
                            {
                                kanjiList.Remove(kanjiToRemove);
                            }
                            KanjiList.Remove(selectedItem);
                        }
                        JsonData.SaveDataToJson(kanjiJsonFilePath, kanjiList);
                        ICollectionView collectionView = CollectionViewSource.GetDefaultView(KanjiList);
                        NumberOfKanji = $"Total : {collectionView.Cast<KanjiData>().Count()}";
                    }
                }
                else
                {
                    MessageBox.Show("Please select one or more items.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Updates the user interface based on the searched Kanji.
        /// </summary>
        private void UpdateKanjiFilter()
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(KanjiList);

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                collectionView.Filter = null;
            }
            else
            {
                string searchText = SearchText.ToLower();
                collectionView.Filter = item =>
                {
                    if (item is KanjiData kanjiData)
                    {
                        return kanjiData.ImageName.ToLower().Contains(searchText) ||
                               kanjiData.Hiragana.ToLower().Contains(searchText) ||
                               kanjiData.Pronunciation.ToLower().Contains(searchText) ||
                               kanjiData.Translation.ToLower().Contains(searchText) ||
                               kanjiData.NumberOfStrokes.ToString().ToLower().Contains(searchText);
                    }
                    return false;
                };
            }

            NumberOfKanji = $"Total : {collectionView.Cast<KanjiData>().Count()}";
        }
    }
}
