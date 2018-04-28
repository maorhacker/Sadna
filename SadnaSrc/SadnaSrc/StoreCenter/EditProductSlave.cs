﻿using System;
using SadnaSrc.Main;
using SadnaSrc.MarketHarmony;

namespace SadnaSrc.StoreCenter
{
    internal class EditProductSlave : AbstractSlave
    {
        internal MarketAnswer answer;
        

        public EditProductSlave(string storename, IUserSeller storeManager) : base(storename, storeManager)
        {
        }

        internal void EditProduct(string productName, string whatToEdit, string newValue)
        {

            MarketLog.Log("StoreCenter", "trying to edit product in store");
            MarketLog.Log("StoreCenter", "check if store exists");
            
            try
            {
                checkIfStoreExistsAndActive();
                MarketLog.Log("StoreCenter", " store exists");
                MarketLog.Log("StoreCenter", " check if has premmision to edit products");
                _storeManager.CanManageProducts();
                MarketLog.Log("StoreCenter", " has premmission");
                MarketLog.Log("StoreCenter", " check if product name exists in the store " + _storeName);
                Product product = global.getProductByNameFromStore(_storeName, productName);
                checkifProductExists(product);
                checkIfEditName(whatToEdit, newValue, ref product);
                checkIfEditPrice(whatToEdit, newValue, ref product);
                checkIfEditDescription(whatToEdit, newValue, ref product);
                checkIfNoLegalFound();
                global.EditProductInDatabase(product);
            }
            catch (StoreException exe)
            {
                answer =  new StoreAnswer(exe);
            }
            catch (MarketException)
            {
                MarketLog.Log("StoreCenter", "no premission");
                answer = new StoreAnswer(StoreEnum.NoPremmision, "you have no premmision to do that");
            }
        }

        private void checkIfNoLegalFound()
        {
            if (answer == null)
            {
                MarketLog.Log("StoreCenter", "no leagal attrebute or founed non-leagal value");
                throw new StoreException(StoreEnum.UpdateProductFail, "no leagal attrebute found");
            }
        }

        private void checkIfEditDescription(string whatToEdit, string newValue, ref Product product)
        {
            if (whatToEdit == "Description" || whatToEdit == "desccription")
            {
                MarketLog.Log("StoreCenter", "edit description");
                answer = new StoreAnswer(StoreEnum.Success, "product " + product.SystemId + " Description has been updated to " + newValue);
                product.Description = newValue;
            }
        }

        private void checkIfEditPrice(string whatToEdit, string newValue, ref Product product)
        {
            if (whatToEdit == "BasePrice" || whatToEdit == "basePrice" || whatToEdit == "Baseprice" || whatToEdit == "baseprice")
            {
                MarketLog.Log("StoreCenter", "edit price");
                double newBasePrice;
                if (!double.TryParse(newValue, out newBasePrice))
                { throw new StoreException(StoreEnum.UpdateProductFail, "value is not leagal"); }
                if (newBasePrice <= 0) { throw new StoreException(StoreEnum.UpdateProductFail, "price can not be negative"); }
                answer = new StoreAnswer(StoreEnum.Success, "product " + product.SystemId + " price has been updated to " + newValue);
                product.BasePrice = newBasePrice;
            }
        }

        private void checkIfEditName(string whatToEdit, string newValue, ref Product product)
        {
            if (whatToEdit == "Name" || whatToEdit == "name")
            {
                MarketLog.Log("StoreCenter", "edit name");
                MarketLog.Log("StoreCenter", "checking if new new is avaliabe");
                Product P = global.getProductByNameFromStore(_storeName, product.Name);
                if (P == null)
                {
                    MarketLog.Log("StoreCenter", "name exists in shop");
                    throw new StoreException(StoreEnum.ProductNameNotAvlaiableInShop, "Product Name is already Exists In Shop");
                }
                answer = new StoreAnswer(StoreEnum.Success, "product " + product.SystemId + " name has been updated to " + newValue);
                product.Name = newValue;
            }
        }
        
    }
}