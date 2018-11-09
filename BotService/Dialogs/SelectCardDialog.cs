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
    public class SelectCardDialog : DialogBase<int>
    {

        public override async Task StartAsync(IDialogContext context)
        {
            List<DebitCard> clientDebitCards = DAL.GetClientDebitCards(1);

            IMessageActivity selectCardReply = context.MakeMessage();
            //itt állítjuk be, hogy görgethető legyen
            selectCardReply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            selectCardReply.Text = TextProvider.Provide(TextCategory.SELECTCARD_PleaseSelect);
            selectCardReply.Attachments = new List<Attachment>();

            foreach (DebitCard card in clientDebitCards)
            {
                List<CardImage> cardImages = new List<CardImage>();
                List<CardAction> cardButtons = new List<CardAction>();

                cardImages.Add(new CardImage(
                    "https://www.mastercard.co.uk/en-gb/businesses/mid-large/travel-expense-solutions/cards/corporate/_jcr_content/contentpar/herolight/image.adaptive.479.high.png/1472151978867.png"));

                cardButtons.Add(new CardAction(ActionTypes.PostBack, "Select", value: card.DebitCardId.ToString()));

                HeroCard hc = new HeroCard()
                {
                    Images = cardImages,
                    Title = card.DebitCardType.Type,
                    Subtitle = card.CardNumber,
                    Text = "Expiration: " + (card.ExpirationDate != null ? card.ExpirationDate.Value.ToString("MM'/'yy") : "UNKNOWN"),
                    Buttons = cardButtons
                };
                selectCardReply.Attachments.Add(hc.ToAttachment());
            }

            await context.PostAsync(selectCardReply);
            context.Wait(CardIdReceivedAsync);
        }

        private async Task CardIdReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            IMessageActivity cardIdAsString = await result; 
            if (int.TryParse(cardIdAsString.Text, out int cardId))
            {
                //betöltjük a kiválasztott kártyát a conversationdata-ba
                context.PrivateConversationData.SetValue("selectedCardId", cardId);
                context.Done(cardId);
            }
            else
            {
                await context.PostAsync(TextProvider.Provide(TextCategory.SELECTCARD_InvalidCardIdentifier));
                await context.PostAsync(TextProvider.Provide(TextCategory.SELECTCARD_PleaseSelect));
                context.Wait(RetryAsync);
            }
        }

        private async Task RetryAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            IMessageActivity cardIdAsString = await result;
            if (int.TryParse(cardIdAsString.Text, out int cardId))
            {
                //betöltjük a kiválasztott kártyát a conversationdata-ba
                context.PrivateConversationData.SetValue("selectedCardId", cardId);
                context.Done(cardId);
            }
            else
            {
                await context.PostAsync(TextProvider.Provide(TextCategory.SELECTCARD_InvalidCardIdentifier));
                await context.PostAsync(TextProvider.Provide(TextCategory.COMMON_TooManyAttempt));
                context.Reset();
            }
        }
    }
}