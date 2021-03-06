using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankingChatBot.DAL.EntityFramework.Model
{
    [Serializable]
    public partial class Currency
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Currency()
        {
            Accounts = new HashSet<Account>();
        }

        [Key]
        [StringLength(3)]
        public string ISO { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
