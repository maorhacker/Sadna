﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SadnaSrc.Main
{
    public interface ISystemAdminService
    {
        MarketAnswer RemoveUser(string userName);
        MarketAnswer ViewPurchaseHistoryByUser(string userName);
        MarketAnswer ViewPurchaseHistoryByStore(string storeName);
        MarketAnswer AddCategory(string categoryname);
        MarketAnswer RemoveCategory(string categoryname);
	    MarketAnswer CreatePolicy(string type, string subject, string op, string arg1, string optArg);
	    MarketAnswer ViewPolicies();
	    MarketAnswer ViewPoliciesSessions();
		MarketAnswer SavePolicy();
        MarketAnswer RemovePolicy(string type, string subject);
  
        MarketAnswer ViewLog();

        MarketAnswer ViewError();

	    MarketAnswer GetEntranceDetails();

        MarketAnswer ViewAllCategories();

    }

    public enum RemoveUserStatus
    {
        Success,
        SelfTermination,
        NotSystemAdmin,
        NoUserFound,
        NoDB = 500,
        BadInput = 600
    }

    public enum ViewPurchaseHistoryStatus
    {
        Success,
        NotSystemAdmin,
        NoUserFound,
        NoStoreFound,
	    MistakeTipGiven,
		NoDB = 500,
        BadInput = 600
    }

    public enum ViewSystemLogStatus
    {
        Success,
        NotSystemAdmin,
        NoDB = 500,
        BadInput = 600
    }

    public enum ViewSystemErrorStatus
    {
        Success,
        NotSystemAdmin,
        NoDB = 500,
        BadInput = 600
    }

    public enum EditCategoryStatus
    {
        Success,
        CategoryNotExistsInSystem,
        CategoryAlradyExist,
		InvalidCategory,
        NoDB = 500,
        BadInput = 600
    }

    public enum EditPolicyStatus
    {
        Success,
        InvalidPolicyData,
        NoAuthority,
        NoDB = 500,
        BadInput = 600

    }

    public enum ViewPolicyStatus
    {
        Success,
        NoAuthority,
        NoDB = 500,
        BadInput = 600
    }

	public enum GetEntranceDetailsEnum
	{
		Success,
		NoAuthority,
		NoDB = 500,
	    BadInput = 600
    }
}
