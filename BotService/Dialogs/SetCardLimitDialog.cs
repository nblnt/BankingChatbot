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
    public class SetCardLimitDialog : DialogBase<CardLimitModificationResult>
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

        public SetCardLimitDialog(int clientId)
        {
            _options = Options.OnlyClientSelected;
            _clientId = clientId;
        }

        public SetCardLimitDialog(int clientId, int selectedCardId)
        {
            _options = Options.ClientAndCardSelected;
            _clientId = clientId;
            _selectedCardId = selectedCardId;
        }

        public SetCardLimitDialog(int clientId, CardLimitType cardLimitType)
        {
            _options = Options.ClientAndLimitTypeSelected;
            _clientId = clientId;
            _cardLimitType = cardLimitType;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            //context.Wait(MessageReceivedAsync);
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
            //await context.PostAsync("Set card limit dialog");
        }

        private async Task ResumeAfterSelectCardDialogAsync(IDialogContext context, IAwaitable<int> result)
        {
            _selectedCardId = await result;
            await context.PostAsync($"cardid : {_selectedCardId}");
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
            context.Done<CardLimitModificationResult>(null);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Done<CardLimitModificationResult>(null);//todo: ne maradjon így
        }
    }
}