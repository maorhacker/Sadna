﻿using BlackBox;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SadnaSrc.Main;
using SadnaSrc.MarketData;

namespace BlackBox.AdminBlackBoxTests
{
	[TestClass]
	public class UseCase5_2
	{
		private IStoreShoppingBridge _storeBridge;
		private IStoreManagementBridge _managerBridge;
		private IUserBridge _adminSignInBridge;
		private IAdminBridge _adminBridge;
		private IUserBridge _signUpBridge1;
		private IUserBridge _signUpBridge2;
		private IUserBridge _signInBridge;
		private readonly string userSoleStoreOwner = "PninaSoleStoreOwner";
		private readonly string userNotSoleStoreOwner = "PninaNotSoleStoreOwner";
		private readonly string userSoleStoreOwnerPass = "789456";
		private readonly string userNotSoleStoreOwnerPass = "741852";
		private readonly string adminName = "Arik1";
		private readonly string adminPass = "123";

		[TestInitialize]

		public void MarketBuilder()
		{
		    MarketDB.Instance.InsertByForce();
            _adminBridge = AdminDriver.getBridge();
			SignUp(ref _signUpBridge1, userSoleStoreOwner, "mishol", userSoleStoreOwnerPass, "12345678");
			SignUp(ref _signUpBridge2, userNotSoleStoreOwner, "susia", userNotSoleStoreOwnerPass, "12345678");
			_storeBridge = StoreShoppingDriver.getBridge();
			_storeBridge.GetStoreShoppingService(_signUpBridge1.GetUserSession());
			_storeBridge.OpenStore("blah", "blah2");
			_managerBridge = null;
		}

		[TestMethod]

		public void SuccessDeleteUserNotSoleOwner()
		{
			SignIn(adminName, adminPass);
			_adminBridge.GetAdminService(_adminSignInBridge.GetUserSession());
			Assert.AreEqual((int)RemoveUserStatus.Success, _adminBridge.RemoveUser(userNotSoleStoreOwner).Status);
			_signInBridge = UserDriver.getBridge();
			_signInBridge.EnterSystem();
			Assert.AreEqual((int)SignInStatus.NoUserFound,_signInBridge.SignIn(userNotSoleStoreOwner, userNotSoleStoreOwnerPass).Status);
		}

		[TestMethod]

		public void SuccessDeleteUserSoleOwner()
		{
			SignIn(adminName, adminPass);
			_adminBridge.GetAdminService(_adminSignInBridge.GetUserSession());
			_signInBridge = UserDriver.getBridge();
			_signInBridge.EnterSystem();
			Assert.AreEqual((int)RemoveUserStatus.Success, _adminBridge.RemoveUser(userSoleStoreOwner).Status);
			_managerBridge = StoreManagementDriver.getBridge();
			_managerBridge.GetStoreManagementService(_adminSignInBridge.GetUserSession(),"blah");
			var res = _managerBridge.CloseStore();
			Assert.AreEqual((int)StoreEnum.StoreNotExists, res.Status);
			Assert.AreEqual((int)SignInStatus.NoUserFound, _signInBridge.SignIn(userSoleStoreOwner, userSoleStoreOwnerPass).Status);

		}

		[TestMethod]

		public void DeleteMySelf()
		{
			SignIn(adminName, adminPass);
			_adminBridge.GetAdminService(_adminSignInBridge.GetUserSession());
			Assert.AreEqual((int)RemoveUserStatus.SelfTermination, _adminBridge.RemoveUser(adminName).Status);
		}

		[TestMethod]

		public void UserToRemoveWasntFound()
		{
			SignIn(adminName, adminPass);
			_adminBridge.GetAdminService(_adminSignInBridge.GetUserSession());
			Assert.AreEqual((int)RemoveUserStatus.NoUserFound, _adminBridge.RemoveUser("sdadasdasdasdasdasdas").Status);
		}

		[TestMethod]

		public void NoSystemAdmin1()
		{
			SignIn("hello", adminPass);
			_adminBridge.GetAdminService(_adminSignInBridge.GetUserSession());
			Assert.AreEqual((int)RemoveUserStatus.NotSystemAdmin, _adminBridge.RemoveUser(userNotSoleStoreOwner).Status);
		}

		[TestMethod]

		public void NoSystemAdmin2()
		{
			SignIn(adminName, "852963");
			_adminBridge.GetAdminService(_adminSignInBridge.GetUserSession());
			Assert.AreEqual((int)RemoveUserStatus.NotSystemAdmin, _adminBridge.RemoveUser(userNotSoleStoreOwnerPass).Status);
		}

		[TestMethod]

		public void NoSystemAdmin3()
		{
			SignIn("Hello", "852963");
			_adminBridge.GetAdminService(_adminSignInBridge.GetUserSession());
			Assert.AreEqual((int)RemoveUserStatus.NotSystemAdmin, _adminBridge.RemoveUser(userSoleStoreOwnerPass).Status);
		}

		private void SignIn(string userName, string password)
		{
			_adminSignInBridge = UserDriver.getBridge();
			_adminSignInBridge.EnterSystem();
			_adminSignInBridge.SignIn(userName, password);
		}

		private void SignUp(ref IUserBridge _userBridge, string userName, string address, string password, string creditCard)
		{
			_userBridge = UserDriver.getBridge();
			_userBridge.EnterSystem();
			_userBridge.SignUp(userName, address, password, creditCard);
		}

		[TestCleanup]

		public void UserTestCleanUp()
		{
		    MarketDB.Instance.CleanByForce();
		    MarketYard.CleanSession();

        }
	}
}