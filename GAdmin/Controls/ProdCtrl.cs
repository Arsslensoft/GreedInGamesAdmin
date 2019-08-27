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
    public partial class ProdCtrl : UserControl
    {
        public ProdCtrl()
        {
            InitializeComponent();
          
        }
        public GProduct GetSelectedCommand()
        {
        
                if (listViewEx1.SelectedItems.Count > 0)
                    return (GProduct)listViewEx1.SelectedItems[0].Tag;
        
            return null;
        }


        public void AddCMD(GProduct cmd)
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
        public void Init()
        {
            try
            {
         
                    listViewEx1.Items.Clear();
                    Admins.SetSTAT("Recherche des produits...");
                    GProduct scmd = new GProduct();
                
                  //  AddCMD(scmd);

                    foreach (GProduct cmd in Admins.admin.GetProducts("1,30"))
                        AddCMD(cmd);


                    Admins.SetSTAT("Recherche terminé");
               
            }
            catch
            {

            }

        }
        private delegate void addMessageDelegate(GProduct cmd);
        private void addMessageToList(GProduct cmd)
        {
            try
            {
                ListViewItem item = this.listViewEx1.Items.Add(new ListViewItem(cmd.ProductID.ToString()));
                item.Tag = cmd;
      item.SubItems.Add(cmd.Name);
                item.SubItems.Add(cmd.Quantity.ToString());
                item.SubItems.Add(cmd.Price.ToString());
                item.SubItems.Add(cmd.Description);
                item.SubItems.Add(cmd.ProductType.ToString());

                item.SubItems.Add(cmd.ProductDate.ToString());
            

            }
            catch (Exception ex)
            {
                //GigSpace.LogError(ex);
            }
        }
    }
}
