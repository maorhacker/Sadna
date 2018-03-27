﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SadnaSrc.Main;
using SadnaSrc.UserSpot;

namespace UserSpotTests
{
    [TestClass]
    public class UseCase1_1_Test
    {
        private UserService userServiceSession;
        private User generatedGuest;
        private MarketYard marketSession;

        [TestInitialize]
        public void MarketBuilder()
        {
            marketSession = new MarketYard();
            userServiceSession = (UserService)marketSession.GetUserService();
            Assert.AreEqual((int) EnterSystemStatus.Success, userServiceSession.EnterSystem().Status);
            generatedGuest = userServiceSession.GetUser();
        }

        [TestMethod]
        public void GuestHasNoPolicyTest()
        {
            Assert.AreEqual(0, generatedGuest.GetPolicies().Length);

        }

        [TestMethod]
        public void GuestDataTest()
        {
            object[] expectedData = { generatedGuest.SystemID, null, null, null };
            Assert.IsTrue(expectedData.SequenceEqual(generatedGuest.ToData()));
        }

        [TestMethod]
        public void NewGuestCartEmptyTest()
        {
            Assert.AreEqual(0, generatedGuest.GetCart().Length);
        }

        [TestCleanup]
        public void UserTestCleanUp()
        {
            userServiceSession.CleanSession();
            MarketLog.RemoveLogs();
            MarketException.RemoveErrors();
            marketSession.Exit();
        }
    }
}