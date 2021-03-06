using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingChatBot.DAL.EntityFramework.Model
{
    [Serializable]
    public partial class DebitCard
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DebitCardId { get; set; }

        [Required]
        [StringLength(50)]
        public string CardNumber { get; set; }

        public int AccountId { get; set; }

        public int DebitCardTypeId { get; set; }

        public int? AvailableBalance { get; set; }

        public int? DailyPaymentLimit { get; set; }

        public int? DailyCashWithdrawalLimit { get; set; }

        [StringLength(100)]
        public string NameOnCard { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string SecurityCodeHash { get; set; }

        public int? ClientId { get; set; }

        public virtual Account Account { get; set; }

        public virtual Client Client { get; set; }

        public virtual DebitCardType DebitCardType { get; set; }
    }
}
