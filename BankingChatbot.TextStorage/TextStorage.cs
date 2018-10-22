using System.Collections.Generic;

namespace BankingChatbot.TextStorage
{
    public class TextStorage
    {
        public string StorageName { get; set; }

        public string CreatedBy { get; set; }

        public string Language { get; set; }

        public bool Valid { get; set; }

        public List<TextStorageItem> Storage { get; set; }
    }
}