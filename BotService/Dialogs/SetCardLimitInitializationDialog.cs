using System;
using System.Collections.Generic;
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
    public class SetCardLimitInitializationDialog : DialogBase<CardLimitModificationResult>
    {
        private enum Options
        {
            OnlyClientSelected,
            ClientAndCardSelected,
            ClientAndLimitTypeSelected
        }

        private Options _options;

        private readonly int _clientId;

        private List<DebitCard> _userDebitCards;

        private int? _selectedCardId;

        private CardLimitType _cardLimitType;

        public SetCardLimitInitializationDialog(int clientId)
        {
            _options = Options.OnlyClientSelected;
            _clientId = clientId;
        }

        public SetCardLimitInitializationDialog(int clientId, int selectedCardId)
        {
            _options = Options.ClientAndCardSelected;
            _clientId = clientId;
            _selectedCardId = selectedCardId;
        }

        public SetCardLimitInitializationDialog(int clientId, CardLimitType cardLimitType)
        {
            _options = Options.ClientAndLimitTypeSelected;
            _clientId = clientId;
            _cardLimitType = cardLimitType;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            _userDebitCards = DAL.GetClientDebitCards(_clientId);
            switch (_options)
            {
                case Options.OnlyClientSelected:
                    if (_userDebitCards.Count > 1)
                    {
                        context.Call(new SelectCardDialog(), ResumeAfterSelectCardDialogAsync);
                    }
                    else
                    {
                        context.Done<CardLimitModificationResult>(null);//todo: ide nem ez kell majd
                    }
                    break;
                case Options.ClientAndCardSelected:
                    context.Done<CardLimitModificationResult>(null);//todo: ide nem ez kell majd
                    break;
                case Options.ClientAndLimitTypeSelected:
                    context.Call(new SelectCardDialog(), ResumeAfterSelectCardDialogAsync);
                    break;
                default:
                    context.Done<CardLimitModificationResult>(null);
                    break;
            }
        }

        private async Task ResumeAfterSelectCardDialogAsync(IDialogContext context, IAwaitable<int> result)
        {
            _selectedCardId = await result;
            CreateLimitTypeChoice(context);
        }

        private void CreateLimitTypeChoice(IDialogContext context)
        {
            PromptDialog.Choice(context, ResumeAfterLimitTypeChoiceAsync,
                new[] { CardLimitType.PurchaseLimit, CardLimitType.CashWithdrawalLimit, CardLimitType.All },
                TextProvider.Provide(TextCategory.SETCARDLIMIT_PleaseSelectLimitType),
                TextProvider.Provide(TextCategory.COMMON_NotValidOption),
                descriptions: new[]
                {
                    TextProvider.Provide(TextCategory.SETCARDLIMIT_PurchaseLimitDesc),
                    TextProvider.Provide(TextCategory.SETCARDLIMIT_WithDrawalLimitDesc),
                    TextProvider.Provide(TextCategory.SETCARDLIMIT_BothLimitTypeDesc)
                });
        }

        private async Task ResumeAfterLimitTypeChoiceAsync(IDialogContext context, IAwaitable<CardLimitType> result)
        {
            CardLimitType limitType = await result;
            context.Call(
                new SetCardLimitDialog(
                    _selectedCardId ?? throw new NullReferenceException("The selected card identifier is invalid!"),
                    limitType), ResumeAfterSetCardLimitDialogAsync);
        }

        private async Task ResumeAfterSetCardLimitDialogAsync(IDialogContext context, IAwaitable<CardLimitModificationResult> result)
        {
            context.Done<CardLimitModificationResult>(null);//todo: ne maradjon így
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Done<CardLimitModificationResult>(null);//todo: ne maradjon így
        }
    }
}