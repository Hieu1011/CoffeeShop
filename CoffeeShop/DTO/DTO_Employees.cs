using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DTO
{
    public class DTO_Employees
    {
        #region Atrributes
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeTypeID { get; set; }
        public string Password { get; set; }
        #endregion

        #region Methods
        public DTO_Employees()
        {
            EmployeeID = "";
            EmployeeName = "";
            EmployeeTypeID = "";
            Password = "";
        }
        public DTO_Employees(string ID, string name, string typeID, string pass)
        {
            EmployeeID = ID;
            EmployeeName = name;
            EmployeeTypeID = typeID;
            Password = pass;
        }
        #endregion
    }
}