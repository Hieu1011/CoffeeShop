using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DTO
{
    public class DTO_AccessPermission
    {
        #region Atrributes
        public string AccessPermissionID { get; set; }
        public string AccessPermissionName { get; set; }
        #endregion

        #region Method
        public DTO_AccessPermission()
        {
            AccessPermissionID = "";
            AccessPermissionName = "";
        }
        public DTO_AccessPermission(string ID, string name)
        {
            AccessPermissionID = ID;
            AccessPermissionName = name;
        }
        #endregion
    }
}
