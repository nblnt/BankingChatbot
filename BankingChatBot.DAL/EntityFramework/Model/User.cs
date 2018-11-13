using System;
using System.ComponentModel.DataAnnotations;

namespace BankingChatBot.DAL.EntityFramework.Model
{
    [Serializable]
    public partial class User
    {
        public int UserId { get; set; }

        public int ClientId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public virtual Client Client { get; set; }
    }
}
