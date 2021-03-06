﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SadnaSrc.Main;
using SadnaSrc.MarketHarmony;
using SadnaSrc.StoreCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadnaSrc.MarketData;

namespace StoreCenterTests.StoreCenterDbIntegrationTests
{
    //TODO: maybe remove these tests

    [TestClass]
    public class AddProductsTests
    {
        private MarketYard market;
        public StockListItem ProductToDelete;
        private IStoreDL handler;
        IUserService userService;
        [TestInitialize]
        public void BuildStore()
        {
            MarketDB.Instance.InsertByForce();
            market = MarketYard.Instance;
            handler = StoreDL.Instance;
            userService = market.GetUserService();
        }
        [TestMethod]
        public void AddProductWhenStoreNotExists()
        {
            userService.EnterSystem();
            userService.SignIn("Arik1", "123");
            StoreManagementService liorSession = (StoreManagementService)market.GetStoreManagementService(userService, "storeNotExists");
            MarketAnswer ans = liorSession.AddNewProduct("name0", 1, "des", 4);
            Assert.AreEqual((int)StoreEnum.StoreNotExists, ans.Status); 
        }
        [TestMethod]
        public void AddProductWhenHasNoPremmision()
        {
            userService.EnterSystem();
            userService.SignIn("Big Smoke", "123");
            StoreManagementService liorSession = (StoreManagementService)market.GetStoreManagementService(userService, "X");
            MarketAnswer ans = liorSession.AddNewProduct("name0", 1, "des", 4);
            Assert.AreEqual((int)StoreEnum.NoPermission, ans.Status);
        }
        [TestMethod]
        public void AddProductWhenProductNameIsNotAvailableInStore()
        {
            userService.EnterSystem();
            userService.SignIn("Arik1", "123");
            StoreManagementService liorSession = (StoreManagementService)market.GetStoreManagementService(userService, "X");
            MarketAnswer ans = liorSession.AddNewProduct("BOX", 1, "des", 4);
            Assert.AreEqual((int)StoreEnum.ProductNameNotAvlaiableInShop, ans.Status);
        }
     
        [TestMethod]
        public void AddProductSuccess()
        {
            userService.EnterSystem();
            userService.SignIn("Arik1", "123");
            StoreManagementService liorSession = (StoreManagementService)market.GetStoreManagementService(userService, "X");
            MarketAnswer ans = liorSession.AddNewProduct("item", 1, "des", 4);
            ProductToDelete = handler.GetProductFromStore("X", "item");
            Assert.AreEqual((int)StoreEnum.Success, ans.Status);
        }

        [TestMethod]
        public void AddProductBadInputFail()
        {
            userService.EnterSystem();
            userService.SignIn("Arik1", "123");
            StoreManagementService liorSession = (StoreManagementService)market.GetStoreManagementService(userService, "X");
            MarketAnswer ans = liorSession.AddNewProduct("it'em", 1, "des", 4);
            Assert.AreEqual((int)StoreEnum.BadInput, ans.Status);
        }

        [TestCleanup]
        public void CleanUpOpenStoreTest()
        {
            MarketDB.Instance.CleanByForce();
            MarketYard.CleanSession();
        }
    }
}
