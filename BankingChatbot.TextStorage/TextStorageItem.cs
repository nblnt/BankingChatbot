using System.Collections.Generic;

namespace BankingChatbot.TextStorage
{
    public class TextStorageItem
    {
        public TextCategory Category { get; set; }

        public List<string> Texts { get; set; }
    }
}