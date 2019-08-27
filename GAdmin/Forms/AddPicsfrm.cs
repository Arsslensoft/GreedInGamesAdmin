using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using GAdminLib;

namespace GAdmin
{
    public partial class AddPicsfrm : DevComponents.DotNetBar.Metro.MetroForm
    {
        public bool Done = false;
        public AddPicsfrm(GProduct prod, bool mod)
        {
            InitializeComponent();
            this.Text = "Modification des images du produit ID_" + prod.ProductID.ToString();
        }
        public AddPicsfrm(GProduct prod)
        {
            InitializeComponent();
            this.Text += " ID_" + prod.ProductID.ToString();
        }
        private void buttonX1_Click(object sender, EventArgs e)
        {
            Done = true;
            this.Close();
        }
    }
}