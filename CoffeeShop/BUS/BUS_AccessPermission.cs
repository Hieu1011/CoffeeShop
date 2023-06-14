using CoffeeShop.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.BUS
{
    class BUS_AccessPermission
    {
        DAL_AccessPermission dalAccPer = new DAL_AccessPermission();
        public DataTable GetAccessInfo(int limit, int offset)
        {
            return dalAccPer.GetAccessInfo(limit, offset);
        }

        public DataTable GetAccessInfoByEmployeeTypeName(string employeeTypeName)
        {
            return dalAccPer.GetAccessInfoByEmployeeTypeName(employeeTypeName);
        }

        public DataTable GetAccessInfoByEmployeeTypeID(string employeeTypeID)
        {
            return dalAccPer.GetAccessInfoByEmployeeTypeID(employeeTypeID);
        }

        public DataTable GetAccessPermission()
        {
            return dalAccPer.GetAccessPermissions();
        }    
    }
}
