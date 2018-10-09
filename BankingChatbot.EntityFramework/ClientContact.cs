namespace BankingChatbot.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ClientContact
    {
        public int ClientContactId { get; set; }

        public int? ClientId { get; set; }

        public int ContactTypeId { get; set; }

        public string ContactText { get; set; }

        public virtual ContactType ContactType { get; set; }
    }
}
