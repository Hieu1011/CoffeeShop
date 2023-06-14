using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DTO
{
    public class DTO_EmployeeType
    {
        #region Atrributes
        public string EmployeeTypeID { get; set; }
        public string EmployeeTypeName { get; set; }
        #endregion

        #region Method
        public DTO_EmployeeType()
        {
            EmployeeTypeID = "";
            EmployeeTypeName = "";
        }
        public DTO_EmployeeType(string ID, string name)
        {
            EmployeeTypeID = ID;
            EmployeeTypeName = name;
        }
        #endregion
    }
}
