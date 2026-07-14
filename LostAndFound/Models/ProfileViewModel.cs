using LostAndFound.Models;
using System;
using System.Collections.Generic;

namespace LostAndFound.Models
{
    public class ProfileViewModel
    {
        // بيانات المستخدم
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public string? AvatarBase64 { get; set; }
        public bool ShowPhone { get; set; }
        public bool ShowEmail { get; set; }
        public LostAndFound.Enums.City? City { get; set; }
        public DateTime CreatedAt { get; set; }

        // الـ Reports بتاعته
        public List<Item> Items { get; set; } = new List<Item>();

        // الـ Claims بتاعته
        public List<Claim> Claims { get; set; } = new List<Claim>();
        public int LostReportsCount => Items.FindAll(i => i.Status == LostAndFound.Enums.ItemType.Lost).Count;
        public int FoundReportsCount => Items.FindAll(i => i.Status == LostAndFound.Enums.ItemType.Found).Count;

        // Stats
        public int TotalReports => Items.Count;
        public int ResolvedReports => Items.FindAll(i => i.IsResolved).Count;
        public int TotalClaims => Claims.Count;
    }
}