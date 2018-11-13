using System;
using System.Data.Entity;
using BankingChatBot.DAL.EntityFramework.Model;

namespace BankingChatBot.DAL.EntityFramework
{
    [Serializable]
    public partial class BankingChatbotDataContext : DbContext
    {
        public BankingChatbotDataContext()
            : base("name=BankingChatbotDataContext")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<BookedAppointment> BookedAppointments { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<ClientContact> ClientContacts { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ContactType> ContactTypes { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<DebitCard> DebitCards { get; set; }
        public virtual DbSet<DebitCardType> DebitCardTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.Currency)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .HasOptional(e => e.DebitCard)
                .WithRequired(e => e.Account);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.Accounts)
                .WithRequired(e => e.Client)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Client)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContactType>()
                .HasOptional(e => e.ClientContact)
                .WithRequired(e => e.ContactType);

            modelBuilder.Entity<Currency>()
                .Property(e => e.ISO)
                .IsUnicode(false);

            modelBuilder.Entity<Currency>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Currency>()
                .HasMany(e => e.Accounts)
                .WithOptional(e => e.Currency1)
                .HasForeignKey(e => e.Currency);

            modelBuilder.Entity<DebitCardType>()
                .HasMany(e => e.DebitCards)
                .WithRequired(e => e.DebitCardType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);
        }
    }
}
