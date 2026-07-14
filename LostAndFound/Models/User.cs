using LostAndFound.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace LostAndFound.Models
{
    // Email, UserName, PasswordHash, PhoneNumber, EmailConfirmed, ...
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
        public City? City { get; set; }
        public string? AvatarBase64 { get; set; }
        public bool ShowPhone { get; set; } = true;
        public bool ShowEmail { get; set; } = true;

        public bool IsVerified { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Role Role { get; set; }
        public bool IsBanned { get; set; } = false;

        public ICollection<Item> Items { get; set; } = new List<Item>();
        public ICollection<Claim> Claims { get; set; } = new List<Claim>();
        public ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
    }
}
