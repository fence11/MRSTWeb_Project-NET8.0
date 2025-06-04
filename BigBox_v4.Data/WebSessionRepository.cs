using BigBox_v4.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BigBox_v4.Data
{
    public class WebSessionRepository : Repository<WebSession>, IWebSessionRepository
    {
        private readonly ApplicationDBContext _context;
        public WebSessionRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<WebSession?> GetByHashAsync(string hash)
        {
            return await _context.Set<WebSession>()
                .FirstOrDefaultAsync(s => s.SessionHash == hash);
        }

        public async Task RemoveByHashAsync(string hash)
        {
            var entity = await GetByHashAsync(hash);
            if (entity != null)
            {
                _context.WebSessions.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveUserSessionsAsync(int userId)
        {
            var sessions = _context.WebSessions.Where(s => s.UserId == userId);
            _context.WebSessions.RemoveRange(sessions);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveExpiredSessionsAsync()
        {
            var expired = _context.WebSessions.Where(s => s.ExpiresAt < DateTime.UtcNow);
            if (expired.Any())
            {
                _context.WebSessions.RemoveRange(expired);
                await _context.SaveChangesAsync();
            }
        }
    }
}
