namespace Japanese.Utilities
{
    /// <summary>
    /// An interface representing data that has an associated image name.
    /// </summary>
    interface IDataWithImage
    {
        /// <summary>
        /// Gets or sets the image name associated with the data.
        /// </summary>
        public string ImageName { get; set; }
    }
}
