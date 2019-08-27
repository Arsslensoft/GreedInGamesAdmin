using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar;

namespace GAdmin
{
    public partial class LoginFrm : MetroForm
    {
        public LoginFrm()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            try
            {

                close = true;
                this.Close();
                if (string.IsNullOrEmpty(textBoxX1.Text) || string.IsNullOrEmpty(textBoxX2.Text) || (textBoxX1.Text + textBoxX2.Text).Contains("&") || (textBoxX1.Text + textBoxX2.Text).Contains("=") || (textBoxX1.Text + textBoxX2.Text).Contains("?"))
                    MessageBoxEx.Show("Enter a valid username and password", "LOGIN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                  
                    if (Admins.admin.Login(textBoxX1.Text, textBoxX2.Text))
                    {
                        close = true;
                        this.Close();

                    }
                    else
                        MessageBoxEx.Show("Login Failed : " + Admins.admin.ErrorMSG, "LOGIN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
               
              
            }
            catch
            {

            }
        }
       internal bool close = false;
        private void LoginFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !close;
        }

        private void LoginFrm_Shown(object sender, EventArgs e)
        {
         
        }

        private void LoginFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
          
        }

        private void LoginFrm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {

                if (e.KeyCode == Keys.Enter)
                {
                    if (string.IsNullOrEmpty(textBoxX1.Text) || string.IsNullOrEmpty(textBoxX2.Text) || (textBoxX1.Text + textBoxX2.Text).Contains("&") || (textBoxX1.Text + textBoxX2.Text).Contains("=") || (textBoxX1.Text + textBoxX2.Text).Contains("?"))
                        MessageBoxEx.Show("Enter a valid username and password", "LOGIN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {

                        if (Admins.admin.Login(textBoxX1.Text, textBoxX2.Text))
                        {
                            close = true;
                            this.Close();

                        }
                        else
                            MessageBoxEx.Show("Login Failed : " + Admins.admin.ErrorMSG, "LOGIN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch
            {

            }

        }
    }
}
