using System;
using System.Threading.Tasks;
using BankingChatbot.Commons.Enum;
using BankingChatbot.Commons.Util;
using BankingChatbot.TextStorage;
using BankingChatBot.DAL.EntityFramework.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotService.Dialogs
{
    [Serializable]
    public class SetCardLimitDialog : DialogBase<CardLimitModificationResult>
    {
        private int _cardId;

        private DebitCard _selectedCard;

        private string _cardCurrencyIso;

        private CardLimitType _limitType;

        public SetCardLimitDialog(int cardId, CardLimitType limitType)
        {
            _cardId = cardId;
            _limitType = limitType;
            _selectedCard = DAL.GetDebitCard(_cardId);
            _cardCurrencyIso = DAL.GetIsoCurrency(_cardId);
        }

        public override async Task StartAsync(IDialogContext context)
        {
            switch (_limitType)
            {
                case CardLimitType.PurchaseLimit:
                    await context.PostAsync(TextProvider.Provide(TextCategory.SETCARDLIMIT_OldPurchaseLimit) +
                                            _selectedCard.DailyPaymentLimit + " " + _cardCurrencyIso);
                    await context.PostAsync(TextProvider.Provide(TextCategory.SETCARDLIMIT_InputPurchaseLimit));
                    context.Wait(ResumeAfterLimitChangedAsync);
                    break;
                case CardLimitType.CashWithdrawalLimit:
                    await context.PostAsync(TextProvider.Provide(TextCategory.SETCARDLIMIT_OldWithdrawalLimit) +
                                            _selectedCard.DailyCashWithdrawalLimit + " " + _cardCurrencyIso);
                    await context.PostAsync(TextProvider.Provide(TextCategory.SETCARDLIMIT_InputWithdrawalLimit));
                    context.Wait(ResumeAfterLimitChangedAsync);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task ResumeAfterLimitChangedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            string newLimitAsString = (await result).Text;
            int newLimit;
            if (!int.TryParse(newLimitAsString, out newLimit))
            {
                await context.PostAsync(TextProvider.Provide(TextCategory.SETCARDLIMIT_InvalidAmount));
                context.Wait(ResumeAfterLimitChangedAsync);
            }
            else
            {
                DAL.UpdateCardLimit(_cardId, _limitType, newLimit);

                CardLimitModificationResult modificationResult;
                switch (_limitType)
                {
                    case CardLimitType.PurchaseLimit:
                        modificationResult = new CardLimitModificationResult()
                        {
                            PurchaseLimitChanged = true,
                            PurchaseLimit = newLimit
                        };
                        await context.PostAsync(TextProvider.Provide(TextCategory.SETCARDLIMIT_PurchaseLimitChanged) +
                                                newLimit + " " + _cardCurrencyIso);
                        break;
                    case CardLimitType.CashWithdrawalLimit:
                        modificationResult = new CardLimitModificationResult()
                        {
                            WithDrawalLimitChanged = true,
                            WithDrawalLimit = newLimit
                        };
                        await context.PostAsync(TextProvider.Provide(TextCategory.SETCARDLIMIT_WithdrawalLimitChanged) +
                                                newLimit + " " + _cardCurrencyIso);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


                context.Done(modificationResult);
            }
        }
    }
}