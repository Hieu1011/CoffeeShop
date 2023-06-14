using CoffeeShop.DAL;
using CoffeeShop.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.BUS
{
    class BUS_EmployeeType
    {
        DAL_EmployeeType dalEmpType = new DAL_EmployeeType();

        public DataTable GetEmployeeTypes()
        {
            return dalEmpType.GetEmployeeTypes();
        }

        public int CountEmployeeTypes()
        {
            return dalEmpType.CountEmployeeTypes();
        }

        public string CreateEmployeeTypes(DTO_EmployeeType newEmpType)
        {
            return dalEmpType.CreateEmployeeType(newEmpType);
        }

        public string GetNameByID(string id)
        {
            return dalEmpType.GetNameByID(id);
        }

        public string GetIDByName(string name)
        {
            return dalEmpType.GetIDByName(name);
        }

        public int DeleteEmployeeType(string id)
        {
            return dalEmpType.Delete(id);
        }

        public bool EditEmployeeType(DTO_EmployeeType editEmpType)
        {
            return dalEmpType.EditEmployeeType(editEmpType);
        }    
    }
}
