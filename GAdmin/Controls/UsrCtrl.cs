using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GAdminLib;
using DevComponents.DotNetBar.Controls;

namespace GAdmin.Controls
{
    public partial class UsrCtrl : UserControl
    {
        public UsrCtrl()
        {
            InitializeComponent();
          
        }
        public GigUser GetSelectedUser()
        {
        
                if (listViewEx1.SelectedItems.Count > 0)
                    return (GigUser)listViewEx1.SelectedItems[0].Tag;
        
            return null;
        }
        public void SetVisible(byte id)
        {
            if (id == 1)
            {
                // msg
                superTabItem1.Visible = true;
                superTabItem2.Visible = false;
                superTabItem3.Visible = false;
                superTabItem4.Visible = false;
                superTabItem6.Visible = false;
                superTabControl1.SelectedTab = superTabItem1;
            }
            else if (id == 2)
            {
                // amis
                superTabItem1.Visible = false;
                superTabItem2.Visible = true;
                superTabItem3.Visible = false;
                superTabItem4.Visible = false;
                superTabItem6.Visible = false;
                superTabControl1.SelectedTab = superTabItem2;
            }
            else if (id == 3)
            {
                // notif
                superTabItem1.Visible = false;
                superTabItem2.Visible = false;
                superTabItem3.Visible = true;
                superTabItem4.Visible = false;
                superTabItem6.Visible = false;
                superTabControl1.SelectedTab = superTabItem3;
            }
            else if (id == 4)
            {
                // logs
                superTabItem1.Visible = false;
                superTabItem2.Visible = false;
                superTabItem3.Visible = false;
                superTabItem4.Visible = true;
                superTabItem6.Visible = false;
                superTabControl1.SelectedTab = superTabItem4;
            }
            else if (id == 5)
            {
                // logs
                superTabItem1.Visible = false;
                superTabItem2.Visible = false;
                superTabItem3.Visible = false;
                superTabItem4.Visible = false;
                superTabItem6.Visible = true;
                superTabControl1.SelectedTab = superTabItem6;
            }
        }

        public void AddCMD(GigUser cmd)
        {
            try
            {

                if (this.listViewEx1.InvokeRequired)
                {
                    addMessageDelegate d = new addMessageDelegate(addMessageToList);
                 listViewEx1.Invoke(d, cmd);
                }
                else
                {
                    addMessageToList(cmd);
                }

            }
            catch (Exception ex)
            {
                //   GigSpace.LogError(ex);
            }
        }
        public void Init(string user)
        {
            try
            {
         
                    listViewEx1.Items.Clear();
                    Admins.SetSTAT("Recherche des produits...");
                    GigUser scmd = new GigUser();
                
                   // AddCMD(scmd);

                    foreach (KeyValuePair<string,string> f in Admins.admin.FindUser(user))
                    {
                        scmd = Admins.admin.GetUserInfo(f.Key);
                        AddCMD(scmd);
                    }

                    Admins.SetSTAT("Recherche terminé");
               
            }
            catch
            {

            }

        }
        private delegate void addMessageDelegate(GigUser cmd);
        private void addMessageToList(GigUser cmd)
        {
            try
            {
                ListViewItem item = this.listViewEx1.Items.Add(new ListViewItem(cmd.Username.ToString()));
                item.Tag = cmd;
      item.SubItems.Add(cmd.Name);
                item.SubItems.Add(cmd.Email   );
                item.SubItems.Add(cmd.Role.ToString());
                item.SubItems.Add(cmd.RegistrationDate.ToString());
            

            }
            catch (Exception ex)
            {
                //GigSpace.LogError(ex);
            }
        }
    }
}
