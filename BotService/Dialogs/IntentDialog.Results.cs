using System;
using System.Threading.Tasks;
using BankingChatbot.Commons.Util;
using Microsoft.Bot.Builder.Dialogs;

namespace BotService.Dialogs
{
    public partial class IntentDialog
    {
        private async Task ResumeAfterChildDialogAsync(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Can I help you something more?");
            context.Wait(MessageReceived);
        }

        private async Task ResumeAfterGetCardLimitDialogAsync(IDialogContext context, IAwaitable<object> result)
        {
            await result;
            PromptDialog.Confirm(context, ResumeAfterSetLimitConfirmDialogAsync,
                "Do you wish to change the settings of your card?", "I don't get it!");
        }

        private async Task ResumeAfterSetLimitConfirmDialogAsync(IDialogContext context, IAwaitable<bool> result)
        {
            bool changeSettings = await result;
            if (changeSettings)
            {
                context.Call(new SetCardLimitDialog(), ResumeAfterSetCardLimitDialogAsync);
            }
            else
            {
                await context.PostAsync("Ok, your card limits won't be changed!");
            }
        }

        private async Task ResumeAfterSetCardLimitDialogAsync(IDialogContext context, IAwaitable<CardLimitModificationResult> result)
        {
            CardLimitModificationResult modificationData = await result;
        }

        private async Task AskForAccurateInput(IDialogContext context)
        {
            await context.PostAsync("I don't get it!");
            await context.PostAsync("Could you be more accurate please?");
        }
    }
}