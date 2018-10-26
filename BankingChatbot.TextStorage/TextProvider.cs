using System;
using System.Linq;

namespace BankingChatbot.TextStorage
{
    public static class TextProvider
    {
        private static TextStorageLoader _storageLoader;

        private static TextStorage _storage;

        static TextProvider()
        {
            _storageLoader = new TextStorageLoader();
            _storage = _storageLoader.Storage;
        }

        public static string Provide(TextCategory category, int specifiedTextIndex = -1)
        {
            TextStorageItem textsByCategory = _storage.Storage.SingleOrDefault(x => x.Category == category);
            if (textsByCategory != null)
            {
                int maxIndex = textsByCategory.Texts.Count;
                int index;
                if (specifiedTextIndex == -1)
                {
                    index = new Random().Next(maxIndex);
                }
                else if (specifiedTextIndex <= maxIndex)
                {
                    index = specifiedTextIndex;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(
                        $"There is only {maxIndex} text item in the {category.ToString()} category. {specifiedTextIndex} is out of this range");
                }
                string retVal = textsByCategory.Texts[index];
                return retVal;
            }
            throw new Exception("An error occured while resolve text data from text storage");
        }
    }
}