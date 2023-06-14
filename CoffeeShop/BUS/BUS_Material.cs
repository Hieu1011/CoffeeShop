using CoffeeShop.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.BUS
{
    class BUS_Material
    {
        DAL_Material dalVL = new DAL_Material();
        public DataTable selectByName(List <String> name)
        {
            return dalVL.SelectByName(name);
        }
        public DataTable selectAll()
        {
            return dalVL.SelectAllMaterial();
        }
        public bool Create (String name, String unit)
        {
           return dalVL.Create(name, unit);
        }
        public bool Delete (String name)
        {
            return dalVL.Delete(name);
        }
        public bool Update(String oldName,String newName, String unit)
        {
            return dalVL.Update(oldName, newName, unit);
        }
    }
}
