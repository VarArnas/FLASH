using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FirstLab.src.back_end
{
    public class DataManager
    {
        private static string PATH = AppDomain.CurrentDomain.BaseDirectory + "../../../DataFiles/";
        private static string FILE_NAME = "Set";

        public static void SaveFlashcardSet(FlashcardSet flashcardSet)
        {
            string json = JsonSerializer.Serialize(flashcardSet);
            File.WriteAllText(PATH + FILE_NAME + ".json", json);
        }

        public static FlashcardSet LoadFlashcard(string fileName)
        {
            FlashcardSet? flashcardSet = null;
            if (File.Exists(PATH + fileName))
            {
                string jsonContent = File.ReadAllText(PATH + fileName);
                flashcardSet = JsonSerializer.Deserialize<FlashcardSet>(jsonContent);
            }

            return flashcardSet;
        }
    }
}
