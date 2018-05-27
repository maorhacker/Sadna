﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadnaSrc.MarketHarmony;

namespace SadnaSrc.StoreCenter
{
    public interface IStoreDL
    {
        Store GetStorebyID(string storeID);
        void AddProductToDatabase(Product product);
        void EditProductInDatabase(Product product);
        int GetUserIDFromUserName(string userName);
        Store GetStorebyName(string storeName);
        LinkedList<LotteryTicket> GetAllTickets(string systemID);
        StockListItem GetStockListItembyProductID(string product);
        Discount GetDiscount(string discountCode);
        bool IsStoreExistAndActive(string store);
        string[] GetStoreInfo(string store);
        string[] GetStoreStockInfo(string store);
        void AddLotteryTicket(LotteryTicket lottery);
        LotteryTicket GetLotteryTicket(string ticketID);
        void EditLotteryTicketInDatabase(LotteryTicket lotter);
        void RemoveLotteryTicket(LotteryTicket lottery);
        LotterySaleManagmentTicket GetLotteryByProductID(string productID);
        void EditLotteryInDatabase(LotterySaleManagmentTicket lotteryManagment);
        void AddLottery(LotterySaleManagmentTicket lotteryManagment);
        string[] GetAllStoreProductsID(string systemID);
        Product GetProductID(string iD);
        void RemoveLottery(LotterySaleManagmentTicket lotteryManagment);
        void RemoveStockListItem(StockListItem stockListItem);
        void EditDiscountInDatabase(Discount discount);
        void EditStore(Store store);
        void RemoveProduct(Product product);
        void EditStockInDatabase(StockListItem stockListItem);
        void RemoveStore(Store store);
        void AddStore(Store toAdd);
        string[] GetHistory(Store store);
        void AddDiscount(Discount discount);
        void AddStockListItemToDataBase(StockListItem stockListItem);
        void RemoveDiscount(Discount discount);
        Product GetProductByNameFromStore(string storeName, string ProductName);
        StockListItem GetProductFromStore(string store, string productName);
        LotterySaleManagmentTicket GetLotteryByProductNameAndStore(string storeName, string productName);
        string[] GetAllCategorysIDs();
        Category GetCategoryByName(string categoryName);
        LinkedList<Product> GetAllCategoryProducts(string categoryid);
        void AddProductToCategory(string CategoryID, string ProductID);
        void RemoveProductFromCategory(string CategoryID, string ProductID);
        CategoryDiscount GetCategoryDiscount(string categoryName, string storeName);
        void AddCategoryDiscount(CategoryDiscount discount);
    }
    }
