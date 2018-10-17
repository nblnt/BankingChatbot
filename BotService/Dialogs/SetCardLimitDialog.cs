using System;
using System.Threading.Tasks;
using BankingChatbot.Commons.Util;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotService.Dialogs
{
    [Serializable]
    public class SetCardLimitDialog : DialogBase<CardLimitModificationResult>
    {
        public override async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            throw new NotImplementedException();
        }
    }
}