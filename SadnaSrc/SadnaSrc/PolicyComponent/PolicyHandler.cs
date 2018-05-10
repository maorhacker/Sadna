﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SadnaSrc.Main;

namespace SadnaSrc.PolicyComponent
{
    public class PolicyHandler : IGlobalPolicyManager, IStorePolicyManager, IPolicyChecker
    {
        public List<PurchasePolicy> Policies;
        private List<PurchasePolicy> SessionPolicies;

        private static PolicyHandler _instance;
        private static IPolicyDL _dataLayer;

        public PolicyType Type;
        public string Subject;

        public static PolicyHandler Instance => _instance ?? (_instance = new PolicyHandler(PolicyDL.Instance));

        private PolicyHandler(IPolicyDL dataLayer)
        {
           // Policies = new List<PurchasePolicy>();
            SessionPolicies = new List<PurchasePolicy>();
            _dataLayer = dataLayer;
            Policies = dataLayer.GetAllPolicies();
        }

        public string[] CreateGlobalSimplePolicy(ConditionType cond, string value)
        {
            return CreateCondition(PolicyType.Global, null, cond, value).GetData();
        }

        public string[] CreateCategorySimplePolicy(string category, ConditionType cond, string value)
        {
            return CreateCondition(PolicyType.Category, category, cond, value).GetData();
        }

        public string[] CreateProductSimplePolicy(string product, ConditionType cond, string value)
        {
            return CreateCondition(PolicyType.Product, product, cond, value).GetData();
        }

        public string[] CreateStoreSimplePolicy(string store, ConditionType cond, string value)
        {
            return CreateCondition(PolicyType.Store, store, cond, value).GetData();
        }

        public string[] CreateStockItemSimplePolicy(string store, string product, ConditionType cond, string value)
        {
            return CreateCondition(PolicyType.StockItem, store + "." + product, cond, value).GetData();
        }

        public string[] CreateGlobalPolicy(OperatorType op, int id1, int id2)
        {
            return CreatePolicy(PolicyType.Global, null, op, id1, id2).GetData();
        }

        public string[] CreateCategoryPolicy(string category, OperatorType op, int id1, int id2)
        {
            return CreatePolicy(PolicyType.Category, category, op, id1, id2).GetData();
        }

        public string[] CreateProductPolicy(string product, OperatorType op, int id1, int id2)
        {
            return CreatePolicy(PolicyType.Product, product, op, id1, id2).GetData();
        }

        public string[] CreateStorePolicy(string store, OperatorType op, int id1, int id2)
        {
            return CreatePolicy(PolicyType.Store, store, op, id1, id2).GetData();
        }

        public string[] CreateStockItemPolicy(string store, string product, OperatorType op, int id1, int id2)
        {
            return CreatePolicy(PolicyType.StockItem, store + "." + product, op, id1, id2).GetData();
        }

        public void AddPolicy(int policyId)
        {
            PurchasePolicy toAdd = GetPolicy(policyId);
            toAdd.ID = GeneratePolicyID();
            Policies.Add(toAdd);
            SessionPolicies.Clear();
            _dataLayer.SavePolicy(toAdd);
        }

        public void RemovePolicy(PolicyType type, string subject)
        {
            PurchasePolicy toRemove = null;
            foreach (PurchasePolicy policy in Policies)
            {
                if (policy.Type == type && policy.Subject == subject)
                {
                    toRemove = policy;
                   
                }
                    
            }
            _dataLayer.RemovePolicy(toRemove);
            Policies.Remove(toRemove);
        }

        public void RemoveSessionPolicy(int policyId)
        {
            PurchasePolicy toRemove = null;
            foreach (PurchasePolicy policy in SessionPolicies)
            {
                if (policy.ID == policyId)
                    toRemove = policy;
            }
            SessionPolicies.Remove(toRemove);
        }

        public bool CheckRelevantPolicies(string product, string store, string category, string username,
            string address, int quantity, double price)
        {
            return
            CheckPolicy(PolicyType.Global, null, username, address, quantity, price) &&
            CheckPolicy(PolicyType.Category, category, username, address, quantity, price) &&
            CheckPolicy(PolicyType.Product, product, username, address, quantity, price) &&
            CheckPolicy(PolicyType.Store, store, username, address, quantity, price) &&
            CheckPolicy(PolicyType.StockItem, store + "." + product, username, address, quantity, price);
        }

