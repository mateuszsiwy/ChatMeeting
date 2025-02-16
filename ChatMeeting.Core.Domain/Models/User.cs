using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMeeting.Core.Domain.Models
{
    public class User
    {
        public User(string username, string password)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            Username = username;
            Password = password;
        }
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
