﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SadnaSrc.Main;
using SadnaSrc.UserSpot;

namespace UserSpotTests.PureUnitTest
{

    [TestClass]
    public class AddToCart_Test
    {
        private UserService userServiceGuestSession;
        private UserService userServiceRegisteredSession;
        private UserService userServiceLoggedSession;
        private UserService userServiceLoggedSession2;
        private MarketYard marketSession;
        private CartItem item1;
        private CartItem item2;
        private CartItem item3;
        private CartItem item4;
        private List<CartItem> expected;

        [TestInitialize]
        public void MarketBuilder()
        {
            marketSession = MarketYard.Instance;
            userServiceGuestSession = (UserService)marketSession.GetUserService();
            userServiceGuestSession.EnterSystem();
            userServiceRegisteredSession = null;
            userServiceLoggedSession = null;
            userServiceLoggedSession2 = null;
            expected = new List<CartItem>();
            item1 = new CartItem("Health Potion", "X", 1, 5.0, "Immediate");
            item2 = new CartItem("Health Potion", "Y", 2, 0.5, "Immediate");
            item3 = new CartItem("Health Potion", "Y", 2, 6.0, "Immediate");
            item4 = new CartItem("Health Potion", "M", 5, 7.0, "Immediate");
        }

        [TestMethod]
        public void LoadCartTest1()
        {
            CartService cart = new CartService(0);
            expected.AddRange(new [] {item1, item2, item3, item4});
            cart.LoadCart(new[]{item1,item2,item3,item4});
            Assert.IsTrue(cart.GetCartStorage().SequenceEqual(expected));
        }

        [TestMethod]
        public void LoadCartTest2()
        {
            CartService cart = new CartService(0);
            cart.LoadCart(new CartItem[0]);
            Assert.IsTrue(cart.GetCartStorage().SequenceEqual(expected));
        }

        [TestMethod]
        public void LoadCartTest3()
        {
            CartService cart = new CartService(0);
            expected.AddRange(new [] {item1,item2});
            cart.LoadCart(new []{item1});
            Assert.IsFalse(cart.GetCartStorage().SequenceEqual(expected));
            cart.LoadCart(new []{item2});
            Assert.IsTrue(cart.GetCartStorage().SequenceEqual(expected));
        }

        [TestMethod]
        public void SearchInCartTest1()
        {
            CartService cart = new CartService(0);
            cart.LoadCart(new[] { item1, item2, item3, item4 });
            Assert.AreEqual(item1, cart.SearchInCart(item1.Store, item1.Name, item1.UnitPrice, item1.Sale));
            Assert.AreEqual(item3, cart.SearchInCart(item3.Store, item3.Name, item3.UnitPrice, item3.Sale));
        }

        [TestMethod]
        public void SearchInCartTest2()
        {
            CartService cart = new CartService(0);
            cart.LoadCart(new[] { item1});
            Assert.AreEqual(null,cart.SearchInCart(item2.Store,item2.Name,item2.UnitPrice,item2.Sale));
            Assert.AreEqual(null, cart.SearchInCart(item4.Store, item4.Name, item4.UnitPrice, item4.Sale));
        }

        [TestMethod]
        public void SearchInCartTest3()
        {
            CartService cart = new CartService(0);
            cart.LoadCart(new CartItem[0]);
            Assert.AreEqual(null, cart.SearchInCart(item1.Store, item1.Name, item1.UnitPrice, item1.Sale));
            Assert.AreEqual(null, cart.SearchInCart(item2.Store, item2.Name, item2.UnitPrice, item2.Sale));
        }

        [TestMethod]
        public void AddToGuestCartTest()
        {
            Assert.AreEqual(0,userServiceGuestSession.MarketUser.Cart.GetCartStorage().Length);
            addAllItems(userServiceGuestSession);
            Assert.IsTrue(expected.ToArray().SequenceEqual(userServiceGuestSession.MarketUser.Cart.GetCartStorage()));

        }

        [TestMethod]
        public void AddToSignedSaveCartTest()
        {
            DoSignUp("MaorCart1", "no-where", "123");
            addAllItems(userServiceRegisteredSession);
            Assert.IsTrue(expected.ToArray().SequenceEqual(userServiceRegisteredSession.MarketUser.Cart.GetCartStorage()));
        }

        [TestMethod]
        public void AddToLoggedSaveCartTest()
        {
            DoSignUpSignIn("MaorCart2", "no-where", "123");
            addAllItems(userServiceLoggedSession);
            Assert.IsTrue(expected.ToArray().SequenceEqual(userServiceLoggedSession.MarketUser.Cart.GetCartStorage()));
        }

        [TestMethod]
        public void FromGuestToSignedSaveCartTest()
        {
            userServiceRegisteredSession = DoEnter();
            addItem1(userServiceRegisteredSession);
            addItem2(userServiceRegisteredSession);
            Assert.IsTrue(expected.ToArray().SequenceEqual(userServiceRegisteredSession.MarketUser.Cart.GetCartStorage()));
            userServiceRegisteredSession.SignUp("MaorCart3", "no-where", "123");
            addItem3(userServiceRegisteredSession);
            addItem4(userServiceRegisteredSession);
            Assert.IsTrue(expected.ToArray().SequenceEqual(userServiceRegisteredSession.MarketUser.Cart.GetCartStorage()));
        }

