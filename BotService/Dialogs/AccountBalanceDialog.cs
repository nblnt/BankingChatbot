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
        private int _clientId;

        private List<Account> _clientAccounts;

        private Account _selectedAccount;

        public AccountBalanceDialog(int clientId)
        {
            _clientId = clientId;
        }

        public override async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            _clientAccounts = DAL.GetClientAccounts(_clientId);

            if (_clientAccounts.Count > 1)
            {
                ShowUserAccountsOptions(context);                
            }
            else if (_clientAccounts.Count == 1)
            {
                _selectedAccount = _clientAccounts.Single();
                await WriteOutBalanceAsync(context, _selectedAccount);
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
            PromptDialog.Choice(context, OnOptionSelectedAsync, _clientAccounts
                    .Select(x => x.AccountNumber).ToList(),
                TextProvider.Provide(TextCategory.ACCOUNTBALANCE_MoreThanOneAccount),
                TextProvider.Provide(TextCategory.COMMON_NotValidOption));
        }

        private async Task OnOptionSelectedAsync(IDialogContext context, IAwaitable<string> result)
        {
            string selectedAccountNumber = await result;
            Account selectedAccount = _clientAccounts.Single(x => x.AccountNumber == selectedAccountNumber);
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