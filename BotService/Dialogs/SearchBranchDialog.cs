using System;
using System.Threading.Tasks;
using BankingChatbot.TextStorage;
using BankingChatBot.DAL.EntityFramework.Model;
using Microsoft.Bot.Builder.Dialogs;

namespace BotService.Dialogs
{
    [Serializable]
    public class SearchBranchDialog : DialogBase<object>
    {
        public override async Task StartAsync(IDialogContext context)
        {
            context.Call(new SelectBranchDialog(), ResumeAfterSelectBranchDialogAsync);
        }

        private async Task ResumeAfterSelectBranchDialogAsync(IDialogContext context, IAwaitable<int> result)
        {
            int branchId = await result;
            Branch selectedBranch = DAL.GetBranch(branchId);
            await context.PostAsync(
                $"{TextProvider.Provide(TextCategory.SELECTBRANCH_SelectedBranch)}\nName: {selectedBranch.BranchName}\nAddress: {selectedBranch.ZipCode} {selectedBranch.City}, {selectedBranch.Street} {selectedBranch.HouseNumber}.");
            context.Done<object>(null);
        }
    }
}