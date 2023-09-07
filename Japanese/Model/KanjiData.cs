using Japanese.Utilities;

namespace Japanese.Model
{
    /// <summary>
    /// This class represents data related to Kanji characters and inherits from ViewModelBase and implements IDataWithImage.
    /// </summary>
    class KanjiData : ViewModelBase, IDataWithImage
    {
        /* -------------------- Properties -------------------- */
        /// <summary>
        /// A property that stores the name of the image associated with the Kanji character.
        /// </summary>
        private string _imageName = string.Empty;

        public string ImageName
        {
            get { return _imageName; }
            set
            {
                _imageName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// A property that stores the corresponding Hiragana for the Kanji character.
        /// </summary>
        private string _hiragana = string.Empty;

        public string Hiragana
        {
            get { return _hiragana; }
            set
            {
                _hiragana = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// A property that stores the pronunciation of the Kanji character.
        /// </summary>
        private string _pronunciation = string.Empty;

        public string Pronunciation
        {
            get { return _pronunciation; }
            set
            {
                _pronunciation = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// A property that stores the translation of the Kanji character.
        /// </summary>
        private string _translation = string.Empty;

        public string Translation
        {
            get { return _translation; }
            set
            {
                _translation = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// A property that stores the number of strokes in the Kanji character.
        /// </summary>
        private int _numberOfStrokes = 0;

        public int NumberOfStrokes
        {
            get { return _numberOfStrokes; }
            set
            {
                _numberOfStrokes = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FormattedNumberOfStrokes));
            }
        }

        /// <summary>
        /// A read-only property that formats the number of strokes for display.
        /// </summary>
        public string FormattedNumberOfStrokes
        {
            get { return NumberOfStrokes == 0 ? string.Empty : NumberOfStrokes.ToString(); }
        }

        /// <summary>
        /// A property that indicates whether the image is missing in the folder compared to the JSON file.
        /// </summary>
        private bool _missingElement;

        public bool MissingElement
        {
            get { return _missingElement; }
            set
            {
                _missingElement = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// A property that stores an error message to display.
        /// </summary>
        private string _errorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }
    }
}
