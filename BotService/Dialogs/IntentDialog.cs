using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingChatbot.Commons.Enum;
using BankingChatbot.TextStorage;
using BotService.Properties;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace BotService.Dialogs
{
    [Serializable]
    [LuisModel("86814b35-602d-4b50-91c1-280144f8e9b5",
        "5754ccecd67d462a95192fbce4209492")]
    public partial class IntentDialog : LuisDialog<object>
    {
        private int _clientId = Properties.Settings.Default.MockClientId;

        [LuisIntent("Greeting")]
        public async Task GreetAsync(IDialogContext context, LuisResult result)
        {
            if (CheckMinimumIntentScore(result.TopScoringIntent.Score))
            {
                await context.PostAsync(TextProvider.Provide(TextCategory.GREETING));
            }
            else
            {
                await AskForAccurateInput(context);
            }
        }

        [LuisIntent("GetAccountBalance")]
        public async Task BalanceAsync(IDialogContext context, LuisResult result)
        {
            if (CheckMinimumIntentScore(result.TopScoringIntent.Score))
            {
                Activity message = new Activity(text: result.Query);
                await context.Forward(new AccountBalanceDialog(_clientId),
                    ResumeAfterChildDialogAsync, message);
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
                Activity message = new Activity(text: result.Query);
                await context.Forward(new GetCardLimitDialog(_clientId), ResumeAfterGetCardLimitDialogAsync, message);
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
                if (result.Entities.Any())
                {
                    IEnumerable<string> limitType = result.Entities.Select(x => x.Entity);
                    context.Call(
                        limitType.Any(x => x.Contains("withdrawal"))
                            ? new SetCardLimitInitializationDialog(_clientId, CardLimitType.CashWithdrawalLimit)
                            : new SetCardLimitInitializationDialog(_clientId, CardLimitType.PurchaseLimit),
                        ResumeAfterSetCardLimitInitializationDialogAsync);
                }
                else
                {
                    context.Call(new SetCardLimitInitializationDialog(_clientId), ResumeAfterSetCardLimitInitializationDialogAsync);
                }
            }
            else
            {
                await AskForAccurateInput(context);
            }
        }

        [LuisIntent("SearchBranch")]
        public async Task SearchBranchAsync(IDialogContext context, LuisResult result)
        {
            if (CheckMinimumIntentScore(result.TopScoringIntent.Score))
            {
                context.Call(new SearchBranchDialog(), ResumeAfterSearchBranchDialogAsync);
            }
            else
            {
                await AskForAccurateInput(context);
            }
        }

        [LuisIntent("BookBranchAppointment")]
        public async Task BookBranchAppointmentAsync(IDialogContext context, LuisResult result)
        {
            if (CheckMinimumIntentScore(result.TopScoringIntent.Score))
            {
                context.Call(new SearchBranchDialog(), ResumeAfterSearchBranchDialogForBookingAsync);
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