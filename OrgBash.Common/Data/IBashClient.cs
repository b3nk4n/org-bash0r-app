using OrgBash.Common.Models;
using System.Threading.Tasks;

namespace OrgBash.Common.Data
{
    public interface IBashClient
    {
        Task<BashCollection> GetQuotesAsync(string order);

        Task<BashCollection> GetQueryAsync(string term);

        Task<bool> RateAsync(int id, string type, string linkParam);
    }
}
