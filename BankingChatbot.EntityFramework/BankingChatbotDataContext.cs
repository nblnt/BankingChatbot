namespace BankingChatbot.EntityFramework
{
    using System.Data.Entity;

    public partial class BankingChatbotDataContext : DbContext
    {
        public BankingChatbotDataContext()
            : base("name=BankingChatbotDataContext")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<ClientContact> ClientContacts { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ContactType> ContactTypes { get; set; }
        public virtual DbSet<DebitCard> DebitCards { get; set; }
        public virtual DbSet<DebitCardType> DebitCardTypes { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasOptional(e => e.DebitCard)
                .WithRequired(e => e.Account);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.Accounts)
                .WithRequired(e => e.Client)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContactType>()
                .HasOptional(e => e.ClientContact)
                .WithRequired(e => e.ContactType);

            modelBuilder.Entity<DebitCardType>()
                .HasMany(e => e.DebitCards)
                .WithRequired(e => e.DebitCardType)
                .WillCascadeOnDelete(false);
        }
    }
}
