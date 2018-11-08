using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingChatbot.TextStorage;
using BankingChatBot.DAL.EntityFramework.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotService.Dialogs
{
    [Serializable]
    public class GetCardLimitDialog : DialogBase<object>
    {
        private readonly int _clientId;

        private List<DebitCard> _clientDebitCards;

        public GetCardLimitDialog(int clientId)
        {
            _clientId = clientId;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            IMessageActivity message = await result;

            _clientDebitCards = DAL.GetClientDebitCards(_clientId);

            switch (_clientDebitCards.Count)
            {
                case 1:
                    int cardId = _clientDebitCards.Single().DebitCardId;
                    await PostLimitInformationAsync(context, cardId);
                    context.Done<object>(null);
                    break;
                case 0:
                    await context.PostAsync(TextProvider.Provide(TextCategory.GETCARDLIMIT_ZeroCard));
                    context.Done<object>(null);
                    break;
                default:
                    await context.PostAsync(TextProvider.Provide(TextCategory.GETCARDLIMIT_MoreThanOneCard));
                    context.Call(new SelectCardDialog(), ResumeAfterSelectCardDialogAsync);
                    break;
            }
        }

        private async Task PostLimitInformationAsync(IDialogContext context, int cardId)
        {
            DebitCard card = _clientDebitCards.Single(x => x.DebitCardId == cardId);
            string cardTypeName = card.DebitCardType.Type;
            int withdrawLimit = card.DailyCashWithdrawalLimit ?? 0;
            int purchaseLimit = card.DailyPaymentLimit ?? 0;
            string currency = card.Account.Currency;

            await context.PostAsync(TextProvider.Provide(TextCategory.GETCARDLIMIT_WithdrawalLimit) + withdrawLimit +
                                    " " + currency);
            await context.PostAsync(TextProvider.Provide(TextCategory.GETCARDLIMIT_PurchaseLimit) + purchaseLimit +
                                    " " + currency);
        }

        private async Task ResumeAfterSelectCardDialogAsync(IDialogContext context, IAwaitable<int> result)
        {
            int cardId = await result;
            await PostLimitInformationAsync(context, cardId);
            context.Done<object>(null);
        }
    }
}