﻿using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using SadnaSrc.Main;
using SadnaSrc.MarketData;

namespace SadnaSrc.UserSpot
{
    public class UserDL : IUserDL
    {

        private static UserDL _instance;

        public static UserDL Instance => _instance ?? (_instance = new UserDL());

        private readonly IMarketDB dbConnection;

        private UserDL()
        {
            dbConnection = new ProxyMarketDB();
        }

        public int[] GetAllSystemIDs()
        {
            var ids = new List<int>();
            using (var dbReader = dbConnection.SelectFromTable("Users", "SystemID"))
            {
                while (dbReader.Read())
                {
                    if (dbReader.GetValue(0) != null)
                    {
                        ids.Add(dbReader.GetInt32(0));
                    }
                }
            }

            return ids.ToArray();
        }

        public bool IsUserNameExist(string name)
         {
             dbConnection.CheckInput(name);
            using (var dbReader = dbConnection.SelectFromTableWithCondition("Users", "*", "Name = '" + name + "'"))
            {
                return dbReader.Read();

            }
        }
        public RegisteredUser RegisterUser(int userID,string name, string address, string password, string creditCard, CartItem[] guestCart)
        {
            dbConnection.CheckInput(name); dbConnection.CheckInput(address);
            dbConnection.CheckInput(password); dbConnection.CheckInput(creditCard);
            string[] columnNames = { "Name" , "Address" , "Password","CreditCard" };
            string[] valuesNames = {"@name", "@address", "@password","@card"};
            object[] values = {name, address, password,creditCard};
            dbConnection.UpdateTable("Users","SystemID = "+ userID, columnNames ,valuesNames,values);
            SaveCartItem(userID,guestCart);
            return new RegisteredUser(this,userID, name,address,password,creditCard,guestCart);
        }
        public void SaveUserStatePolicy(int userID,StatePolicy policy)
        {
            dbConnection.CheckInput(policy.GetStateString());
            string [] valuesNames = {"@idParam","@stateParam"};
            object[] values = { userID, policy.GetStateString()};
            dbConnection.InsertTable("StatePolicy", "SystemID,State",valuesNames,values);
        }

        private int GetIDFromUserName(string userName)
        {
            dbConnection.CheckInput(userName);
            using (var dbReader = dbConnection.SelectFromTableWithCondition("Users", "SystemID", "Name = '" + userName + "'"))
            {
                if (dbReader.Read())
                {
                    return dbReader.GetInt32(0);
                }
                return -1;
            }
        }

        public void SaveUserStorePolicy(string userName,StoreManagerPolicy policy)
        {
            dbConnection.CheckInput(policy.Store); dbConnection.CheckInput(StoreManagerPolicy.GetStoreActionString(policy.Action));
            int idOfPromoted = GetIDFromUserName(userName);
            string[] valuesNames = { "@idParam", "@storeParam","@actionParam" };
            object[] values = { idOfPromoted, policy.Store, StoreManagerPolicy.GetStoreActionString(policy.Action) };
            dbConnection.InsertTable("StoreManagerPolicy", "SystemID,Store,Action", valuesNames, values);
        }

        public void SaveUserStorePolicy(int userID,StoreManagerPolicy policy)
        {
            dbConnection.CheckInput(policy.Store); dbConnection.CheckInput(StoreManagerPolicy.GetStoreActionString(policy.Action));
            string[] valuesNames = { "@idParam", "@storeParam", "@actionParam" };
            object[] values = { userID, policy.Store, StoreManagerPolicy.GetStoreActionString(policy.Action) };
            dbConnection.InsertTable("StoreManagerPolicy", "SystemID,Store,Action", valuesNames, values);
        }

        public void DeleteUserStorePolicy(string userName, StoreManagerPolicy policy)
        {
            dbConnection.CheckInput(policy.Store); dbConnection.CheckInput(policy.Action.ToString());
            int idOfDemoted = GetIDFromUserName(userName);
            dbConnection.DeleteFromTable("StoreManagerPolicy","SystemID = "+ idOfDemoted + " AND Store = '"+policy.Store
                                                 + "' AND Action = '" + policy.Action +"'");
        }

        private StatePolicy[] LoadUserStatePolicy(int userID)
        {
            List<StatePolicy> loadedStatesPolicies = new List<StatePolicy>();
            using (var dbReader = dbConnection.SelectFromTableWithCondition("StatePolicy", "State", "SystemID = " + userID))
            {
                while (dbReader.Read())
                {
                    StatePolicy.State state = StatePolicy.GetStateFromString(dbReader.GetString(0));
                    loadedStatesPolicies.Add(new StatePolicy(state));
                }
            }
            return loadedStatesPolicies.ToArray();
        }

