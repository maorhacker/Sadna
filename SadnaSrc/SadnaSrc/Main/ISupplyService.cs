﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadnaSrc.Main
{
    public interface ISupplyService
    {

        void BreakExternal();

        void FixExternal();

    }

    public enum SupplyStatus
    {
        Success,
        SupplySystemError,
        NoSupplySystem,
        InvalidOrder
    }
}
