using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingChatBot.DAL.EntityFramework.Model
{
    [Serializable]
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
