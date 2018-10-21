using System;
using System.Threading.Tasks;
using BankingChatbot.Commons.Enum;
using BankingChatbot.Commons.Util;
using BotService.Properties;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace BotService.Dialogs
{
    [Serializable]
    [LuisModel("86814b35-602d-4b50-91c1-280144f8e9b5", "5754ccecd67d462a95192fbce4209492")]
    public partial class IntentDialog : LuisDialog<object>
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
                await context.PostAsync("I don't get it!");
                await context.PostAsync("Could you be more accurate please?");
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
                await AskForAccurateInput(context);
            }
        }

        [LuisIntent("GetCardLimit")]
        public async Task GetCardLimitAsync(IDialogContext context, LuisResult result)
        {
            if (CheckMinimumIntentScore(result.TopScoringIntent.Score))
            {
                var message = new Activity(text: result.Query);
                await context.Forward(new GetCardLimitDialog(1)/*todo: statikus userid-t majd töröld*/, ResumeAfterGetCardLimitDialogAsync, message);
            }
            else
            {
                await AskForAccurateInput(context);
            }            
        }

        [LuisIntent("SetCardLimit")]
        public async Task SetCardLimitAsync(IDialogContext context, LuisResult result)
        {
            if (CheckMinimumIntentScore(result.TopScoringIntent.Score))
            {
                var message = new Activity(text: result.Query);
                await context.Forward(new SetCardLimitDialog() /*todo: statikus userid-t majd töröld*/, ResumeAfterSetCardLimitDialogAsync, message);
            }
            else
            {
                await AskForAccurateInput(context);
            }
        }

        [LuisIntent("None")]
        public async Task NoneAsync(IDialogContext context, LuisResult result)
        {
            await AskForAccurateInput(context);
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