﻿using System;
using SadnaSrc.Main;

namespace BlackBox.OrderBlackBoxTests
{
	class ProxyOrderBridge :IOrderBridge
	{
		public IOrderBridge real;

		public void GetOrderService(IUserService userService)
		{
			if (real != null)
			{
				real.GetOrderService(userService);
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		public MarketAnswer BuyItemFromImmediate(string itemName, string store, int quantity, double unitPrice)
		{
			if (real != null)
			{
				return real.BuyItemFromImmediate(itemName, store, quantity, unitPrice);
			}
			throw new NotImplementedException();
		}

		public MarketAnswer BuyEverythingFromCart()
		{
			if (real != null)
			{
				return real.BuyEverythingFromCart();
			}
			throw new NotImplementedException();
		}

		public MarketAnswer BuyItemWithCoupon(string itemName, string store, int quantity, double unitPrice, string coupon)
		{
			if (real != null)
			{
				return real.BuyItemWithCoupon(itemName, store, quantity, unitPrice, coupon);
			}
			throw new NotImplementedException();
		}


		public MarketAnswer GiveDetails(string userName, string address, string creditCard)
		{
			if (real != null)
			{
				return real.GiveDetails(userName, address, creditCard);
			}
			throw new NotImplementedException();
		}

		public void DisableSupplySystem()
		{
			if (real != null)
			{
				real.DisableSupplySystem();
			}

			else
			{
				throw new NotImplementedException();
			}
		}

		public void DisablePaymentSystem()
		{
			if (real != null)
			{
				real.DisablePaymentSystem();
			}

			else
			{
				throw new NotImplementedException();
			}
		}

		public void EnableSupplySystem()
		{
			if (real != null)
			{
				real.EnableSupplySystem();
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		public void EnablePaymentSystem()
		{
			if (real != null)
			{
				real.EnablePaymentSystem();
			}
			else
			{
				throw new NotImplementedException();
			}
		}


		public void CleanSession()
		{
			if (real != null)
			{
				real.CleanSession();
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}
