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
    class BUS_Employees
    {
        DAL_Employees dalEmp = new DAL_Employees();
        public string GetPasswordByID(string ID)
        {
            return dalEmp.GetPasswordByID(ID);
        }

        public int CountEmployees()
        {
            return dalEmp.CountEmployees();
        }

        public DataTable GetEmployees(int limit, int offset)
        {
            return dalEmp.GetEmployees(limit, offset);
        }
        
        public bool CreateEmployee(DTO_Employees newEmp)
        {
            return dalEmp.CreateEmployee(newEmp);
        }    

        public bool EditEmployee(DTO_Employees editEmp)
        {
            return dalEmp.EditEmployee(editEmp);
        }

        public bool EditPassword(string id, string newPass)
        {
            return dalEmp.EditPassword(id, newPass);
        }

        public bool DeleteEmployee(string deleteEmpID)
        {
            return dalEmp.Delete(deleteEmpID);
        }

        public bool SetState(string deleteEmpID, bool state)
        {
            return dalEmp.SetState(deleteEmpID, state);
        }

        public string GetEmpTypeByID(string id)
        {
            return dalEmp.GetEmpTypeByID(id);
        }

        public string GetEmpNameByID(string id)
        {
            return dalEmp.GetEmpNameByID(id);
        }

        public DTO_Employees GetEmpByID(string id)
        {
            return dalEmp.GetEmpByID(id);
        }

        public bool IsDoingAnything(string id)
        {
            return dalEmp.IsDoingAnything(id);
        }

        public int CountEmployeesByTypeID(string id)
        {
            return dalEmp.CountEmployeesByTypeID(id);
        }    
    }
}
