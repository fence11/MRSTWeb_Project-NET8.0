using System.Threading.Tasks;

namespace BigBox_v4.Domain
{
    public interface IWebSessionRepository : IRepository<WebSession>
    {
        Task<WebSession?> GetByHashAsync(string hash);
        Task RemoveByHashAsync(string hash);
        Task RemoveUserSessionsAsync(int userId);
        Task RemoveExpiredSessionsAsync();
    }
}
