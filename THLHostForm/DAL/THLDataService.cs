using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DAL
{
    public class THLDataService
    {
        public bool AddTHLData(THLData thData)
        {
            string sql = "insert into THData ";
            sql += "(DTime,Humidity,Temperature,Light) ";
            sql += "values('{0}',{1},{2},{3})";
            sql = string.Format(sql, thData.DTime,thData.Humidity,thData.Temperature,thData.Light);
            try
            {
               return SQLHelper.Update(sql)==1;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public List<THLData> ShowThlData(DateTime startTime, DateTime endTime)
        {
            // 拼接时间范围条件
            string sql = $"SELECT DTime, Humidity, Temperature, Light FROM THData " +
                         $"WHERE DTime >= '{startTime:yyyy-MM-dd HH:mm:ss}' " +
                         $"AND DTime <= '{endTime:yyyy-MM-dd HH:mm:ss}' " +
                         $"ORDER BY DTime";

            List<THLData> list = new List<THLData>();

            try
            {
                var reader = SQLHelper.GetReader(sql);
                while (reader.Read())
                {
                    var data = new THLData
                    {
                        DTime = Convert.ToDateTime(reader["DTime"]),
                        Humidity = Convert.ToSingle(reader["Humidity"]),
                        Temperature = Convert.ToSingle(reader["Temperature"]),
                        Light = Convert.ToSingle(reader["Light"])
                    };
                    list.Add(data);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }

    }
}
