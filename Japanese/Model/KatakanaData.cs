using Japanese.Utilities;

namespace Japanese.Model
{
    /// <summary>
    /// This class represents data related to Katakana characters and implements 'IDataWithImage'.
    /// </summary>
    class KatakanaData : IDataWithImage
    {
        /* -------------------- Properties -------------------- */
        /// <summary>
        /// A property that stores the name of the image associated with the Katakana character.
        /// </summary>
        public string ImageName { get; set; } = string.Empty;

        /// <summary>
        /// A property that stores the pronunciation of the Katakana character.
        /// </summary>
        public string Pronunciation { get; set; } = string.Empty;

        /// <summary>
        /// A property that stores the number of strokes in the Katakana character.
        /// </summary>
        public int NumberOfStrokes { get; set; } = 0;
    }
}
