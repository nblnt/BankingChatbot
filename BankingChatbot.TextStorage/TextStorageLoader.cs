using System.IO;
using System.Net;
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
            if (Settings.Default.UsingLocal)
            {
                //_filePath = Settings.Default.TextStorageLocalPath;
                //using (StreamReader streamReader = new StreamReader(_filePath))
                //{
                //    _textFile = streamReader.ReadToEnd();                    
                //} 
                _textFile = System.Text.Encoding.Default.GetString(Resources._default);
            }
            else
            {
                using (WebClient client = new WebClient())
                {
                    _textFile = client.DownloadString(Settings.Default.TextStorageAzureUri);
                }
            }
            Storage = JsonConvert
                .DeserializeObject<TextStorage>(_textFile);
        }
    }
}
