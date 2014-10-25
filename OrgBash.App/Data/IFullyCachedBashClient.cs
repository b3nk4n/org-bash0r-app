using OrgBash.Common.Data;
using OrgBash.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgBash.App.Data
{
    public interface IFullyCachedBashClient : ICachedBashClient
    {
        Task<BashCollection> GetQueryAsync(string term, bool forceReload);
    }
}
