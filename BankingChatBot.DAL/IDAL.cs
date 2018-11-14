using System;
using System.Collections.Generic;
using BankingChatbot.Commons.Enum;
using BankingChatBot.DAL.EntityFramework.Model;

namespace BankingChatBot.DAL
{
    public interface IDAL
    {
        List<Account> GetClientAccounts(int clientId);

        DebitCard GetDebitCard(int cardId);

        List<DebitCard> GetClientDebitCards(int clientId);

        void UpdateCardLimit(int cardId, CardLimitType limitType, int newLimit);

        string GetIsoCurrency(int cardId);

        List<Branch> GetBranches();

        Branch GetBranch(int branchId);

        void InsertAppointmentBooking(int branchId, int clientId, int caseType, DateTime dateTime);
    }
}