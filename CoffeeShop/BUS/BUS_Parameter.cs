using CoffeeShop.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.BUS
{
    class BUS_Parameter
    {
        DAL_Parameter dalParameter = new DAL_Parameter();
        public int GetValue(string valueName)
        {
            return dalParameter.GetValue(valueName);
        }

        public bool SetValue(string valueName, int value)
        {
            return dalParameter.SetValue(valueName, value);
        }
    }
}
