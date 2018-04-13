﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SadnaSrc.Main;
using SadnaSrc.MarketHarmony;
using SadnaSrc.StoreCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreCenterTests
{
    [TestClass]
    public class RemoveDiscountFromProductTests
    {
        private MarketYard market;
        public StockListItem ProductToDelete;
        private ModuleGlobalHandler handler;
        IUserService userService;
        [TestInitialize]
        public void BuildStore()
        {

            market = MarketYard.Instance;
            handler = ModuleGlobalHandler.GetInstance();
            userService = market.GetUserService();
        }
        [TestMethod]
        public void RemoveDiscountWhenStoreNotExists()
        {
            userService.EnterSystem();
            userService.SignIn("Arik1", "123");
            StoreManagementService liorSession = (StoreManagementService)market.GetStoreManagementService(userService, "storeNotExists");
            MarketAnswer ans = liorSession.RemoveDiscountFromProduct("BOX");
            Assert.AreEqual((int)DiscountStatus.NoStore, ans.Status);
        }
        [TestMethod]
        public void RemoveDiscountWhenHasNoPremmision()
        {
            userService.EnterSystem();
            userService.SignIn("Big Smoke", "123");
            StoreManagementService liorSession = (StoreManagementService)market.GetStoreManagementService(userService, "X");
            MarketAnswer ans = liorSession.RemoveDiscountFromProduct("BOX");
            Assert.AreEqual((int)ViewStoreStatus.InvalidUser, ans.Status);
        }
        [TestMethod]
        public void RemoveDiscountWhenProductNotExists()
        {
            userService.EnterSystem();
            userService.SignIn("Arik1", "123");
            StoreManagementService liorSession = (StoreManagementService)market.GetStoreManagementService(userService, "X");
            MarketAnswer ans = liorSession.RemoveDiscountFromProduct("notExists");
            Assert.AreEqual((int)DiscountStatus.ProductNotFound, ans.Status);
        }
        public void RemoveDiscountWhenDiscountNotExists()
        {
            userService.EnterSystem();
            userService.SignIn("Arik1", "123");
            StoreManagementService liorSession = (StoreManagementService)market.GetStoreManagementService(userService, "X");
            MarketAnswer ans = liorSession.RemoveDiscountFromProduct("Golden BOX");
            Assert.AreEqual((int)DiscountStatus.DiscountNotFound, ans.Status);
        }
        [TestMethod]
        public void RemoveDiscountSuccess()
        {
            userService.EnterSystem();
            userService.SignIn("Arik1", "123");
            StoreManagementService liorSession = (StoreManagementService)market.GetStoreManagementService(userService, "X");
            liorSession.AddNewProduct("item", 1, "des", 4);
            
            liorSession.AddDiscountToProduct("item", DateTime.Parse("01/01/2019"), DateTime.Parse("20/01/2019"), 10, "HIDDEN", true);
            MarketAnswer ans = liorSession.RemoveDiscountFromProduct("item");
            ProductToDelete = handler.GetProductFromStore("X", "item");
            Assert.IsNull(ProductToDelete.Discount);
            Assert.AreEqual((int)DiscountStatus.Success, ans.Status);
        }


        [TestCleanup]
        public void CleanUpOpenStoreTest()
        {
            if (ProductToDelete != null)
            {
                handler.DataLayer.RemoveStockListItem(ProductToDelete);
            }
            userService.CleanSession();
            MarketYard.CleanSession();
        }
    }
}