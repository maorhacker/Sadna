﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadnaSrc.Main;

namespace SadnaSrc.SupplyPoint
{
    class SupplyAnswer : MarketAnswer
    {
        public SupplyAnswer(SupplyStatus status, string answer) : base((int)status, answer)
        {

        }
    }
}
