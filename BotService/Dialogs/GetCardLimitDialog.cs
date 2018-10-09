﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingChatbot.Commons.Enum;
using BankingChatbot.EntityFramework;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotService.Dialogs
{
    [Serializable]
    public class GetCardLimitDialog : IDialog<object>
    {
        private readonly int? UserId;

        private List<DebitCard> UserCards;

        public GetCardLimitDialog(int? userId)
        {
            UserId = userId;
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            ;
            using (var dbContext = new BankingChatbotDataContext())
            {
                UserCards = dbContext.DebitCards.Where(x => x.Account.ClientId == UserId).ToList();
            }

            if (UserCards.Count == 1)
            {
                int cardId = UserCards.Single().DebitCardId;
                await PostLimitInformationAsync(context, cardId);
            }
            else if (UserCards.Count == 0)
            {
                await context.PostAsync("You don't have a card to inspect!");
            }
            else
            {
                await context.Forward(new SelectCardDialog(), ResumeAfterSelectCardDialogAsync, message);
            }
        }

        private async Task PostLimitInformationAsync(IDialogContext context, int cardId)
        {
            DebitCard card = UserCards.Single(x => x.DebitCardId == cardId);
            string cardTypeName = card.DebitCardType.Type;
            int withdrawLimit = card.DailyCashWithdrawalLimit ?? 0;
            int purchaseLimit = card.DailyPaymentLimit ?? 0;

            await context.PostAsync($"Your {cardTypeName} card's cash withdrawal limit is {withdrawLimit}");
            await context.PostAsync($"Your {cardTypeName} card's purchase limit is {purchaseLimit}");            
        }

        private async Task ResumeAfterSelectCardDialogAsync(IDialogContext context, IAwaitable<int> result)
        {
            int cardId = await result;
            await PostLimitInformationAsync(context, cardId);
        }
    }
}