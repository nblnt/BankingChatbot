using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingChatBot.DAL;
using BankingChatBot.DAL.EntityFramework;
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
            context.Wait(ReplyWithCardSelectorAsync);
        }

        private async Task ReplyWithCardSelectorAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await result;

            await PostCardSelectorCarousel(context);
        }

        private async Task PostCardSelectorCarousel(IDialogContext context)
        {
            List<DebitCard> clientDebitCards = DAL.GetClientDebitCards(1);
            

            IMessageActivity selectCardReply = context.MakeMessage();
            //itt állítjuk be, hogy görgethető legyen
            selectCardReply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            selectCardReply.Text = "Please select a card below!";
            selectCardReply.Attachments = new List<Attachment>();

            foreach (var card in clientDebitCards)
            {
                HeroCard hc = CreateCardSelectorHeroCard(card);
                selectCardReply.Attachments.Add(hc.ToAttachment());
            }

            await context.PostAsync(selectCardReply);
            //bug: nem vár
            context.Wait(CardIdReceivedAsync);
        }

        private async Task CardIdReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var res = await result;
            if (int.TryParse(res.Text, out int cardId))
            {
                context.Done(cardId);
            }
            else
            {
                await context.PostAsync("Not valid card identifier!");
                await PostCardSelectorCarousel(context);
            }
        }

        private HeroCard CreateCardSelectorHeroCard(DebitCard card)
        {
            List<CardImage> cardImages = new List<CardImage>();
            List<CardAction> cardButtons = new List<CardAction>();

            cardImages.Add(new CardImage(
                    "https://www.mastercard.co.uk/en-gb/businesses/mid-large/travel-expense-solutions/cards/corporate/_jcr_content/contentpar/herolight/image.adaptive.479.high.png/1472151978867.png"));

            cardButtons.Add(new CardAction(ActionTypes.PostBack, "Select", value: card.DebitCardId.ToString()));

            return new HeroCard()
            {
                Images = cardImages,
                Title = card.DebitCardType.Type,
                Subtitle = card.CardNumber,
                Text = "Expiration: " + (card.ExpirationDate != null ? card.ExpirationDate.Value.ToString("MM'/'yy") : "UNKNOWN"),
                Buttons = cardButtons
            };
        }


    }
}