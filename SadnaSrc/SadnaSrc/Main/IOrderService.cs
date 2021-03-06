﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadnaSrc.OrderPool;
using SadnaSrc.UserSpot;

namespace SadnaSrc.Main
{
    public interface IOrderService
    {
        MarketAnswer BuyItemFromImmediate(string itemName, string store, int quantity, double unitPrice, string coupon);

        MarketAnswer BuyLotteryTicket(string itemName, string store, int quantity,double unitPrice);
        
        MarketAnswer BuyEverythingFromCart(string[] coupons);

        MarketAnswer GiveDetails(string userName, string address, string creditCard);

        void Cheat(int cheatResult);
    }

    public enum GiveDetailsStatus
    {
        Success,
        InvalidNameOrAddress,
        NoDB = 500
    }
    public enum OrderStatus
    {
        Success,
        InvalidUser,
        InvalidNameOrAddress,
        InvalidCoupon,
        NoDB = 500

    }

    public enum OrderItemStatus
    {
        Success,
        NoOrderItemInOrder,
        ItemAlreadyInOrder,
        InvalidDetails,
        NotComplyWithPolicy,
        NoDB = 500
    }

    public enum LotteryOrderStatus
    {
        Success,
        InvalidLotteryID,
        InvalidLotteryTicket,
        NoDB = 500
    }
}
