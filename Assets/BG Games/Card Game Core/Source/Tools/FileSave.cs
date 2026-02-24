using System;
using System.IO;
using UnityEngine;

namespace BG_Games.Card_Game_Core.Tools
{
    public static class FileSave
    {
        public static void DeleteFile(string fullPath)
        {
            File.Delete(fullPath);
        }
        public static void Save(string data, string fullPath)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                using (FileStream fs = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.Write(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error occured when trying to save file: {fullPath} + \n + {ex.Message}");
                throw;
            }
        }
        public static bool Load(string fullPath, out string result)
        {
            if (File.Exists(fullPath))
            {
                try
                {
                    string loaded;

                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            loaded = reader.ReadToEnd();
                        }
                    }

                    result = loaded;
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error occured when trying to load file: {fullPath} + \n + {ex.Message}");
                    throw;
                }
            }

            result = default(string);
            return false;
        }
    }
}