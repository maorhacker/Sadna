﻿using System.Collections.Generic;
using SadnaSrc.Main;

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

        public static string GetStoreActionString(StoreAction action)
        {
            switch (action)
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

        public static string GetStoreActionName(StoreAction action)
        {
            switch (action)
            {
                case StoreAction.StoreOwner:
                    return "StoreOwner";
                case StoreAction.PromoteStoreAdmin:
                    return "Promote Store Admins";
                case StoreAction.ManageProducts:
                    return "ManageProducts";
                case StoreAction.DeclarePurchasePolicy:
                    return "Manage Store Purchase-Policy";
                case StoreAction.DeclareDiscountPolicy:
                    return "Manage Store Discounts";
                default:
                    return "View Purchase History";
            }
        }
        public static StoreAction GetActionFromString(string actionString)
        {
            switch (actionString)
            {
                case "StoreOwner": return StoreAction.StoreOwner;
                case "PromoteStoreAdmin": return StoreAction.PromoteStoreAdmin;
                case "ManageProducts": return StoreAction.ManageProducts;
                case "DeclareDiscountPolicy": return StoreAction.DeclareDiscountPolicy;
                case "DeclarePurchasePolicy": return StoreAction.DeclarePurchasePolicy;
                case "ViewPurchaseHistory": return StoreAction.ViewPurchaseHistory;
                default:
                    throw new UserException(PromoteStoreStatus.InvalidPromotion,
                        "Procedure to cast string into store action has failed! there is no state by that name...");
            }
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
