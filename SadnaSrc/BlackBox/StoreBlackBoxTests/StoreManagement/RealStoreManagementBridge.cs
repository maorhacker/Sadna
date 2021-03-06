﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadnaSrc.Main;

namespace BlackBox
{
    class RealStoreManagement : IStoreManagementBridge
    {
        private readonly MarketYard _market;
        private IStoreManagementService _storeManagementService;

        public RealStoreManagement()
        {
            _market = MarketYard.Instance;
            _storeManagementService = null;
        }

        public void GetStoreManagementService(IUserService userService, string store)
        {
            _storeManagementService = _market.GetStoreManagementService(userService, store);
        }

        public MarketAnswer PromoteToStoreManager(string someoneToPromoteName, string actions)
        {
            return _storeManagementService.PromoteToStoreManager(someoneToPromoteName, actions);
        }

        public MarketAnswer AddNewProduct(string _name, int _price, string _description, int quantity)
        {
            return _storeManagementService.AddNewProduct(_name, _price, _description, quantity);
        }

        public MarketAnswer RemoveProduct(string productName)
        {
            return _storeManagementService.RemoveProduct(productName);
        }

        public MarketAnswer EditProduct(string productName, string productNewName, string basePrice, string description)
		{
            return _storeManagementService.EditProduct(productName, productNewName, basePrice, description);
        }


        public MarketAnswer AddNewLottery(string _name, double _price, string _description, DateTime startDate,
            DateTime endDate)
        {
            return _storeManagementService.AddNewLottery(_name,_price,_description, startDate, endDate);
        }

        public MarketAnswer AddQuanitityToProduct(string productName, int quantity)
        {
            return _storeManagementService.AddQuanitityToProduct(productName, quantity);
        }

        public MarketAnswer AddDiscountToProduct(string productName, DateTime startDate, DateTime endDate,
            int discountAmount, string discountType, bool presenteges)
        {
            return _storeManagementService.AddDiscountToProduct(productName, startDate, endDate, discountAmount,
                discountType, presenteges);
        }

        public MarketAnswer EditDiscount(string product, string discountCode, bool isHidden, string startDate, string EndDate, string discountAmount, bool isPercentage)
        {
            return _storeManagementService.EditDiscount(product, discountCode, isHidden, startDate, EndDate, discountAmount, isPercentage);
        }

        public MarketAnswer RemoveDiscountFromProduct(string productName)
        {
            return _storeManagementService.RemoveDiscountFromProduct(productName);
        }

        public MarketAnswer ViewStoreHistory()
        {
            return _storeManagementService.ViewStoreHistory();
        }

        public MarketAnswer ViewPromotionHistory()
        {
            return _storeManagementService.ViewPromotionHistory();
        }

        public MarketAnswer CloseStore()
        {
            return _storeManagementService.CloseStore();
        }

    }
}