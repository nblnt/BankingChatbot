using System;

namespace BankingChatbot.Commons.Util
{
    [Serializable]
    public class CardLimitModificationResult
    {
        public bool? WithDrawalLimitChanged { get; set; }

        public int? WithDrawalLimit { get; set; }

        public bool? PurchaseLimitChanged { get; set; }

        public int? PurchaseLimit { get; set; }
    }
}