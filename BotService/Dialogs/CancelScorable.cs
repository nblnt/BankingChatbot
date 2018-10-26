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
    public class CancelScorable : ScorableBase<IActivity, string, double> //ScorableBase<Item, State, Score>
    {
        private readonly IDialogTask task;

        public CancelScorable(IDialogTask task)
        {
            SetField.NotNull(out this.task, nameof(task), task);
        }

        //fogadja és elemzi a bejövő üzenetet, és beállítja a dialog state-et, amit majd az összes többi metódus használni fog
        protected override async Task<string> PrepareAsync(IActivity item, CancellationToken token)
        {
            IMessageActivity message = item as IMessageActivity;
            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                if (message.Text.Equals("cancel", StringComparison.InvariantCultureIgnoreCase))
                {
                    return message.Text;
                }
            }

            return null;
        }

        //eldönti, hogy kell-e a scorable dialogot a stack tetejére vinni, vagyis hogy ennek a dialognak át kell-e venni az irányítást
        protected override bool HasScore(IActivity item, string state)
        {
            return state != null;
        }

        //ez adja vissza a pontszámot (0-1), ha a hasscore true-et ad vissza
        protected override double GetScore(IActivity item, string state)
        {
            return 1;
        }

        //ez reagál a felhasználónak, amennyiben a score alapján ez a dialógus veszi át az irányítást
        protected override async Task PostAsync(IActivity item, string state, CancellationToken token)
        {
            InterruptionDialog cancelInterruptionDialog = new InterruptionDialog(TextProvider.Provide(TextCategory.INTERRUPTION_Cancel));
            IDialog<IMessageActivity> cancelInterruption = cancelInterruptionDialog.Void<object, IMessageActivity>();
            task.Call(cancelInterruption, null);
            task.Reset();
            //await task.PollAsync(token);
        }

        //ez fut le a végén, dispose-ok ajánlottak ide
        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}