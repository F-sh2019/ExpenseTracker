 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExpenseTracker.Model
{
    public static class FileManager
    {
        private static string defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static string ReadFileData(string fileName)
        {
            // The File path for user
            string jsonFilePath= Path.Combine(defaultPath, $"{fileName.ToLower()}.json");

            // Get all the File names from Directory that match *.json pattern.
            var filePaths = Directory.EnumerateFiles(defaultPath, $"*.json");

            // IF there are any files in the directory, then search for file name
            if (filePaths != null && filePaths.Any())
            {
                var userFileFound = filePaths.Where(filepath => filepath.ToLower() == jsonFilePath.ToLower()).FirstOrDefault();
                if (!string.IsNullOrEmpty(userFileFound))
                {
                    return File.ReadAllText(jsonFilePath);
                }
            }
            return null;
        }

        public static bool SaveDataToFile(string fileName, string data)
        {
            // Create File name
            var jsonFileName = $"{fileName.ToLower()}.json";
            File.WriteAllText(Path.Combine(defaultPath, jsonFileName), data);
            return true;
        }
    }
}
