using OrgBash.Common.Models;
using System.Threading.Tasks;

namespace OrgBash.Common.Data
{
    public interface ICachedBashClient : IBashClient
    {
        Task<BashCollection> GetQuotesAsync(string order, double lifeTimeDays, bool forceReload);

        void UpdateCache(BashCollection data);
    }
}