        [TestMethod]
        public void FromGuestToLoggedSaveCartTest()
        {
            userServiceRegisteredSession = DoEnter();
            addItem1(userServiceRegisteredSession);
            addItem2(userServiceRegisteredSession);
            userServiceRegisteredSession.SignUp("MaorCart4", "no-where", "123");
            DoSignIn("MaorCart4", "123");
            addItem3(userServiceLoggedSession);
            addItem4(userServiceLoggedSession);
            Assert.IsTrue(expected.ToArray().SequenceEqual(userServiceLoggedSession.MarketUser.Cart.GetCartStorage()));
        }

        [TestMethod]
        public void FromSignedToLoggedSaveCartTest()
        {
            DoSignUp("MaorCart5", "no-where", "123");
            addItem1(userServiceRegisteredSession);
            addItem2(userServiceRegisteredSession);
            Assert.IsTrue(expected.ToArray().SequenceEqual(userServiceRegisteredSession.MarketUser.Cart.GetCartStorage()));
            DoSignIn("MaorCart5", "123");
            addItem3(userServiceLoggedSession);
            addItem4(userServiceLoggedSession);
            Assert.IsTrue(expected.ToArray().SequenceEqual(userServiceLoggedSession.MarketUser.Cart.GetCartStorage()));
        }

        [TestMethod]
        public void FromLoggedToLoggedSaveCartTest()
        {
            DoSignUpSignIn("MaorCart6","no-where", "123");
            addItem1(userServiceLoggedSession);
            addItem2(userServiceLoggedSession);
            DoSignIn2("MaorCart6", "123");
            addItem3(userServiceLoggedSession2);
            addItem4(userServiceLoggedSession2);
            Assert.IsTrue(expected.ToArray().SequenceEqual(userServiceLoggedSession2.MarketUser.Cart.GetCartStorage()));
        }

        [TestMethod]
        public void FromGuestToSignedToLoggedToLoggedSaveCartTest()
        {
            userServiceRegisteredSession = DoEnter();
            addItem1(userServiceRegisteredSession);
            userServiceRegisteredSession.SignUp("MaorCart7", "no-where", "123");
            addItem2(userServiceRegisteredSession);
            DoSignIn("MaorCart7", "123");
            addItem3(userServiceLoggedSession);
            DoSignIn2("MaorCart7", "123");
            addItem4(userServiceLoggedSession2);
            Assert.IsTrue(expected.ToArray().SequenceEqual(userServiceLoggedSession2.MarketUser.Cart.GetCartStorage()));

        }

        [TestMethod]
        public void EmptyCartTest()
        {
            DoSignUpSignIn("MaorCart8", "no-where", "123");
            addAllItems(userServiceLoggedSession);
            userServiceLoggedSession.MarketUser.Cart.EmptyCart();
            expected.Clear();
            Assert.IsTrue(expected.ToArray().SequenceEqual(userServiceLoggedSession.MarketUser.Cart.GetCartStorage()));
            DoSignIn2("MaorCart8","123");
            addItem1(userServiceLoggedSession2);
            Assert.IsTrue(expected.ToArray().SequenceEqual(userServiceLoggedSession2.MarketUser.Cart.GetCartStorage()));
        }

        [TestCleanup]
        public void UserTestCleanUp()
        {
            userServiceLoggedSession?.CleanSession();
            userServiceLoggedSession2?.CleanSession();
            userServiceRegisteredSession?.CleanSession();
            userServiceGuestSession.CleanSession();
            MarketYard.CleanSession();
        }

        private UserService DoEnter()
        {
            UserService userService = (UserService)marketSession.GetUserService();
            userService.EnterSystem();
            return userService;
        }

        private void DoSignUp(string name, string address, string password)
        {
            userServiceRegisteredSession = DoEnter();
            Assert.AreEqual((int)SignUpStatus.Success, userServiceRegisteredSession.SignUp(name, address, password).Status);
            userServiceRegisteredSession.Synch();
            Assert.IsTrue(expected.ToArray().SequenceEqual(userServiceRegisteredSession.MarketUser.Cart.GetCartStorage()));

        }

        private void DoSignIn(string name, string password)
        {
            userServiceLoggedSession = DoEnter();
            Assert.AreEqual((int)SignInStatus.Success, userServiceLoggedSession.SignIn(name, password).Status);
            Assert.IsTrue(expected.ToArray().SequenceEqual(userServiceLoggedSession.MarketUser.Cart.GetCartStorage()));
        }

        private void DoSignIn2(string name, string password)
        {
            userServiceLoggedSession2 = DoEnter();
            Assert.AreEqual((int)SignInStatus.Success, userServiceLoggedSession2.SignIn(name, password).Status);
            Assert.IsTrue(expected.ToArray().SequenceEqual(userServiceLoggedSession2.MarketUser.Cart.GetCartStorage()));
        }

        private void DoSignUpSignIn(string name, string address, string password)
        {
            DoSignUp(name, address, password);
            Assert.IsFalse(MarketException.hasErrorRaised());
            DoSignIn(name, password);
        }

        private void addItem1(UserService userService)
        {
            expected.Add(item1);
            userService.AddToCart("Health Potion", "X", 1, 5.0, "Immediate");
        }

        private void addItem2(UserService userService)
        {
            expected.Add(item2);
            userService.AddToCart("Health Potion", "Y", 2, 0.5, "Immediate");
        }

        private void addItem3(UserService userService)
        {
            expected.Add(item3);
            userService.AddToCart("Health Potion", "Y", 2, 6.0, "Immediate");
        }

        private void addItem4(UserService userService)
        {
            expected.Add(item4);
            userService.AddToCart("Health Potion", "M", 5, 7.0, "Immediate");
        }

        private void addAllItems(UserService userService)
        {
            addItem1(userService);
            addItem2(userService);
            addItem3(userService);
            addItem4(userService);
        }

    }
}
