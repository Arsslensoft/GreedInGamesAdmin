using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using GAdminLib;
using DevComponents.Editors;

namespace GAdmin
{
    public partial class UsrEditFrm : DevComponents.DotNetBar.Metro.MetroForm
    {
      public  GigUser user;
      public bool Done = false;
        public UsrEditFrm(GigUser u)
        {
            user = u;
            InitializeComponent();
            comboBoxEx1.SelectedItem = comboItem8;
            this.Text += u.Username;

            textBoxX2.Text = u.Email;
            textBoxX3.Text = u.Name;
            if(u.Role == GIGRoles.Utilisateur)
                comboBoxEx1.SelectedItem = comboItem8;
            else if (u.Role == GIGRoles.Moderateur)
                comboBoxEx1.SelectedItem = comboItem9;
            else if (u.Role == GIGRoles.Administrateur)
                comboBoxEx1.SelectedItem = comboItem10;
            else if (u.Role == GIGRoles.Directeur)
                comboBoxEx1.SelectedItem = comboItem11;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            user.Email = textBoxX2.Text;
            user.Name = textBoxX3.Text;
            user.Role = (GIGRoles)byte.Parse(((ComboItem)comboBoxEx1.SelectedItem).Value.ToString());
            Done = true;
            this.Close();
        }
    }
}