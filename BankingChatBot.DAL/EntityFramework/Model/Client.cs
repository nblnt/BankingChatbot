using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankingChatBot.DAL.EntityFramework.Model
{
    [Serializable]
    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            Accounts = new HashSet<Account>();
            DebitCards = new HashSet<DebitCard>();
        }

        public int ClientId { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        [StringLength(100)]
        public string BirthPlace { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account> Accounts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DebitCard> DebitCards { get; set; }
    }
}
