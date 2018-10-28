using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingChatbot.TextStorage;
using BankingChatBot.DAL.EntityFramework;
using BankingChatBot.DAL.EntityFramework.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotService.Dialogs
{
    [Serializable]
    public class AccountBalanceDialog : DialogBase<object>
    {
        private List<Account> userAccounts;

        private Account selectedAccount;

        public override async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            using (BankingChatbotDataContext dbContext = new BankingChatbotDataContext())
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
                await context.PostAsync(TextProvider.Provide(TextCategory.ACCOUNTBALANCE_ZeroAccount));
                context.Done(true);
            }
        }

        private void ShowUserAccountsOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, OnOptionSelectedAsync, userAccounts.Select(x => x.AccountNumber).ToList(),
                TextProvider.Provide(TextCategory.ACCOUNTBALANCE_MoreThanOneAccount),
                TextProvider.Provide(TextCategory.COMMON_NotValidOption));
        }

        private async Task OnOptionSelectedAsync(IDialogContext context, IAwaitable<string> result)
        {
            string selectedAccountNumber = await result;
            Account selectedAccount = userAccounts.Single(x => x.AccountNumber == selectedAccountNumber);
            await WriteOutBalanceAsync(context, selectedAccount);
            context.Done(true);
        }

        private async Task WriteOutBalanceAsync(IDialogContext context, Account account)
        {
            await context.PostAsync(TextProvider.Provide(TextCategory.ACCOUNTBALANCE_BalanceIs) +
                                    $"{account.Balance} {account.Currency}");
        }

        
    }
}