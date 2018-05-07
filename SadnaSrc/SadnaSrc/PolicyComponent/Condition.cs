﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadnaSrc.PolicyComponent
{
    public abstract class Condition : PurchasePolicy
    {
        protected readonly string _value;

        protected Condition(PolicyType type, string subject, string value, int id) : base(type, subject, id)
        {
            _value = value;
        }
    }
}