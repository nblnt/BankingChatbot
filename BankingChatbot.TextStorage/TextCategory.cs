namespace BankingChatbot.TextStorage
{
    public enum TextCategory
    {
        COMMON_NotValidOption,
        COMMON_Error,
        COMMON_NotUnderstandable,
        COMMON_AskingMoreAccurateInput,
        COMMON_HelpMore,

        INTERRUPTION_Help,
        INTERRUPTION_Cancel,   

        GREETING,
        GREETING_UserAdded,

        ACCOUNTBALANCE_MoreThanOneAccount,
        ACCOUNTBALANCE_ZeroAccount,
        ACCOUNTBALANCE_BalanceIs,

        GETCARDLIMIT_MoreThanOneCard,
        GETCARDLIMIT_ZeroCard,
        GETCARDLIMIT_WithDrawalLimit,
        GETCARDLIMIT_PurchaseLimit,

        SELECTCARD_PleaseSelect,
        SELECTCARD_InvalidCardIdentifier,

        SETCARDLIMIT_WishToChange,
        SETCARDLIMIT_WontChange,
        SETCARDLIMIT_TooManyAttempt,
        SETCARDLIMIT_PleaseSelectLimitType,
        SETCARDLIMIT_PurchaseLimitDesc,
        SETCARDLIMIT_WithDrawalLimitDesc,
        SETCARDLIMIT_BothLimitTypeDesc
    }
}