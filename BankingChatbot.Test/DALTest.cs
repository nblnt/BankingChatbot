using System;
using System.Collections.Generic;
using BankingChatBot.DAL.EntityFramework;
using BankingChatBot.DAL.EntityFramework.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankingChatbot.Test
{
    [TestClass]
    public class DALTest
    {
        [TestMethod]
        public void TestGetClientDebitCards()
        {
            DAL dal = new DAL();
            List<DebitCard> cards = dal.GetClientDebitCards(1);
            if (cards.Count != 1)
            {
                throw new Exception("Collection count is not valid!");
            }
        }
    }
}
