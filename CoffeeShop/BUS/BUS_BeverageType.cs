using CoffeeShop.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.DTO;
namespace CoffeeShop.BUS
{
    public class BUS_BeverageType
    {
        DAL_BeverageType dalBeverageType = new DAL_BeverageType();
       
        public List<DTO_BeverageType> findBeverageType(string type)
        {
            return dalBeverageType.findBeverageType(type);
        }
        public int createNewBeverageType(DTO_BeverageType BeverageType)
        {
            return dalBeverageType.createNewBeverageType(BeverageType);
        }
        public int editBeverageType(DTO_BeverageType BeverageType)
        {
            return dalBeverageType.editBeverageType(BeverageType);
        }
        public int deleteBeverageType(string ID)
        {
            return dalBeverageType.deleteBeverageType(ID);
        }
        public string createID()
        {
            return dalBeverageType.createID();
        }
        public List<DTO_BeverageType> getBeverageType()
        {
            return dalBeverageType.GetBeverageType();
        }
      
    }
}
