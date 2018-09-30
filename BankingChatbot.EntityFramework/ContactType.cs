namespace BankingChatbot.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ContactType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ContactTypeId { get; set; }

        [Column("ContactType")]
        [Required]
        [StringLength(50)]
        public string ContactType1 { get; set; }

        public virtual ClientContact ClientContact { get; set; }
    }
}
