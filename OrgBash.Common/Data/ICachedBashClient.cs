﻿using OrgBash.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgBash.Common.Data
{
    public interface ICachedBashClient : IBashClient
    {
        Task<BashCollection> GetQuotesAsync(string order, double lifeTimeDays, bool forceReload);

        void UpdateCache(BashCollection data);
    }
}