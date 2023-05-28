using System.Net.Http.Json;

namespace Client
{
    class Program
    {
        private static FileInfo fileForRead;
        private static FileInfo fileForWrite;
        public static string ReadText(string pathFileForRead, string pathFileForWrite)
        {
            string text = null;
            fileForRead = new FileInfo(pathFileForRead);
            fileForWrite = new FileInfo(pathFileForWrite);
            if (!fileForRead.Exists)
                throw new Exception($"File with path: {fileForRead.FullName} doesn't exists!");
            if (!fileForWrite.Exists)
            {
                var forRead = File.Create(fileForRead.FullName);
                forRead.Close();
            }

            text = File.ReadAllText(fileForRead.FullName, System.Text.Encoding.Default).Replace("\n", " ");

            Console.WriteLine($"File from {fileForRead.FullName} read successfully!");

            return text;
        }

        public static void WriteToFile(Dictionary<string, int> wordCountDictionary)
        {
            StreamWriter fileWriter = new StreamWriter(fileForWrite.FullName, false, System.Text.Encoding.Default);
            foreach (var wordCountPair in wordCountDictionary)
            {
                fileWriter.WriteLine($"{wordCountPair.Key} {wordCountPair.Value}");
            }

            fileWriter.Close();
            Console.WriteLine($"Words have been successfully written to file: {fileForWrite.FullName}");
            Console.WriteLine($"Total: {wordCountDictionary.Count()}");
        }

        public static async Task<Dictionary<string, int>> SendRequestAsync(string readedText)
        {

            string adaptiveUri = "https://localhost:7050/wordCounter";
            Dictionary<string, int> answer;
            
            using (HttpClient httpClient = new HttpClient())
            {

                var response = await httpClient.PostAsJsonAsync(adaptiveUri, readedText);
                answer = await response.Content.ReadFromJsonAsync<Dictionary<string, int>>();
            }

            return answer;
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Input file's path for read: ");
            string pathForRead = Console.ReadLine();
            Console.WriteLine("Input file's path for write: ");
            string pathForWrite = Console.ReadLine();

            string readedText = Program.ReadText(pathForRead, pathForWrite);

            var result = await SendRequestAsync(readedText);

            WriteToFile(result);

            Console.ReadKey();
        }
    }
}