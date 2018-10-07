namespace BankingChatbot.EntityFramework
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class DebitCardType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DebitCardType()
        {
            DebitCards = new HashSet<DebitCard>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DebitCardTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        public bool Contactless { get; set; }

        public bool Embossed { get; set; }

        public bool WebCard { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DebitCard> DebitCards { get; set; }
    }
}
