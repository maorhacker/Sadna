﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadnaSrc.MarketHarmony
{
    class OrderSyncherHarmony
    {
        //integration from StoreShoppingService(StoreCenter module) to OrderPool module
        public OrderSyncherHarmony()
        {

        }

        void BuyLotteryTicket(string buyerName, string creditCard, double priceToPay)
        {
            // should not catch OrderException if it occurs(and it should if something wrong with the payment system or the ticket).
            //its the StoreShoppingService responsibility
        }
    }
}