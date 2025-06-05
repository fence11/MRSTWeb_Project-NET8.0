using System;

namespace BigBox_v4.Domain
{
    public class WebSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SessionHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddHours(1);
        public User? User { get; set; }
    }
}
