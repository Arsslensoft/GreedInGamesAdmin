using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace GAdmin
{
    public partial class CreateGtafrm : DevComponents.DotNetBar.Metro.MetroForm
    {
        public CreateGtafrm()
        {
            InitializeComponent();
        }

        private void textBoxX1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxX1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
                this.Close();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
             this.Close();
        }
        }
    }
