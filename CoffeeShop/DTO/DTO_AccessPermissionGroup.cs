using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DTO
{
    public class DTO_AccessPermissionGroup
    {
        #region Atrributes
        public string AccessPermissionID { get; set; }
        public string EmployeeTypeID { get; set; }
        #endregion

        #region Method
        public DTO_AccessPermissionGroup()
        {
            AccessPermissionID = "";
            EmployeeTypeID = "";
        }
        public DTO_AccessPermissionGroup(string APID, string ETID)
        {
            AccessPermissionID = APID;
            EmployeeTypeID = ETID;
        }
        #endregion
    }
}
