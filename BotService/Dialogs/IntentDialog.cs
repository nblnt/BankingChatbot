using System;
using System.Threading.Tasks;
using BankingChatbot.Commons.Enum;
using BotService.Properties;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace BotService.Dialogs
{
    [Serializable]
    [LuisModel("86814b35-602d-4b50-91c1-280144f8e9b5", "5754ccecd67d462a95192fbce4209492")]
    public class IntentDialog : LuisDialog<object>
    {
        [LuisIntent("Greeting")]
        public async Task GreetAsync(IDialogContext context, LuisResult result)
        {
            if (CheckMinimumIntentScore(result.TopScoringIntent.Score))
            {
                await context.PostAsync("Hi there");                
            }
            else
            {
                await AnswerNoneAsync(context);
            }
        }

        [LuisIntent("GetAccountBalance")]
        public async Task BalanceAsync(IDialogContext context, LuisResult result)
        {
            if (CheckMinimumIntentScore(result.TopScoringIntent.Score))
            {
                var message = new Activity(text: result.Query);
                await context.Forward(new AccountBalanceDialog(), ResumeAfterChildDialogAsync, message);
                context.Done(true);
            }
            else
            {
                await AnswerNoneAsync(context);
            }
        }

        [LuisIntent("GetCardLimit")]
        public async Task GetCardLimitAsync(IDialogContext context, LuisResult result)
        {
            if (CheckMinimumIntentScore(result.TopScoringIntent.Score))
            {
                var message = new Activity(text: result.Query);
                await context.Forward(new GetCardLimitDialog(1)/*todo: statikus userid-t majd töröld*/, ResumeAfterChildDialogAsync, message);
                context.Done(true);
            }
            else
            {
                await AnswerNoneAsync(context);
            }
        }

        [LuisIntent("SetCardLimit")]
        public async Task SetCardLimitAsync(IDialogContext context, LuisResult result)
        {
            throw new NotImplementedException();
        }

        [LuisIntent("None")]
        public async Task NoneAsync(IDialogContext context, LuisResult result)
        {
            await AnswerNoneAsync(context);
        }

        private async Task AnswerNoneAsync(IDialogContext context)
        {
            await context.PostAsync("I don't get it!");
            await context.PostAsync("Could you be more accurate please?");
        }

        private async Task ResumeAfterChildDialogAsync(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);
        }

        private bool CheckMinimumIntentScore(double? score)
        {
            if (score != null)
            {
                return score >= Settings.Default.LuisIntentScoreMinimum;
            }
            return false;
        }
    }
}