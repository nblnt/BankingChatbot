using System;

namespace BankingChatBot.DAL.EntityFramework.Model
{
    [Serializable]
    public partial class ClientContact
    {
        public int ClientContactId { get; set; }

        public int? ClientId { get; set; }

        public int ContactTypeId { get; set; }

        public string ContactText { get; set; }

        public virtual Client Client { get; set; }

        public virtual ContactType ContactType { get; set; }
    }
}
