using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace BotService.Dialogs
{
    [Serializable]
    public class InterruptionDialog : IDialog<object>
    {
        private readonly string _reply;

        public InterruptionDialog(string reply)
        {
            _reply = reply;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(_reply);
            context.Done<object>(null);
        }
    }
}