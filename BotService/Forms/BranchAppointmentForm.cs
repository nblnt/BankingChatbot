using System;
using System.Web.Http.Description;
using BankingChatbot.Commons.Enum;
using BankingChatbot.TextStorage;
using BankingChatBot.DAL.EntityFramework;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;

namespace BotService.Forms
{
    [Serializable]
    public class BranchAppointmentForm
    {
        //{||} jel az enumok megjelenítésére alkalmas, ha promptot használunk
        [Prompt("In what kind of case do you need help? {||}")] 
        public AppointmentCaseOptions? Case;

        [Prompt("In which week do you want to attend? {||}")]
        public BookTerm? Term;

        [Prompt("Which day do you want to attend? {||}")]
        public WorkDays? Days;

        [Numeric(9, 16)]
        [Prompt("Which hour do you want to attend? (9-16)")]
        public int? Hour;

        public static IForm<BranchAppointmentForm> BuildForm()
        {
            return new FormBuilder<BranchAppointmentForm>()
                .Message(TextProvider.Provide(
                    TextCategory.BRANCHAPPOINTMENT_Welcome)) 
                .Build();
        }
    }
}