        public StoreManagerPolicy[] LoadUserStorePolicies(int userID)
        {
            List<StoreManagerPolicy> loadedStorePolicies = new List<StoreManagerPolicy>();
            using (var dbReader = dbConnection.SelectFromTableWithCondition("StoreManagerPolicy", "*", "SystemID = " + userID))
            {
                while (dbReader.Read())
                {
                    string storeName = dbReader.GetString(1);
                    StoreManagerPolicy.StoreAction action =
                        StoreManagerPolicy.GetActionFromString(dbReader.GetString(2));
                    loadedStorePolicies.Add(new StoreManagerPolicy(storeName,action));

                }
            }
            return loadedStorePolicies.ToArray();
        }
        public virtual void SaveUser(User user)
        {
            dbConnection.InsertTable("Users", "SystemID",
                    new [] {"@idParam"}, new object[]{user.SystemID});         
        }

        public string[] UserNamesInSystem()
        {
            var userNames = new List<string>();
            using (var dbReader = dbConnection.SelectFromTable("Users", "Name"))
            {
                while (dbReader.Read())
                {
                    if (!dbReader.IsDBNull(0))
                    {
                        userNames.Add(dbReader.GetString(0));
                    }
                }

            }
            return userNames.ToArray();
        }

        public object[] FindRegisteredUserData(string name, string password)
        {
            dbConnection.CheckInput(name); dbConnection.CheckInput(password);
            using (var dbReader = dbConnection.SelectFromTableWithCondition("Users", "*", "name = '" + name + "' AND password = '"+ password +"'"))
            {
                if(dbReader.Read())
                {
                    return new object[] {dbReader.GetInt32(0),dbReader.GetString(1), dbReader.GetString(2),
                        dbReader.GetString(3),dbReader.GetString(4)};
                }

                return null;
            }
        }
        public RegisteredUser LoadUser(object[] userData, CartItem[] guestCart)
        {
            var loadedID = (int)userData[0];
            var loadedName = (string)userData[1];
            var loadedAddress = (string)userData[2];
            var loadedPassword = (string)userData[3];
            var loadedCreditCard = (string)userData[4];
            SaveCartItem(loadedID, guestCart);
            return new RegisteredUser(this,loadedID, loadedName, loadedAddress, loadedPassword, loadedCreditCard, 
                LoadCartItems(loadedID), LoadUserStatePolicy(loadedID), LoadUserStorePolicies(loadedID));
        }

        public void RemoveCart(int userID)
        {
            dbConnection.DeleteFromTable("CartItem","SystemID = "+ userID);
        }

        public void SaveCartItem(int userID,CartItem[] cart)
        {
            foreach (CartItem item in cart)
            {
                var userItem = new List<object>();
                userItem.Add(userID);
                userItem.AddRange(item.ToData());
                object[] itemVals = userItem.ToArray();
                foreach (object val in itemVals)
                    dbConnection.CheckInput(val.ToString());

                dbConnection.InsertTable("CartItem", "SystemID,Name,Store,Quantity,UnitPrice,FinalPrice",
                    new [] { "@idParam", "@nameParam", "@storeParam","@quantityParam","@unitpriceParam","@finalpriceParam"}, itemVals);
            }
        }

        public void RemoveCartItem(int userID,CartItem item)
        {
            dbConnection.CheckInput(item.Name); dbConnection.CheckInput(item.Store);
            dbConnection.DeleteFromTable("CartItem", "SystemID = "+ userID + " AND "+ item.GetDbIdentifier());
        }
        private CartItem[] LoadCartItems(int userID)
        {
            List<CartItem> loadedItems = new List<CartItem>();
            using (var dbReader = dbConnection.SelectFromTableWithCondition("CartItem", "*", "SystemID = " + userID))
            {
                while (dbReader.Read())
                {
                    loadedItems.Add(new CartItem(dbReader.GetString(1), null,
                        dbReader.GetString(2),dbReader.GetInt32(3),dbReader.GetDouble(4)));
                }
            }
            return loadedItems.ToArray();
        }

        public void UpdateCartItemQuantity(CartItem item)
        {
            string[] columnNames = { "Quantity", "FinalPrice"};
            string[] valuesNames = { "@quantity", "@price"};
            object[] values = { item.Quantity,item.FinalPrice};
            dbConnection.CheckInput(item.Name); dbConnection.CheckInput(item.Store);
            dbConnection.UpdateTable("CartItem", item.GetDbIdentifier(), columnNames, valuesNames, values);
        }


	    public string[] GetAllActiveStoreNames()
	    {
		    var result = new List<string>();
		    using (var dbReader = dbConnection.SelectFromTableWithCondition("Store", "Name", "Status = 'Active'"))
		    {
			    while (dbReader.Read())
			    {
				    result.Add(dbReader.GetString(0));
			    }
		    }

		    return result.ToArray();
	    }

		public void InsertSignedInUser(int systemId)
		{
			string[] valuesNames = { "@systemidParam", "@timeParam" };
			object[] values = { systemId, DateTime.Now.Date };
			dbConnection.InsertTable("SignInReports", "SystemID,Time", valuesNames, values);
		}

    }
}
