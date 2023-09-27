using System;
using System.IO;
using System.Text;

namespace FirstLab.src.back_end.utilities
{
    public static class FileUtility
    {
        public static string? ReadAppNameFromFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        int bufferSize = 1024;
                        byte[] buffer = new byte[bufferSize];
                        StringBuilder fileData = new StringBuilder();

                        int bytesRead;
                        while ((bytesRead = fileStream.Read(buffer, 0, bufferSize)) > 0)
                        {
                            fileData.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                        }

                        return fileData.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }
}
