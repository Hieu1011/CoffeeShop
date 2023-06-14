using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.DTO;
namespace CoffeeShop.DAL
{
    class DAL_Discount : DBConnect
    {
        public DataTable getAllDiscount()
        {
            string sql = $"select DiscountID , DiscountName , StartDate, EndDate, DiscountValue, Description  from discount order by enddate DESC";
            DataTable dt = new DataTable();
            dt = DataProvider.Instance.ExecuteQuery(sql);
            return dt;
        }
        public DTO_Discount findDiscount(string ID)
        {
            string sql = $"select * from discount where discountID ='" + ID + "'";
            DTO_Discount dto = new DTO_Discount();
            try
            {
                DataTable dt = DataProvider.Instance.ExecuteQuery(sql);

                dto.DiscountID = ID;
                dto.DiscountName = (string)dt.Rows[0]["DiscountName"].ToString();
                dto.StartDate = (string)dt.Rows[0]["StartDate"].ToString();
                dto.EndDate = (string)dt.Rows[0]["EndDate"].ToString();
                dto.DiscountValue = float.Parse((string)dt.Rows[0]["DiscountValue"].ToString());
                dto.Description = (string)dt.Rows[0]["Description"].ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return dto;
        }
        public DataTable findDiscount(string startdate, string enddate)
        {
            string sql = $"select DiscountID as 'Mã giảm giá', DiscountName as 'Tên ưu đãi', DiscountValue as 'Mức ưu đãi (%)', StartDate as 'Ngày bắt đầu', EndDate as 'Ngày kết thúc' from discount where startdate>=" + startdate + " and enddate<=" + enddate;
            
            DataTable dt = new DataTable();
            dt = DataProvider.Instance.ExecuteQuery(sql);
            return dt;
        }
        public DataTable findDiscount1(string ID)
        {
            string sql = $"select * from discount where discountName ='" + ID + "'";
            DataTable dt = null; ;
            try
            {
                dt = DataProvider.Instance.ExecuteQuery(sql);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return dt;
        }
        public int createNewDiscount(DTO_Discount dTO_Discount)
        {
            int result = 0;
            string sql = $"Insert into Discount (DiscountID, DiscountName, StartDate, EndDate, DiscountValue, Description) values ('" + createID() + "',N'" + dTO_Discount.DiscountName + "','" + dTO_Discount.StartDate + "','" + dTO_Discount.EndDate + "'," + dTO_Discount.DiscountValue + ",N'" + dTO_Discount.Description + "');";
            try
            {
                result = DataProvider.Instance.ExecuteNoneQuery(sql);
            }
            catch (Exception)
            {
                Console.WriteLine(sql);
            }
            return result;
        }
        public string createID()
        {
            string ID = "DC";
            string sql = "Select DiscountID from Discount";
            int max = 0;
            //SQLiteCommand sqlite = new SQLiteCommand(sql, getConnection());
            //sqlite.Connection.Open();
            //SQLiteDataReader reader = sqlite.ExecuteReader();
            //while (reader.Read())
            //{
            //    count = Int32.Parse(reader["DiscountID"].ToString().Remove(0, 2));
            //    if (count > max)
            //        max = count;
            //};
            DataTable dt = DataProvider.Instance.ExecuteQuery(sql);
            max = Int32.Parse(dt.Rows[dt.Rows.Count-1]["DiscountID"].ToString().Remove(0, 2));
            max++;
            ID = ID + max.ToString();
            return ID;
        }
        public int editDiscount(DTO_Discount dTO_Discount)
        {
            string sql = $"Update Discount set Discountname = N'" + dTO_Discount.DiscountName + "', StartDate = '" + dTO_Discount.StartDate + "', EndDate = '" + dTO_Discount.EndDate + "', DiscountValue = " + dTO_Discount.DiscountValue + ", Description = N'" + dTO_Discount.Description + "' Where DiscountID = '" + dTO_Discount.DiscountID + "'";
            int rs = 0;
            try
            {
                rs = DataProvider.Instance.ExecuteNoneQuery(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(sql);
            }
            return rs;
        }

        public int deleteDiscount(string discountID)
        {
            string sql = $"Delete from Discount where DiscountID = '" + discountID + "'";
          
            return DataProvider.Instance.ExecuteNoneQuery(sql);
        }

        public DTO_Discount GetCurrentDiscout()
        {
            DataTable discountData = getAllDiscount();
            DTO_Discount result = new DTO_Discount();
            foreach (DataRow row in discountData.Rows)
            {
                DateTime now = DateTime.Now.Date;
                try
                {
                    DateTime start = DateTime.ParseExact(row["StartDate"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).Date;
                    if (now < start)
                        continue;
                    DateTime end = DateTime.ParseExact(row["EndDate"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).Date;
                    if (now > end)
                        continue;
                    result = new DTO_Discount(row["DiscountID"].ToString(), row["DiscountName"].ToString(), row["StartDate"].ToString(), row["EndDate"].ToString(), float.Parse(row["DiscountValue"].ToString()), "");
                }
                catch
                {
                    continue;
                }
            }    
            return result;
        }    
    }
}