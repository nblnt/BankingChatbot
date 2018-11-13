using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingChatBot.DAL.EntityFramework.Model
{
    [Serializable]
    public partial class Branch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Branch()
        {
            BookedAppointments = new HashSet<BookedAppointment>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BranchId { get; set; }

        public string BranchName { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string Street { get; set; }

        [StringLength(10)]
        public string HouseNumber { get; set; }

        public int? ZipCode { get; set; }

        [StringLength(11)]
        public string WeekDayOpeningHours { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookedAppointment> BookedAppointments { get; set; }
    }
}
