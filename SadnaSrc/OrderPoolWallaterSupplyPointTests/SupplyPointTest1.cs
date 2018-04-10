﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SadnaSrc.Main;
using SadnaSrc.OrderPool;
using SadnaSrc.SupplyPoint;

namespace OrderPoolWallaterSupplyPointTests
{
    [TestClass]
    public class SupplyPointTest1
    {
        private MarketYard market;
        private OrderItem item1;
        private OrderItem item2;
        private OrderItem item3;
        private IUserService userService;
        private OrderService orderService;
        private SupplyService supplyService;

        [TestInitialize]
        public void BuildSupplyPoint()
        {
            market = MarketYard.Instance;
            userService = market.GetUserService();
            orderService = (OrderService)market.GetOrderService(ref userService);
            orderService.LoginBuyer("Big Smoke","123");
            item1 = new OrderItem("Cluckin Bell", "#9", 5.00, 2);
            item2 = new OrderItem("Cluckin Bell", "#9 Large", 7.00, 1);
            item3 = new OrderItem("Cluckin Bell", "#6 Extra Dip", 8.50, 1);
            supplyService = new SupplyService(orderService);
        }

        [TestMethod]
        public void TestExternalSystemAttachment()
        {
            MarketAnswer ans = supplyService.AttachExternalSystem();
            Assert.AreEqual((int)SupplyStatus.Success, ans.Status);
        }

        [TestMethod]
        public void TestNoExternalSystem()
        {
            try
            {
                int orderId;
                orderService.CreateOrder(out orderId);
                supplyService.CreateDelivery(orderId, "Grove Street");
                Assert.Fail();
            }
            catch (MarketException e)
            {
                Assert.AreEqual((int)SupplyStatus.NoSupplySystem, e.Status);
            }
        }

        [TestMethod]
        public void TestSuccesfulOrder()
        {
            supplyService.AttachExternalSystem();
            int orderId;
            orderService.CreateOrder(out orderId);
            orderService.AddItemToOrder(orderId, item1.Store, item1.Name, item1.Price, item1.Quantity);
            orderService.AddItemToOrder(orderId, item2.Store, item2.Name, item2.Price, item2.Quantity);
            MarketAnswer ans = supplyService.CreateDelivery(orderId, "Grove Street");
            Assert.AreEqual((int)SupplyStatus.Success, ans.Status);
        }

        [TestMethod]
        public void TestExternalSystemError()
        {
            supplyService.AttachExternalSystem();
            int orderId;
            orderService.CreateOrder(out orderId);
            orderService.AddItemToOrder(orderId, item1.Store, item1.Name, item1.Price, item1.Quantity);
            orderService.AddItemToOrder(orderId, item2.Store, item2.Name, item2.Price, item2.Quantity);
            supplyService.FuckUpExternal();
            try
            {
                MarketAnswer ans = supplyService.CreateDelivery(orderId, "Grove Street");
                Assert.Fail();
            }
            catch (MarketException e)
            {
                Assert.AreEqual((int)SupplyStatus.SupplySystemError, e.Status);

            }
        }

        [TestCleanup]
        public void UserTestCleanUp()
        {

            userService.CleanSession();
            MarketYard.CleanSession();
        }
    }
}
