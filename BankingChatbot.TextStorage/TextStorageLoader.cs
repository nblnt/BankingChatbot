using System.IO;
using BankingChatbot.TextStorage.Properties;
using Newtonsoft.Json;

namespace BankingChatbot.TextStorage
{
    public class TextStorageLoader
    {
        private string _filePath;

        private string _textFile;

        public TextStorage Storage { get; set; }

        public TextStorageLoader()
        {
            using (StreamReader streamReader = new StreamReader(_filePath))
            {
                _filePath = Settings.Default.TextStoragePath;
                _textFile = streamReader.ReadToEnd();
                Storage = JsonConvert
                    .DeserializeObject<TextStorage>(_textFile);
            }
        }
    }
}
