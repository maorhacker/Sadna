﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadnaSrc.Main
{
    public interface IStoreManagementService
    {

        /**
         *StoreManagers/StoreOwners Promotion
         */
        MarketAnswer PromoteToStoreManager(string someoneToPromoteName, string actions);

        /**
         * Products Management
         **/
        MarketAnswer AddNewProduct(string _name, double _price, string _description, int quantity);
        MarketAnswer RemoveProduct(string productName);
        MarketAnswer EditProduct(string productName, string whatToEdit, string newValue);
        MarketAnswer AddQuanitityToProduct(string productName, int quantity);
        MarketAnswer ChangeProductPurchaseWayToImmediate(string productName);
        MarketAnswer ChangeProductPurchaseWayToLottery(string productName, DateTime startDate, DateTime endDate);

        MarketAnswer AddNewLottery(string _name, double _price, string _description, DateTime startDate,
            DateTime endDate);
        /**
         * Category Managment
         **/
        MarketAnswer AddProductToCategory(string productName, string categoryName);
        MarketAnswer RemoveProductFromCategory(string productName, string categoryName);
        /**
         * Discounts Management
         **/

        MarketAnswer AddDiscountToProduct(string productName, DateTime startDate, DateTime endDate, 
        int discountAmount,string discountType, bool presenteges);
        MarketAnswer EditDiscount(string productName, string whatToEdit, string newValue);
        MarketAnswer RemoveDiscountFromProduct(string productName);
        /**
         * History View
         */

        MarketAnswer ViewStoreHistory();
	    MarketAnswer CloseStore();


    }
    public enum StoreEnum
    {
        Success,
        ProductNotFound,
        UpdateProductFail,
        CloseStoreFail,
        StoreNotExists,
        ProductNameNotAvlaiableInShop,
        NoPermission,
        QuantityIsNegative,
        QuantityIsTooBig,
        EnumValueNotExists,
        DatesAreWrong,
        CategoryExistsInStore,
        CategoryNotExistsInStore,
        ProductAlreadyInCategory,
        ProductNotInCategory
    }
    public enum DiscountStatus
    {
        Success,
        NoStore,
        DatesAreWrong,
        AmountIsHundredAndpresenteges,
        DiscountGreaterThenProductPrice,
        ThereIsAlreadyAnotherDiscount,
        ProductNotFound,
        DiscountNotFound,
        DiscountAmountIsNegativeOrZero,
        DiscountAmountIsNotNumber,
        PrecentegesIsNotBoolean,
        NoLegalAttrebute
    }
    public enum ManageStoreStatus
    {
        Success,
        InvalidStore,
        InvalidManager
        
    }

    public enum ViewStorePurchaseHistoryStatus
    {
        Success,
        InvalidStore,
        InvalidManager
    }
    public enum StoreSyncStatus
    {
        NoStore,
        NoProduct
    }

	public enum PromoteStoreStatus
	{
		Success,
		InvalidStore,
		PromoteSelf,
		PromotionOutOfReach,
		NoAuthority,
		NoUserFound,
		InvalidPromotion
	}
    public enum CalculateEnum
    {
        Success,
        StoreNotExists,
        ProductNotFound,
        QuantityIsGreaterThenStack,
        ProductHasNoDiscount,
        DiscountCodeIsWrong,
        DiscountExpired,
        DiscountNotStarted,
        QuanitityIsNonPositive,
        DiscountIsNotHidden
    }
    public enum ChangeToLotteryEnum
    {
        Success,
        StoreNotExists,
        ProductNotFound,
        LotteryExists,
        DatesAreWrong,
    }


    public enum PurchaseEnum { Immediate, Lottery };
    public enum DiscountTypeEnum { Hidden, Visible };
    public enum LotteryTicketStatus { Waiting, Winning, Losing, Cancel };
}
