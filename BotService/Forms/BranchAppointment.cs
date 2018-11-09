using System;
using BankingChatbot.Commons.Enum;
using BankingChatbot.TextStorage;
using Microsoft.Bot.Builder.FormFlow;

namespace BotService.Forms
{
    [Serializable]
    public class BranchAppointment
    {
        [Prompt("In what kind of case do you need help? {||}")] //{||} jel az enumok megjelenítésére alkalmas, ha promptot használunk
        public AppointmentCaseOptions? Case;

        [Prompt("In which term do you want to attend? {||}")]
        public BookTerm? Term;

        [Prompt("Which day do you want to attend? {||}")]
        public WorkDays? Days;

        [Prompt("Which hour do you want to attend? (9-16)")]
        public int? Hour;

        public static IForm<BranchAppointment> BuildForm()
        {
            return new FormBuilder<BranchAppointment>()
                .Message(TextProvider.Provide(TextCategory.BRANCHAPPOINTMENT_Welcome)).Build();
        }
    }
}