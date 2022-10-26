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
    public partial class Modulos : Form
    {
        public Modulos()
        {
            InitializeComponent();
        }

        private void btn_cerrar_seion_Click(object sender, EventArgs e)
        {
            DialogResult response = MessageBox.Show("¿Está seguro que desea cerrar sesión?", __title__, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (response == DialogResult.No)
                return;
            Form LoginForm = new Login();
            openFormByName(LoginForm, this.Parent);
            modulosDict.Clear();
        }

        private void cbx_modulos_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentModule = ((KeyValuePair<int, string>)cbx_modulos.SelectedItem).Key;
            currentModuleName = ((KeyValuePair<int, string>)cbx_modulos.SelectedItem).Value;

            if (currentModule == 1)
            {
                Form ajustesiniestroForm = new formAjusteSiniestros();
                openFormByName(ajustesiniestroForm, pnl_modulos_form);
            }
        }

        private void Modulos_Load(object sender, EventArgs e)
        {
            cbx_modulos.SelectedIndexChanged -= cbx_modulos_SelectedIndexChanged;
            cbx_modulos.DataSource = new BindingSource(modulosDict, null);
            cbx_modulos.DisplayMember = "Value";
            cbx_modulos.ValueMember = "Key";
            cbx_modulos.SelectedIndexChanged += cbx_modulos_SelectedIndexChanged;
            // cbx_modulos.SelectedIndex = 0
            cbx_modulos_SelectedIndexChanged(sender, e);
            _LOADER_CONTROL = this.pgb_progress;
        }
    }
}
