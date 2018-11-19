using System;
using System.Threading.Tasks;
using BankingChatBot.DAL;
using BotService.Properties;
using Microsoft.Bot.Builder.Dialogs;

namespace BotService.Dialogs
{
    [Serializable]
    public abstract class DialogBase<T> : IDialog<T>
    {
        protected IDAL DAL { get; }

        protected DialogBase()
        {
            DAL = Settings.Default.UseEntityFramework 
                ? new BankingChatBot.DAL.EntityFramework.DAL() 
                : null;
        }

        public abstract Task StartAsync(IDialogContext context);
    }
}