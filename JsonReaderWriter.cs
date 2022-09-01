using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventureGame
{
    public static class JsonFileReader
    {

        public static T ReadJson<T>(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(jsonString)!;
        }

        public static async Task<T> ReadJsonAsync<T>(string filePath)
        {
            using FileStream stream = File.OpenRead(filePath);
            return await JsonSerializer.DeserializeAsync<T>(stream);

            // To call use:
            // Root root = await JsonFileReader.ReadJsonAsync<Root>(filePath);
        }
    }
}
