﻿using System.Threading.Tasks;
using BankingChatbot.Commons.Util;
using BankingChatbot.TextStorage;
using Microsoft.Bot.Builder.Dialogs;

namespace BotService.Dialogs
{
    public partial class IntentDialog
    {
        private async Task ResumeAfterChildDialogAsync(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync(TextProvider.Provide(TextCategory.COMMON_HelpMore));
            context.Wait(MessageReceived);
        }

        private async Task ResumeAfterGetCardLimitDialogAsync(IDialogContext context, IAwaitable<object> result)
        {
            await result;
            PromptDialog.Confirm(context, ResumeAfterSetLimitConfirmDialogAsync,
                TextProvider.Provide(TextCategory.SETCARDLIMIT_WishToChange),
                TextProvider.Provide(TextCategory.COMMON_NotUnderstandable));
        }

        private async Task ResumeAfterSetLimitConfirmDialogAsync(IDialogContext context, IAwaitable<bool> result)
        {
            try
            {
                bool changeSettings = await result;
                if (changeSettings)
                {
                    context.Call(
                        new SetCardLimitInitializationDialog(1,
                            context.PrivateConversationData.GetValue<int>("selectedCardId")),
                        ResumeAfterSetCardLimitInitializationDialogAsync);
                }
                else
                {
                    await context.PostAsync(TextProvider.Provide(TextCategory.SETCARDLIMIT_WontChange));
                }
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync(TextProvider.Provide(TextCategory.SETCARDLIMIT_TooManyAttempt));                
            }
        }

        private async Task ResumeAfterSetCardLimitInitializationDialogAsync(IDialogContext context, IAwaitable<bool> result)
        {
            bool success = await result;
            if (success)
            {
                await context.PostAsync(TextProvider.Provide(TextCategory.SETCARDLIMIT_Success));
            }
            else
            {
                await context.PostAsync(TextProvider.Provide(TextCategory.SETCARDLIMIT_Error));
            }

            context.PrivateConversationData.RemoveValue("selectedCardId");
        }

        private async Task AskForAccurateInput(IDialogContext context)
        {
            await context.PostAsync(TextProvider.Provide(TextCategory.COMMON_AskingMoreAccurateInput));
        }
    }
}