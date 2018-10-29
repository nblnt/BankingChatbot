using System.Collections.Generic;
using BankingChatbot.Commons.Enum;
using BankingChatBot.DAL.EntityFramework.Model;

namespace BankingChatBot.DAL
{
    public interface IDAL
    {
        DebitCard GetDebitCard(int cardId);

        List<DebitCard> GetClientDebitCards(int clientId);

        void UpdateCardLimit(int cardId, CardLimitType limitType, int newLimit);

        string GetIsoCurrency(int cardId);
    }
}