﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SadnaSrc.AdminView;
using SadnaSrc.Main;
using SadnaSrc.MarketHarmony;
using SadnaSrc.OrderPool;
using SadnaSrc.StoreCenter;
using SadnaSrc.UserSpot;

namespace IntegrationTests
{
    [TestClass]
    public class UserSpot_OrderPool_Test
    {
        private IUserService userServiceSession;
        private OrderService orderServiceSession;
        private UserBuyerHarmony userBuyerHarmony;

        private MarketYard marketSession;
        private string user = "Vadim Chernov";
        private string emptyUser = "Arik1";
        private string singleItemUser = "Vova";
        private string pass = "123";

        [TestInitialize]
        public void MarketBuilder()
        {
            marketSession = MarketYard.Instance;
            userServiceSession = (UserService)marketSession.GetUserService();
            userServiceSession.EnterSystem();
            orderServiceSession = (OrderService)marketSession.GetOrderService(ref userServiceSession);
            userBuyerHarmony = new UserBuyerHarmony(ref userServiceSession);
        }

        /*
         * CheckoutAll tests
         */

        [TestMethod]
        public void CheckoutAllTest()
        {
            try
            {
                string result = getItemsFromCart(user, pass);
                string expected = "20 OCB, 24. 18 Bamba, The Red Rock. 33 Goldstar, The Red Rock. ";

                Assert.AreEqual(result,expected);
            }
            catch (MarketException)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void CheckoutAllSingleItemTest()
        {
            try
            {
                string result = getItemsFromCart(singleItemUser, pass);
                string expected = "10 Coated Peanuts, 24. ";

                Assert.AreEqual(result, expected);
            }
            catch (MarketException)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void CheckoutAllEmptyCartTest()
        {
            try
            {
                string result = getItemsFromCart(emptyUser, pass);
                string expected = "";

                Assert.AreEqual(result, expected);
            }
            catch (MarketException)
            {
                Assert.Fail();
            }
        }

        /*
         * CheckoutFromStore tests
         */

        [TestMethod]
        public void CheckoutSingleStoreTest()
        {
            try
            {
                string result = getSingleStoreItems("24");
                string expected = "20 OCB, 24. ";

                Assert.AreEqual(result, expected);
            }
            catch (MarketException)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void CheckoutNonExistantStoreTest()
        {
            try
            {
                string result = getSingleStoreItems("The Blue Rock");
                string expected = "";

                Assert.AreEqual(result, expected);
            }
            catch (MarketException)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void CheckoutStoreNotInOrderTest()
        {
            try
            {
                string result = getSingleStoreItems("Cluckin' Bell");
                string expected = "";

                Assert.AreEqual(result, expected);
            }
            catch (MarketException)
            {
                Assert.Fail();
            }
        }

        /*
         * EmptyCart (all) tests
         */

        [TestMethod]
        public void EmptyCartTest()
        {
            try
            {
                userServiceSession.SignIn(user, pass);
                userBuyerHarmony.EmptyCart();

                Assert.AreEqual(0, userServiceSession.ViewCart().ReportList.Length);
            }
            catch (MarketException)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void EmptyCartAlreadyEmptyTest()
        {
            try
            {
                userServiceSession.SignIn(emptyUser, pass);
                userBuyerHarmony.EmptyCart();

                Assert.AreEqual(0, userServiceSession.ViewCart().ReportList.Length);
            }
            catch (MarketException)
            {
                Assert.Fail();
            }
        }

        /*
         * EmptyCart (store) tests
         */

        [TestMethod]
        public void EmptyCartSingleStoreTest()
        {
            try
            {
                userServiceSession.SignIn(user, pass);
                userBuyerHarmony.EmptyCart("The Red Rock");

                Assert.AreEqual(1, userServiceSession.ViewCart().ReportList.Length);
            }
            catch (MarketException)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void EmptyCartNonExistantStoreTest()
        {
            try
            {
                userServiceSession.SignIn(user, pass);
                userBuyerHarmony.EmptyCart("The Blue Rock");

                Assert.AreEqual(3, userServiceSession.ViewCart().ReportList.Length);
            }
            catch (MarketException)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void EmptyCartAlreadyEmptySingleStoreTest()
        {
            try
            {
                userServiceSession.SignIn(emptyUser, pass);
                userBuyerHarmony.EmptyCart("24");

                Assert.AreEqual(0, userServiceSession.ViewCart().ReportList.Length);
            }
            catch (MarketException)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void EmptyCartWithSingleStoreTest()
        {
            try
            {
                userServiceSession.SignIn(singleItemUser, pass);
                userBuyerHarmony.EmptyCart("24");

                Assert.AreEqual(0, userServiceSession.ViewCart().ReportList.Length);
            }
            catch (MarketException)
            {
                Assert.Fail();
            }
        }

        [TestCleanup]
        public void UserOrderTestCleanUp()
        {
            userServiceSession.CleanGuestSession();
            orderServiceSession.CleanSession();
            userBuyerHarmony.CleanSession();
            MarketYard.CleanSession();
        }

        /*
         * Private helper functions
         */

        private string getItemsFromCart(string username, string password)
        {
            userServiceSession.SignIn(username, password);
            OrderItem[] items = userBuyerHarmony.CheckoutAll();
            return getOrderString(items);
        }

        private string getSingleStoreItems(string store)
        {
            userServiceSession.SignIn(user, pass);
            OrderItem[] items = userBuyerHarmony.CheckoutFromStore(store);
            return getOrderString(items);
        }

        private string getOrderString(OrderItem[] items)
        {
            Order o = orderServiceSession.InitOrder(items);
            OrderItem[] orderItems = o.GetItems().ToArray();
            string result = "";
            for (int i = 0; i < orderItems.Length; i++)
            {

                result += "" + orderItems[i].Price + " " + orderItems[i].Name + ", " + orderItems[i].Store + ". ";
            }

            return result;
        }
    }
}
