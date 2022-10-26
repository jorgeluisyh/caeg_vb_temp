using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GeoSISGAQSACaddin.settings;

namespace GeoSISGAQSACaddin
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void tbx_user_GotFocus(object sender, EventArgs e)
        {
            if(tbx_user.Text == "USERNAME")
            {
                tbx_user.ForeColor = Color.Black;
                tbx_user.Text = "";
            }

        }

        private void tbx_user_LostFocus(object sender, EventArgs e)
        {
            if (tbx_user.Text == null)
            {
                tbx_user.ForeColor = Color.Gray;
                tbx_user.Text = "USERNAME";
            }
        }

        private void tbx_pass_GotFocus(object sender, EventArgs e)
        {
            if (tbx_pass.Text == "PASSWORD")
            {
                tbx_pass.UseSystemPasswordChar = true;
                tbx_pass.ForeColor = Color.Black;
                tbx_pass.Text = "";
            }

        }

        private void tbx_pass_LostFocus(object sender, EventArgs e)
        {
            if (tbx_pass.Text == null)
            {
                tbx_pass.UseSystemPasswordChar = false;
                tbx_pass.ForeColor = Color.Gray;
                tbx_pass.Text = "PASSWORD";
            }
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            lbl_login.Text = "";
            tbx_user.Enabled = false;
            tbx_pass.Enabled = false;
            btn_login.Enabled = false;
            

            if (iniciar_sesion())
            {
                modulosDict.Add(1, "Generar Plano");
                Form Modulosform = new Modulos();
                openFormByName(Modulosform, this.Parent);
                lbl_login.Text = "";
            }
            tbx_user.Enabled = true;
            tbx_pass.Enabled = true;
            btn_login.Enabled = true;
            lbl_login.Text = "Usuario o contraseña incorrectos";
            lbl_login.ForeColor = Color.Red;
        }

        private bool iniciar_sesion()
        {
            if ((tbx_user.Text =="admin") && (tbx_pass.Text =="admin"))
            {
                return true;
            }
            return false;
        }
    }
}
