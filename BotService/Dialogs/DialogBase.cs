using System;
using System.Threading.Tasks;
using BankingChatBot.DAL;
using BankingChatBot.DAL.EntityFramework;
using BotService.Properties;
using Microsoft.Bot.Builder.Dialogs;

namespace BotService.Dialogs
{
    [Serializable]
    public abstract class DialogBase<T> : IDialog<T>
    {
        protected IDAL DAL;

        protected DialogBase()
        {
            DAL = Settings.Default.UseEntityFramework ? new BankingChatBot.DAL.EntityFramework.DAL() : null;
        }

        public abstract Task StartAsync(IDialogContext context);
    }
}