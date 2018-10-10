using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotService.Dialogs
{
    [Serializable]
    public class SelectCardDialog : IDialog<int> //fontos: ez nem a szokványos visszatérési érték
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("You have more than one card, please select one from the list below!");
            context.Wait(MessageReceivedAsync);//bug: nem itt várja az üzenetet
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await result;
            await context.PostAsync("selectcarddialog");
            context.Done(1);//todo:delete
        }
    }
}