        public int[] GetSessionPolicies()
        {
            PurchasePolicy[] policiesArr = SessionPolicies.ToArray();
            int[] idArr = new int[policiesArr.Length];
            for (int i = 0; i < idArr.Length; i++)
                idArr[i] = policiesArr[i].ID;
            return idArr;
        }

        public string[] GetSessionPoliciesStrings()
        {
            int[] idArr = GetSessionPolicies();
            return idArr.Select(x => x.ToString()).ToArray();
        }

        public string[] GetPolicyData(PolicyType type, string subject)
        {
            foreach (PurchasePolicy policy in Policies)
            {
                if (policy.Type == type && policy.Subject == subject && policy.IsRoot)
                    return policy.GetData();
            }

            return null;
        }

        public string[] PolicyTypeStrings()
        {
            return new[] {"Global", "Store", "Stock Item", "Product", "Category"};
        }

        public string[] OperatorTypeStrings()
        {
            return new[] {"AND", "OR", "NOT"};
        }

        public string[] ConditionTypeStrings()
        {
            return new[] {"Price >=", "Price <=", "Quantity >=", "Quantity <=", "Username =", "Address ="};
        }

        private PurchasePolicy GetPolicy(PolicyType type, string subject)
        {
            foreach (PurchasePolicy policy in Policies)
            {
                if (policy.Type == type && policy.Subject == subject)
                    return policy;
            }

            return null;
        }

        private PurchasePolicy GetPolicy(int id)
        {
            foreach (PurchasePolicy policy in SessionPolicies)
            {
                if (policy.ID == id)
                    return policy;
            }

            return null;
        }

        private bool CheckPolicy(PolicyType type, string subject, string username, string address, int quantity, double price)
        {
            PurchasePolicy policy = GetPolicy(type, subject);
            if (policy == null) return true;
            return policy.Evaluate(username, address, quantity, price);
        }

        private int GeneratePolicyID()
        {
            Random random = new Random();
            var newID = random.Next(1000, 10000);
            while (GetPolicy(newID) != null)
            {
                newID = random.Next(1000, 10000);
            }

            return newID;
        }

        private PurchasePolicy CreatePolicy(PolicyType type, string subject, OperatorType op, int id1, int id2)
        {
            PurchasePolicy policy = null;
            switch (op)
            {
                case OperatorType.AND:
                    policy = new AndOperator(type, subject, GetPolicy(id1), GetPolicy(id2), SessionPolicies.Count);
                    break;
                case OperatorType.OR:
                    policy = new OrOperator(type, subject, GetPolicy(id1), GetPolicy(id2), SessionPolicies.Count);
                    break;
                case OperatorType.NOT:
                    policy = new NotOperator(type, subject, GetPolicy(id1), null, SessionPolicies.Count);
                    break;
            }
            SessionPolicies.Add(policy);
            return policy;
        }

        private PurchasePolicy CreateCondition(PolicyType type, string subject, ConditionType cond, string value)
        {
            PurchasePolicy policy = null;
            switch (cond)
            {
                case ConditionType.AddressEqual:
                    policy = new AddressEquals(type, subject, value, SessionPolicies.Count);
                    break;
                case ConditionType.PriceGreater:
                    policy = new PriceGreaterThan(type, subject, value, SessionPolicies.Count);
                    break;
                case ConditionType.PriceLesser:
                    policy = new PriceLessThan(type, subject, value, SessionPolicies.Count);
                    break;
                case ConditionType.QuantityGreater:
                    policy = new QuantityGreaterThan(type, subject, value, SessionPolicies.Count);
                    break;
                case ConditionType.QuantityLesser:
                    policy = new QuantityLessThan(type, subject, value, SessionPolicies.Count);
                    break;
                case ConditionType.UsernameEqual:
                    policy = new UsernameEquals(type, subject, value, SessionPolicies.Count);
                    break;
            }
            SessionPolicies.Add(policy);
            return policy;
        }

        public void CleanSession()
        {
            Policies.Clear();
            SessionPolicies.Clear();
        }
    }
}
