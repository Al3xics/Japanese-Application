using Japanese.Utilities;

namespace Japanese.Model
{
    /// <summary>
    /// This class represents data related to Hiragana characters and implements 'IDataWithImage'.
    /// </summary>
    class HiraganaData : IDataWithImage
    {
        /* -------------------- Properties -------------------- */
        /// <summary>
        /// A property that stores the name of the image associated with the Hiragana character.
        /// </summary>
        public string ImageName { get; set; } = string.Empty;

        /// <summary>
        /// A property that stores the pronunciation of the Hiragana character.
        /// </summary>
        public string Pronunciation { get; set; } = string.Empty;

        /// <summary>
        /// A property that stores the number of strokes in the Hiragana character.
        /// </summary>
        public int NumberOfStrokes { get; set; } = 0;
    }
}
