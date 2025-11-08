using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DAL
{
    public class AdminService
    {
        public SysAdmin AdminLogin(SysAdmin objAdmin)
        {
            string sql = "select * from Admins where UserId = '{0}' and Passward = '{1}'";
            sql = string.Format(sql, objAdmin.UserId,objAdmin.PassWard);
            try
            {
                var reader = SQLHelper.GetReader(sql);
                if (reader.Read())
                {
                    objAdmin.AdminName = reader["AdminsName"].ToString();
                    reader.Close();
                }
                else
                    objAdmin = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return objAdmin;
        }
    }
}
