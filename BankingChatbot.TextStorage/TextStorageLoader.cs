using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BankingChatbot.TextStorage
{
    public class TextStorageLoader
    {
        private const string FilePath = @"C:\Users\Balint\Source\Repos\BotService\Shared\text_storage\default.json";

        private string _textFile;

        public TextStorage Storage { get; set; }

        public TextStorageLoader()
        {
            using (StreamReader streamReader = new StreamReader(FilePath))
            {
                _textFile = streamReader.ReadToEnd();
                Storage = JsonConvert.DeserializeObject<TextStorage>(_textFile);
            }
        }
    }
}
