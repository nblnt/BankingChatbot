using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BankingChatbot.Commons.Enum;
using BankingChatBot.DAL.EntityFramework.Model;

namespace BankingChatBot.DAL.EntityFramework
{
    [Serializable]
    public class DAL : IDAL
    {
        public List<Account> GetClientAccounts(int clientId)
        {
            using (BankingChatbotDataContext db
                = new BankingChatbotDataContext())
            {
                return db.Accounts
                    .Where(x => x.ClientId == clientId)
                    .ToList();
            }
        }        

        public DebitCard GetDebitCard(int cardId)
        {
            using (BankingChatbotDataContext db
                = new BankingChatbotDataContext())
            {
                return db.DebitCards
                    .Single(x => x.DebitCardId == cardId);
            }
        }        

        public List<DebitCard> GetClientDebitCards(int clientId)
        {
            using (BankingChatbotDataContext db = new BankingChatbotDataContext())
            {
                return db.DebitCards
                    .Where(x => x.ClientId == clientId)
                    .Include(x => x.Account)
                    .Include(x => x.DebitCardType)
                    .ToList();
            }
        }

        public void UpdateCardLimit(int cardId, CardLimitType limitType, int newLimit)
        {
            using (BankingChatbotDataContext db = new BankingChatbotDataContext())
            {
                DebitCard selectedCard = db.DebitCards.Single(x => x.DebitCardId == cardId);
                switch (limitType)
                {
                    case CardLimitType.PurchaseLimit:
                        selectedCard.DailyPaymentLimit = newLimit;
                        break;
                    case CardLimitType.CashWithdrawalLimit:
                        selectedCard.DailyCashWithdrawalLimit = newLimit;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                db.SaveChanges();
            }
        }

        public string GetIsoCurrency(int cardId)
        {
            using (BankingChatbotDataContext db = new BankingChatbotDataContext())
            {
                return db.DebitCards
                    .Where(x => x.DebitCardId == cardId)
                    .Select(x => x.Account.Currency)
                    .Single();
            }
        }

        public List<Branch> GetBranches()
        {
            using (BankingChatbotDataContext db = new BankingChatbotDataContext())
            {
                return db.Branches.ToList();
            }
        }

        public Branch GetBranch(int branchId)
        {
            using (BankingChatbotDataContext db = new BankingChatbotDataContext())
            {
                return db.Branches.Single(x => x.BranchId == branchId);
            }
        }

        public void InsertAppointmentBooking(int branchId, int clientId, int caseType, DateTime dateTime)
        {
            using (BankingChatbotDataContext db = new BankingChatbotDataContext())
            {
                BookedAppointment appointment = new BookedAppointment()
                {
                    BranchId =  branchId,
                    CaseType = caseType,
                    ClientId = clientId,
                    Date = dateTime
                };
                db.BookedAppointments.Add(appointment);
                db.SaveChanges();
            }
        }
    }
}