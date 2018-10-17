using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BankingChatBot.DAL.EntityFramework.Model;

namespace BankingChatBot.DAL.EntityFramework
{
    [Serializable]
    public class DAL : IDAL
    {
        public List<DebitCard> GetClientDebitCards(int clientId)
        {
            using (var db = new BankingChatbotDataContext())
            {
                return db.DebitCards.Where(x => x.Account.Client.ClientId == clientId).Include(x => x.Account)
                    .Include(x => x.DebitCardType).ToList();
            }
        }
    }
}