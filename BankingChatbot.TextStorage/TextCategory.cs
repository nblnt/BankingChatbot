namespace BankingChatbot.TextStorage
{
    public enum TextCategory
    {
        COMMON_NotValidOption,
        COMMON_Error,
        COMMON_NotUnderstandable,
        COMMON_AskingMoreAccurateInput,

        GREETING,

        ACCOUNTBALANCE_MoreThanOneAccount,
        ACCOUNTBALANCE_ZeroAccount,
        ACCOUNTBALANCE_BalanceIs,

        GETCARDLIMIT_MoreThanOneCard,
        GETCARDLIMIT_ZeroCard,
        GETCARDLIMIT_WithDrawalLimit,
        GETCARDLIMIT_PurchaseLimit,
        GETCARDLIMIT_ChangeIsNotRequires,

        SELECTCARD_PleaseSelect,
        SELECTCARD_InvalidCard
    }
}