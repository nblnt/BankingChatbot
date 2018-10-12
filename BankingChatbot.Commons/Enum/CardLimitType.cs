using System;

namespace BankingChatbot.Commons.Enum
{
    [Serializable]
    public enum CardLimitType
    {
        PurchaseLimit = 0,
        CashWithdrawalLimit = 1,
        All = 3
    }
}
