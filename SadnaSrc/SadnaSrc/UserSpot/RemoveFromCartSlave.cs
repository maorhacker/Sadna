﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadnaSrc.Main;

namespace SadnaSrc.UserSpot
{
    class RemoveFromCartSlave
    {
        private readonly UserServiceDL userDB;

        private readonly User _user;

        public UserAnswer Answer { get; private set; }

        public RemoveFromCartSlave(User user)
        {
            userDB = UserServiceDL.Instance;
            Answer = null;
            _user = user;
        }
        public MarketAnswer RemoveFromCart(string store, string product, double unitPrice)
        {
            MarketLog.Log("UserSpot", "User " + _user.SystemID + " attempting to remove his cart item: " + product + " from store: " + store + " ...");
            try
            {
                CartItem toRemove = ApproveModifyCart(store, product, unitPrice);

                MarketLog.Log("UserSpot", "User " + _user.SystemID + " found cart item: " + product + " from store: " + store + ". proceeding for the removal...");
                _user.Cart.RemoveFromCart(toRemove);
                MarketLog.Log("UserSpot", "User " + _user.SystemID + "successfully removed cart item: " + product + " from store: " + store + " ...");
                return new UserAnswer(RemoveFromCartStatus.Success, "Remove Cart Item has been successful!");
            }
            catch (UserException e)
            {
                MarketLog.Log("UserSpot",
                    "User " + _user.SystemID + " has failed to Edit Cart Item. Error message has been created!");
                return new UserAnswer((RemoveFromCartStatus)e.Status, e.GetErrorMessage());
            }
        }

        private void ApproveEnetered()
        {
            if (_user != null) { return; }
            throw new UserException(RemoveFromCartStatus.DidntEnterSystem,
                "Remove Cart Item action has been requested by User which hasn't fully entered the system yet!");

        }


        private CartItem ApproveModifyCart(string store, string product, double unitPrice)
        {
            ApproveEnetered();
            CartItem found = _user.Cart.SearchInCart(store, product, unitPrice);
            if (found != null)
            {
                return found;
            }
            throw new UserException(RemoveFromCartStatus.NoItemFound,
                "Remove Cart Item operation found no item to modify!");

        }
    }
}
