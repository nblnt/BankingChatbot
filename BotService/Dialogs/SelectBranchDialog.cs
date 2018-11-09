using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingChatbot.TextStorage;
using BankingChatBot.DAL.EntityFramework.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotService.Dialogs
{
    [Serializable]
    public class SelectBranchDialog : DialogBase<int>
    {
        public override async Task StartAsync(IDialogContext context)
        {
            List<Branch> branches = DAL.GetBranches();

            IMessageActivity selectBranchReply = context.MakeMessage();
            selectBranchReply.AttachmentLayout = AttachmentLayoutTypes.List;
            selectBranchReply.Text = TextProvider.Provide(TextCategory.SELECTBRANCH_PleaseSelect);
            selectBranchReply.Attachments = new List<Attachment>();

            foreach (Branch branch in branches)
            {
                List<CardImage> cardImages = new List<CardImage>();
                cardImages.Add(new CardImage("https://icla.org/wp-content/uploads/2018/02/blue-location-icon-png-19.png"));

                ThumbnailCard thumbnailCard = new ThumbnailCard()
                {
                    Images = cardImages,
                    Title = branch.BranchName,
                    Subtitle = $"{branch.ZipCode} {branch.City}, {branch.Street} {branch.HouseNumber}.",
                    Text = $"Open on weekdays: {branch.WeekDayOpeningHours}",
                    Tap = new CardAction(ActionTypes.PostBack, value: branch.BranchId.ToString())
                };

                selectBranchReply.Attachments.Add(thumbnailCard.ToAttachment());
            }

            await context.PostAsync(selectBranchReply);
            context.Wait(BranchIdReceivedAsync);
        }

        private async Task BranchIdReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            IMessageActivity branchIdAsString = await result;
            if (int.TryParse(branchIdAsString.Text, out int branchId))
            {
                context.PrivateConversationData.SetValue("branchId", branchId);
                context.Done(branchId);
            }
            else
            {
                await context.PostAsync(TextProvider.Provide(TextCategory.SELECTBRANCH_InvalidBranchIdentifier));
                await context.PostAsync(TextProvider.Provide(TextCategory.SELECTBRANCH_PleaseSelect));
                context.Wait(RetryAsync);
            }
        }

        private async Task RetryAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            IMessageActivity branchIdAsString = await result;
            if (int.TryParse(branchIdAsString.Text, out int branchId))
            {
                context.PrivateConversationData.SetValue("branchId", branchId);
                context.Done(branchId);
            }
            else
            {
                await context.PostAsync(TextProvider.Provide(TextCategory.SELECTBRANCH_InvalidBranchIdentifier));
                await context.PostAsync(TextProvider.Provide(TextCategory.SETCARDLIMIT_TooManyAttempt));
                context.Reset();
            }
        }
    }
}