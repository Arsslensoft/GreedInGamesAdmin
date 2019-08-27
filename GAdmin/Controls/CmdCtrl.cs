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
    public partial class CmdCtrl : UserControl
    {
        public CmdCtrl()
        {
            InitializeComponent();
            cur = listViewEx1;
        }
        public GigCommand GetSelectedCommand()
        {
            if (superTabControl1.SelectedTab == superTabItem2)
            {
                if (listViewEx1.SelectedItems.Count > 0)
                    return (GigCommand)listViewEx1.SelectedItems[0].Tag;
                

            }
            else if (superTabControl1.SelectedTab == superTabItem3)
            {
                if (listViewEx2.SelectedItems.Count > 0)
                    return (GigCommand)listViewEx2.SelectedItems[0].Tag;
            }
            else if (superTabControl1.SelectedTab == superTabItem4)
            {
                if (listViewEx3.SelectedItems.Count > 0)
                    return (GigCommand)listViewEx3.SelectedItems[0].Tag;
            }
            else if (superTabControl1.SelectedTab == superTabItem5)
            {
                if (listViewEx4.SelectedItems.Count > 0)
                    return (GigCommand)listViewEx4.SelectedItems[0].Tag;
            }

            return null;
        }
        public byte GetSelectedType()
        {
            if (superTabControl1.SelectedTab == superTabItem2)
                return 1;
            else if (superTabControl1.SelectedTab == superTabItem3)
                return 2;
            else if (superTabControl1.SelectedTab == superTabItem4)
                return 3;
            else if (superTabControl1.SelectedTab == superTabItem5)
                return 4;
            else
                return 0;

        }
        public string GetSelectedTypeString()
        {
            if (superTabControl1.SelectedTab == superTabItem2)
                return "item";
            else if (superTabControl1.SelectedTab == superTabItem3)
                return "hcp";
            else if (superTabControl1.SelectedTab == superTabItem4)
                return "sell";
            else if (superTabControl1.SelectedTab == superTabItem5)
                return "skins";
            else
                return "";

        }
        ListViewEx cur ;
        public void AddCMD(GigCommand cmd)
        {
            try
            {

                if (this.cur.InvokeRequired)
                {
                    addMessageDelegate d = new addMessageDelegate(addMessageToList);
                    cur.Invoke(d, cmd);
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
        public void Init(byte typ)
        {
            try
            {
                if (typ == 1)
                {
                    listViewEx1.Items.Clear();
                    Admins.SetSTAT("Recherche des commandes...");
                    cur = listViewEx1;

                    foreach (GigCommand cmd in Admins.admin.GetCommands("item"))
                        this.AddCMD(cmd);


                    Admins.SetSTAT("Recherche terminé");
                }
                else if (typ == 2)
                {
                    // hcp cmd
                    listViewEx1.Items.Clear();
                    Admins.SetSTAT("Recherche des commandes...");
                    cur = listViewEx2;

                    foreach (GigCommand cmd in Admins.admin.GetCommands("hcp"))
                        this.AddCMD(cmd);


                    Admins.SetSTAT("Recherche terminé");
                }
                else if (typ == 3)
                {
                    // sell hcp
                    listViewEx1.Items.Clear();
                    Admins.SetSTAT("Recherche des commandes...");
                    cur = listViewEx3;

                    foreach (GigCommand cmd in Admins.admin.GetCommands("sell"))
                        this.AddCMD(cmd);


                    Admins.SetSTAT("Recherche terminé");
                }
                else if (typ == 4)
                {
                    // Skin
                    listViewEx1.Items.Clear();
                    Admins.SetSTAT("Recherche des commandes...");
                    cur = listViewEx4;

                    foreach (GigCommand cmd in Admins.admin.GetCommands("skins"))
                        this.AddCMD(cmd);


                    Admins.SetSTAT("Recherche terminé");
                }
            }
            catch
            {

            }

        }
        private delegate void addMessageDelegate(GigCommand cmd);
        private void addMessageToList(GigCommand cmd)
        {
            try
            {
                ListViewItem item = cur.Items.Add(new ListViewItem(cmd.SID));
                item.Tag = cmd;
                item.SubItems.Add(cmd.Username);
                item.SubItems.Add(cmd.GTAUsername);
                item.SubItems.Add(cmd.Name);
                item.SubItems.Add(cmd.Email);
                item.SubItems.Add(cmd.Pack);
                item.SubItems.Add(cmd.TimeStamp.ToString());

                item.SubItems.Add(cmd.Price.ToString());
                item.SubItems.Add(cmd.Option);
                item.SubItems.Add(cmd.Status.ToString());

            }
            catch (Exception ex)
            {
                //GigSpace.LogError(ex);
            }
        }
    }
}
