using System;
using System.Windows.Forms;
using DAL;
using Models;

namespace THLHostForm
{
    public partial class FrmAdminLogin : Form
    {
        AdminService objAdminService = new AdminService();
        public FrmAdminLogin()
        {
            InitializeComponent();
        }




        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUserId.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入账号！", "登录提示");
                txtUserId.Focus();
                return;
            }
            if (txtPwd.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入密码！", "登录提示");
                txtPwd.Focus();
                return;
            }
            var admin = new SysAdmin
            {
                UserId = txtUserId.Text.Trim(),
                PassWard = txtPwd.Text.Trim(),

            };
            try
            {
                var objAdmin = objAdminService.AdminLogin(admin);
                if (objAdmin != null)
                {
                    DialogResult = DialogResult.OK;
                    Program.AdminName = objAdmin.AdminName;
                    Close();
                }
                else
                {
                    MessageBox.Show("账号或密码错误！", "登录提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


    }
}
