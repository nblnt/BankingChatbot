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

        private Account selectedAccount;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            using (var dbContext = new BankingChatbotDataContext())
            {
                userAccounts = dbContext.Accounts.Where(x => x.ClientId == 1).ToList();
            }

            if (userAccounts.Count > 1)
            {
                ShowUserAccountsOptions(context);                
            }
            else if (userAccounts.Count == 1)
            {
                selectedAccount = userAccounts.Single();
                await WriteOutBalanceAsync(context, selectedAccount);
                context.Done(true);
            }
            else
            {
                await context.PostAsync("You don't have any account to inspect!");
                context.Done(true);
            }
        }

        private void ShowUserAccountsOptions(IDialogContext context)
        {            
            PromptDialog.Choice(context, this.OnOptionSelectedAsync, userAccounts.Select(x => x.AccountNumber).ToList(),
                "You have more than one accounts! Please choose one!", "Not valid option!");
        }

        private async Task OnOptionSelectedAsync(IDialogContext context, IAwaitable<string> result)
        {
            var selectedAccountNumber = await result;
            Account selectedAccount = userAccounts.Single(x => x.AccountNumber == selectedAccountNumber);
            await WriteOutBalanceAsync(context, selectedAccount);
            context.Done(true);
        }

        private async Task WriteOutBalanceAsync(IDialogContext context, Account account)
        {
            await context.PostAsync($"Your balance is : {account.Balance} {account.Currency}");
        }

        
    }
}