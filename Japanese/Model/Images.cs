using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Japanese.Model
{
    /// <summary>
    /// This class provides methods for working with image files, including retrieving random images and copying images to a destination folder.
    /// </summary>
    class Images
    {
        /* -------------------- Method -------------------- */
        /// <summary>
        /// Retrieves a list of random image file names from a specified directory with supported image extensions.
        /// </summary>
        /// <param name="relativePath">The relative path to the directory containing images.</param>
        /// <returns>A list of random image file names or an empty list if the directory doesn't exist.</returns>
        public static List<string> GetRandomImageList(string relativePath)
        {
            var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            if (!Directory.Exists(relativePath))
            {
                Directory.CreateDirectory(relativePath);
                return new List<string>();
            }

            List<string> randomImageList = Directory.GetFiles(relativePath)
                                                    .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLower()))
                                                    .Select(file => Path.GetFileName(file))
                                                    .ToList();

            Random random = new();
            int n = randomImageList.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                (randomImageList[n], randomImageList[k]) = (randomImageList[k], randomImageList[n]);
            }
            return randomImageList;
        }

        /// <summary>
        /// Copies a list of source image files to a destination folder.
        /// </summary>
        /// <param name="sourcePaths">A list of source file paths to copy.</param>
        public static void CopyImagesToKanjiFolder(List<string> sourcePaths)
        {
            try
            {
                string destinationFolder = @"./Images/Kanji/";
                bool duplicateMessageShown = false;

                foreach (string sourcePath in sourcePaths)
                {
                    if (File.Exists(sourcePath))
                    {
                        string fileName = Path.GetFileName(sourcePath);
                        string destinationPath = Path.Combine(destinationFolder, fileName);

                        if (File.Exists(destinationPath))
                        {
                            duplicateMessageShown = true;
                            continue;
                        }

                        File.Copy(sourcePath, destinationPath, true);
                    }
                    else
                    {
                        MessageBox.Show($"The source file '{sourcePath}' does not exist.");
                    }
                }

                if (duplicateMessageShown)
                {
                    MessageBox.Show("At least one image already exists in the Kanji folder. ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while copying the image : \n" + ex.Message);
            }
        }
    }
}
