using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingChatBot.DAL.EntityFramework.Model
{
    [Serializable]
    public partial class ContactType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContactType()
        {
            ClientContacts = new HashSet<ClientContact>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ContactTypeId { get; set; }

        [Column("ContactType")]
        [Required]
        [StringLength(50)]
        public string ContactType1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientContact> ClientContacts { get; set; }
    }
}
