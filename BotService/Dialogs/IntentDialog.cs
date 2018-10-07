using System;
using System.Threading.Tasks;
using BotService.Properties;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace BotService.Dialogs
{
    [LuisModel("86814b35-602d-4b50-91c1-280144f8e9b5", "5754ccecd67d462a95192fbce4209492")]
    [Serializable]
    public class IntentDialog : LuisDialog<object>
    {
        [LuisIntent("Greeting")]
        public async Task GreetAsync(IDialogContext context, LuisResult result)
        {
            if (CheckMinimumIntentScore(result.TopScoringIntent.Score))
            {
                await context.PostAsync("Hi there");
                context.Wait(MessageReceived); 
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
            }
            else
            {
                await AnswerNoneAsync(context);
            }
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
            context.Wait(MessageReceived);
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