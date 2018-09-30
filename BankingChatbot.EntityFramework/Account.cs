namespace BankingChatbot.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Account
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccountId { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountNumber { get; set; }

        public int ClientId { get; set; }

        public int Balance { get; set; }

        public virtual Client Client { get; set; }

        public virtual DebitCard DebitCard { get; set; }
    }
}
