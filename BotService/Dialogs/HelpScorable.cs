using System;
using System.Threading;
using System.Threading.Tasks;
using BankingChatbot.TextStorage;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;

namespace BotService.Dialogs
{
    public class HelpScorable : ScorableBase<IActivity, string, double>
    {
        private readonly IDialogTask task;

        public HelpScorable(IDialogTask task)
        {
            SetField.NotNull(out this.task, nameof(task), task);
        }

        protected override async Task<string> PrepareAsync(IActivity item, CancellationToken token)
        {
            IMessageActivity message = item as IMessageActivity;
            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                if (message.Text.Equals("help", StringComparison.InvariantCultureIgnoreCase))
                {
                    return message.Text;
                }
            }

            return null;
        }

        protected override bool HasScore(IActivity item, string state)
        {
            return state != null;
        }

        protected override double GetScore(IActivity item, string state)
        {
            return 1;
        }

        protected override async Task PostAsync(IActivity item, string state, CancellationToken token)
        {
            InterruptionDialog helpInterruptionDialog = new InterruptionDialog(TextProvider.Provide(TextCategory.INTERRUPTION_Help));
            //todo: erről olvass majd
            //valószínűleg ez egy olyan hívás, ami ignorálja hívott dialog eredményét, és folytatja itt a futást
            IDialog<IMessageActivity> helpInterruption = helpInterruptionDialog.Void<object, IMessageActivity>();
            task.Call(helpInterruption, null);
            //await task.PollAsync(token);
        }

        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}