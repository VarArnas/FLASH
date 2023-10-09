using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace FirstLab.src.back_end
{
    public static class DataManager
    {
        private static string PATH = AppDomain.CurrentDomain.BaseDirectory + "../../../DataFiles/";
        private static string FILE_NAME = "Set";
        private static string LOG_FILE_NAME = "Logs";

        private static void SaveFlashcardSet(FlashcardSet flashcardSet, int fileNumber)
        {
            string json = JsonSerializer.Serialize(flashcardSet);
            File.WriteAllText(PATH + FILE_NAME + fileNumber + ".json", json);
        }

        public static void SaveAllFlashcardSets(Collection<FlashcardSet> flashcardSets)
        {
            DeleteFiles();
            for (int i = 0; i < flashcardSets.Count; i++)
            {
                SaveFlashcardSet(flashcardSets[i], i + 1);
            }
        }

        private static FlashcardSet LoadFlashcardSet(string filePath)
        {
            FlashcardSet? flashcardSet = null;
            if (File.Exists(filePath))
            {
                string jsonContent = File.ReadAllText(filePath);
                flashcardSet = JsonSerializer.Deserialize<FlashcardSet>(jsonContent);
            }

            return flashcardSet;
        }

        public static Collection<FlashcardSet> LoadAllFlashcardSets()
        {
            Collection<FlashcardSet>? flashcardSets = new Collection<FlashcardSet>();
            string[] files = Directory.GetFiles(PATH, "Set*.json");
            foreach (string file in files)
            {
                flashcardSets.Add(LoadFlashcardSet(file));
            }

            return flashcardSets;
        }

        private static void DeleteFiles()
        {
            string[] files = Directory.GetFiles(PATH, "Set*.json");
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }

        public static void SaveLogs(ObservableCollection<FlashcardSetLog> logs)
        {
            string json = JsonSerializer.Serialize(logs);
            File.WriteAllText(PATH + LOG_FILE_NAME + ".json", json);
        }

        public static ObservableCollection<FlashcardSetLog> LoadLogs()
        {
            ObservableCollection<FlashcardSetLog> logs = new ObservableCollection<FlashcardSetLog>();
            string filePath = PATH + LOG_FILE_NAME + ".json";

            if (File.Exists(filePath))
            {
                string jsonContent = File.ReadAllText(filePath);
                logs = JsonSerializer.Deserialize<ObservableCollection<FlashcardSetLog>>(jsonContent);
            }

            return logs;
        }
    }
}
