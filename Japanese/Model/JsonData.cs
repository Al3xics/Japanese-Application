using Japanese.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Japanese.Model
{
    /// <summary>
    /// This class handles operations related to JSON data.
    /// </summary>
    class JsonData
    {
        /* -------------------- Method -------------------- */
        /// <summary>
        /// Loads data of type T from a JSON file.
        /// </summary>
        /// <typeparam name="T">The type of data to load.</typeparam>
        /// <param name="filePath">The path to the JSON file.</param>
        /// <returns>A list of loaded data or null if an error occurs.</returns>
        public static List<T>? LoadDataFromJson<T>(string filePath)
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(filePath)!; // 'filePath' can't be null here
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, "[]");
                }

                string json = File.ReadAllText(filePath);
                if (string.IsNullOrEmpty(json))
                {
                    MessageBox.Show($"'{filePath}' is empty or file does not exist.");
                    return null;
                }

                List<T> dataList = JsonConvert.DeserializeObject<List<T>>(json)!;
                return dataList;
            }
            catch (Exception ex)
            {
                string callingClass = GetCallingClassName();
                MessageBox.Show($"Error loading JSON data in '{callingClass}' class: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Saves a list of data to a JSON file.
        /// </summary>
        /// <typeparam name="T">The type of data to save.</typeparam>
        /// <param name="filePath">The path to the JSON file.</param>
        /// <param name="dataList">The list of data to save.</param>
        public static void SaveDataToJson<T>(string filePath, List<T> dataList)
        {
            try
            {
                string json = JsonConvert.SerializeObject(dataList, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                string callingClass = GetCallingClassName();
                MessageBox.Show($"Error saving JSON data in '{callingClass}' class: " + ex.Message);
            }
        }

        /// <summary>
        /// Searches for data in a list based on the provided image name.
        /// </summary>
        /// <typeparam name="T">The type of data to search for.</typeparam>
        /// <param name="data">The list of data to search in.</param>
        /// <param name="imageName">The image name to search for.</param>
        /// <returns>The matching data or null if not found.</returns>
        public static T? SearchData<T>(List<T> data, string imageName) where T : IDataWithImage
        {
            return data.FirstOrDefault(dataItem => dataItem.ImageName == imageName);
        }




        /// <summary>
        /// Adds KanjiData objects to an existing JSON file.
        /// </summary>
        /// <param name="kanjiDataList">The list of KanjiData to add.</param>
        public static void AddKanjiDataToKanjiJson(List<KanjiData> kanjiDataList)
        {
            try
            {
                List<KanjiData> existingKanjiDataList = LoadDataFromJson<KanjiData>(@"./Images/Kanji.json") ?? new List<KanjiData>();

                foreach (var kanjiData in kanjiDataList)
                {
                    if (!existingKanjiDataList.Any(existing => existing.ImageName == kanjiData.ImageName))
                    {
                        existingKanjiDataList.Add(kanjiData);
                    }
                }

                existingKanjiDataList.Sort((x, y) => string.Compare(x.ImageName, y.ImageName, StringComparison.Ordinal));

                SaveDataToJson(@"./Images/Kanji.json", existingKanjiDataList);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error modifying JSON data: " + ex.Message);
            }
        }




        /// <summary>
        /// Gets the name of the calling class for error reporting purposes.
        /// </summary>
        /// <returns>The name of the calling class or "Unknown" if not found.</returns>
        private static string GetCallingClassName()
        {
            StackTrace? stackTrace = new();
            StackFrame[] frames = stackTrace.GetFrames();

            if (frames.Length >= 3)
            {
                MethodBase? method = frames[2].GetMethod();
                Type? declaringType = method?.DeclaringType;
                if (declaringType != null)
                {
                    return declaringType.Name;
                }
            }

            return "Unknown";
        }
    }
}
