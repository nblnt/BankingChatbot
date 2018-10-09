namespace BankingChatbot.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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

        public virtual Account Account { get; set; }

        public virtual DebitCardType DebitCardType { get; set; }
    }
}
