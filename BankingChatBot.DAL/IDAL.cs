using System.Collections.Generic;
using BankingChatBot.DAL.EntityFramework.Model;

namespace BankingChatBot.DAL
{
    public interface IDAL
    {
        List<DebitCard> GetClientDebitCards(int clientId);
    }
}