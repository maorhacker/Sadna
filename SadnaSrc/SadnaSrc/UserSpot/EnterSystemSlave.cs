﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SadnaSrc.Main;

namespace SadnaSrc.UserSpot
{
    public class EnterSystemSlave
    {

        private IUserDL _userDB;
        public UserAnswer Answer { get; private set; }

        private static readonly Random random = new Random();

        public EnterSystemSlave(IUserDL userDB)
        {
            _userDB = userDB;
            Answer = null;
        }
        public User EnterSystem()
        {
            MarketLog.Log("UserSpot", "New User attempting to enter the system...");
            User newGuest  = new User(GenerateSystemID());
            MarketLog.Log("UserSpot", "User " + newGuest.SystemID + " has entered the system! " +
                                      "attempting to save the user entry...");
            _userDB.SaveUser(newGuest);
            MarketLog.Log("UserSpot", "User " + newGuest.SystemID + " has been saved successfully as new " +
                                      "guest entry in the system!");
            Answer = new UserAnswer(EnterSystemStatus.Success, "You've been entered the system successfully!");
            return newGuest;
        }

        private int GenerateSystemID()
        {
            var newID = random.Next(1000, 10000);
            int[] savedIDs = _userDB.GetAllSystemIDs();
            while (savedIDs.Contains(newID))
            {
                newID = random.Next(1000, 10000);
            }

            return newID;
        }
    }
}
