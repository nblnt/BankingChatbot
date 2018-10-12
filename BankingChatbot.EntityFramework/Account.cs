namespace BankingChatbot.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    public partial class Account
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccountId { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountNumber { get; set; }

        public int ClientId { get; set; }

        public int Balance { get; set; }

        [StringLength(3)]
        public string Currency { get; set; }

        public virtual Currency Currency1 { get; set; }

        public virtual Client Client { get; set; }

        public virtual DebitCard DebitCard { get; set; }
    }
}
