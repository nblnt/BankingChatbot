using System;
using System.ComponentModel.DataAnnotations;

namespace BankingChatBot.DAL.EntityFramework.Model
{
    [Serializable]
    public partial class BookedAppointment
    {
        [Key]
        public int BookId { get; set; }

        public int? ClientId { get; set; }

        public int? BranchId { get; set; }

        public int? CaseType { get; set; }

        public DateTime? Date { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual Client Client { get; set; }
    }
}
