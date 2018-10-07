namespace BankingChatbot.EntityFramework
{
    public partial class ClientContact
    {
        public int ClientContactId { get; set; }

        public int? ClientId { get; set; }

        public int ContactTypeId { get; set; }

        public string ContactText { get; set; }

        public virtual ContactType ContactType { get; set; }
    }
}
