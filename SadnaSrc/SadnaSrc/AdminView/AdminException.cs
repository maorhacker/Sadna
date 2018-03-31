﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadnaSrc.Main;

namespace SadnaSrc.AdminView
{
    class AdminException : MarketException
    {
        private static int _systemID = -1;

        public AdminException(MarketError error, string message) : base(error, message) { }
        public AdminException(RemoveUserStatus status, string message) : base((int)status, message)
        {
        }

        public AdminException(ViewPurchaseHistoryStatus status, string message) : base((int)status, message)
        {
        }


        public static void SetUser(int systemID)
        {
            _systemID = systemID;
        }

        protected override string GetModuleName()
        {
            return "AdminView";
        }

        protected override string WrapErrorMessageForDb(string message)
        {
            return "System Admin " + _systemID + " Error: " + message;
        }
    }
}
