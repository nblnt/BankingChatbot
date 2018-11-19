using System;
using System.Threading.Tasks;
using BankingChatbot.Commons.Util;
using BankingChatbot.TextStorage;
using BankingChatBot.DAL.EntityFramework;
using BotService.Forms;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace BotService.Dialogs
{
    public partial class IntentDialog
    {
        private async Task ResumeAfterChildDialogAsync(IDialogContext context, 
            IAwaitable<object> result)
        {
            await HelpMoreAsync(context);
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

        private async Task ResumeAfterSearchBranchDialogAsync(IDialogContext context, IAwaitable<object> result)
        {
            await result;
            PromptDialog.Confirm(context, ResumeBookAppointmentBranchConfirmDialogAsync,
                TextProvider.Provide(TextCategory.BRANCHAPPOINTMENT_WishToBook),
                TextProvider.Provide(TextCategory.COMMON_NotUnderstandable));
        }

        private async Task ResumeBookAppointmentBranchConfirmDialogAsync(IDialogContext context, IAwaitable<bool> result)
        {
            try
            {
                bool appoint = await result;
                if (appoint)
                {
                    await context.Forward(FormDialog.FromForm(BranchAppointmentForm.BuildForm),
                        ResumeAfterBranchAppointmentForm, Activity.CreateMessageActivity());
                }
                else
                {
                    await HelpMoreAsync(context);
                }
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync(TextProvider.Provide(TextCategory.SETCARDLIMIT_TooManyAttempt));
            }
        }

        private async Task ResumeAfterBranchAppointmentForm(IDialogContext context, IAwaitable<BranchAppointmentForm> result)
        {
            BranchAppointmentForm appointment = await result;
            if (appointment != null)
            {
                if (context.PrivateConversationData.TryGetValue("branchId", out int branchId))
                {
                    DAL dal = new DAL();
                    //A vasárnapot átalakítjuk hetedik napra
                    int currentDayOfWeek = (int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek;
                    int remainsFromWeek = 7 - currentDayOfWeek;
                    int selectedWorkDay = (int)appointment.Days + 1;
                    DateTime bookingDate = DateTime.Now
                        .AddDays(selectedWorkDay + 7 * (int)appointment.Term + remainsFromWeek).Date.AddHours((int)appointment.Hour);
                    dal.InsertAppointmentBooking(branchId, _clientId, (int)appointment.Case, bookingDate);

                    await context.PostAsync(TextProvider.Provide(TextCategory.BRANCHAPPOINTMENT_BookingSaved));
                }
            }
            await HelpMoreAsync(context);
        }

        private async Task ResumeAfterSearchBranchDialogForBookingAsync(IDialogContext context, IAwaitable<object> result)
        {
            await context.Forward(FormDialog.FromForm(BranchAppointmentForm.BuildForm),
                ResumeAfterBranchAppointmentForm, Activity.CreateMessageActivity());
        }

        private async Task AskForAccurateInput(IDialogContext context)
        {
            await context.PostAsync(TextProvider.Provide(TextCategory.COMMON_AskingMoreAccurateInput));
        }

        private async Task HelpMoreAsync(IDialogContext context)
        {
            await context.PostAsync(
                TextProvider.Provide(TextCategory.COMMON_HelpMore));
        }
    }
}