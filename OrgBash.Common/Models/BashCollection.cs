﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OrgBash.Common.Models
{
    [DataContract]
    public class BashCollection
    {
        public BashCollection()
        {
            Contents = new BashDatas();
            Statistic = new BashStatistic();
        }

        [DataMember(Name = "Inhalte")]
        public BashDatas Contents { get; set; }

        [DataMember(Name = "Statistik")]
        public BashStatistic Statistic { get; set; }
    }
}
