using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotService.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            Activity message = await result as Activity;
            if (message.Text.ToLower().Contains("balance"))
            {
                await context.Forward(new AccountBalanceDialog(), ResumeAfterChildDialogAsync, message);
            }

            context.Wait(MessageReceivedAsync);
        }

        private async Task ResumeAfterChildDialogAsync(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceivedAsync);
        }
    }
}