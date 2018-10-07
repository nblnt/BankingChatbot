using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingChatbot.EntityFramework;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotService.Dialogs
{
    [Serializable]
    public class AccountBalanceDialog : IDialog<object>
    {
        private List<Account> userAccounts;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            using (var dbContext = new BankingChatbotDataContext())
            {
                userAccounts = dbContext.Accounts.Where(x => x.ClientId == 1).ToList();
                if (userAccounts.Count == 1)
                {
                    int balance = userAccounts[0].Balance;
                    await WriteOutBalanceAsync(context, balance);
                }
            }

            if (userAccounts.Count > 1)
            {
                ShowUserAccountsOptions(context);                
            }
        }

        private void ShowUserAccountsOptions(IDialogContext context)
        {            
            PromptDialog.Choice(context, OnOptionSelectedAsync, userAccounts.Select(x => x.AccountNumber).ToList(),
                "You have more than one accounts! Please choose one!", "Not valid option!");
        }

        private async Task OnOptionSelectedAsync(IDialogContext context, IAwaitable<string> result)
        {
            //bug: rögtön lefut a metódus, nem csak választás után
            string selectedAccount = await result;
            int balance = userAccounts.Where(x => x.AccountNumber == selectedAccount).Select(x => x.Balance).Single();
            await context.PostAsync($"Your balance is : {balance}");

        }

        private async Task ResumeAfterFooDialog(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task WriteOutBalanceAsync(IDialogContext context, int balance)
        {
            await context.PostAsync($"Your balance is : {balance}");
        }

        
    }
}