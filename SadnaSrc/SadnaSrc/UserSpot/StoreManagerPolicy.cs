﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadnaSrc.Main;
using SadnaSrc.MarketHarmony;

namespace SadnaSrc.UserSpot
{
    public class StoreManagerPolicy
    {
        public enum StoreAction { StoreOwner,PromoteStoreAdmin, ManageProducts, DeclarePurchasePolicy, DeclareDiscountPolicy, ViewPurchaseHistory}
        public string Store { get; }
        public StoreAction Action { get; }
        public StoreManagerPolicy(string store, StoreAction storeAction)
        {
            Store = store;
            Action = storeAction;
        }

        public string GetStoreActionString()
        {
            switch (Action)
            {
                case StoreAction.StoreOwner:
                    return "StoreOwner";
                case StoreAction.PromoteStoreAdmin:
                    return "PromoteStoreAdmin";
                case StoreAction.ManageProducts:
                    return "ManageProducts";
                case StoreAction.DeclarePurchasePolicy:
                    return "DeclarePurchasePolicy";
                case StoreAction.DeclareDiscountPolicy:
                    return "DeclareDiscountPolicy";
                default:
                    return "ViewPurchaseHistory";
            }
        }
        public static StoreAction GetActionFromString(string actionString)
        {
            if (actionString.Equals("StoreOwner"))
            {
                return StoreAction.StoreOwner;
            }

            if (actionString.Equals("PromoteStoreAdmin"))
            {
                return StoreAction.PromoteStoreAdmin;
            }

            if (actionString.Equals("ManageProducts"))
            {
                return StoreAction.ManageProducts;
            }

            if (actionString.Equals("DeclareDiscountPolicy"))
            {
                return StoreAction.DeclareDiscountPolicy;
            }

            if (actionString.Equals("DeclarePurchasePolicy"))
            {
                return StoreAction.DeclarePurchasePolicy;
            }

            if (actionString.Equals("ViewPurchaseHistory"))
            {
                return StoreAction.ViewPurchaseHistory;
            }

            throw new UserException(PromoteStoreStatus.InvalidPromotion, "Procedure to cast string into store action has failed! there is no state by that name...");
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((StoreManagerPolicy)obj);
        }


        private bool Equals(StoreManagerPolicy obj)
        {
            return obj.Store.Equals(Store) && obj.Action == Action;
        }

        public override int GetHashCode()
        {
            var hashCode = -1019910883;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Store);
            hashCode = hashCode * -1521134295 + Action.GetHashCode();
            return hashCode;
        }
    }
}
