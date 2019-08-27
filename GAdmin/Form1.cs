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
    public partial class Form1 : DevComponents.DotNetBar.Metro.MetroAppForm
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        public void SetControl()
        {
            if (this.metroShell1.SelectedTab == metroTabItem1)
            {
                foreach (Control ct in panelEx1.Controls)
                    ct.Visible = false;
                panelEx1.Controls.Clear();

                cmdCtrl1.Dock = DockStyle.Fill;
                cmdCtrl1.Visible = true;
                panelEx1.Controls.Add(cmdCtrl1);
                panelEx1.Refresh();
            }
            else if (this.metroShell1.SelectedTab == metroTabItem2)
            {
                foreach (Control ct in panelEx1.Controls)
                    ct.Visible = false;
                panelEx1.Controls.Clear();

                prodCtrl1.Dock = DockStyle.Fill;
                prodCtrl1.Visible = true;
                panelEx1.Controls.Add(prodCtrl1);
                panelEx1.Refresh();
            }
            else if (this.metroShell1.SelectedTab == metroTabItem3)
            {
                foreach (Control ct in panelEx1.Controls)
                    ct.Visible = false;
                panelEx1.Controls.Clear();

                usrCtrl1.Dock = DockStyle.Fill;
                usrCtrl1.Visible = true;
                panelEx1.Controls.Add(usrCtrl1);
                panelEx1.Refresh();
            }
        }

   

        private void metroTabItem1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.metroShell1.SettingsButtonText = Admins.admin.MyAccount.Name;
        }

        private void cmdCtrl1_Load(object sender, EventArgs e)
        {

        }
        #region Commandes

        private void buttonItem3_Click(object sender, EventArgs e)
        {

            this.cmdCtrl1.Init(cmdCtrl1.GetSelectedType());
        }

        private void buttonItem1_Click(object sender, EventArgs e)
        {
            try{
                GigCommand cmd = cmdCtrl1.GetSelectedCommand();
                if (cmd != null)
                   if( Admins.admin.ValidateCommand(cmd.SID, cmdCtrl1.GetSelectedTypeString()))
                       MessageBoxEx.Show("Traité avec succès", "Commandes", MessageBoxButtons.OK, MessageBoxIcon.Information);
           

            }
                catch{

                }
        }

        private void buttonItem2_Click(object sender, EventArgs e)
        {
            try
            {
                GigCommand cmd = cmdCtrl1.GetSelectedCommand();
                if (cmd != null)
                    if (Admins.admin.RemoveCommand(cmd.SID, cmdCtrl1.GetSelectedTypeString()))
                        MessageBoxEx.Show("Supprimé avec succès", "Commandes", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch
            {

            }
        }

        private void buttonItem5_Click(object sender, EventArgs e)
        {
            try
            {
                if (Admins.admin.RemoveAllCommand( cmdCtrl1.GetSelectedTypeString()))
                    MessageBoxEx.Show("Vidé avec succès", "Commandes", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch
            {

            }
        }

        private void buttonItem7_Click(object sender, EventArgs e)
        {
            try
            {
                if (Admins.admin.RemovAllValidatedCommand(cmdCtrl1.GetSelectedTypeString()))
                    MessageBoxEx.Show("Vidé avec succès", "Commandes", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch
            {

            }
        }

        private void buttonItem8_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem9_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private void metroShell1_SelectedTabChanged(object sender, EventArgs e)
        {
            SetControl();
        }
        private void metroShell1_Click(object sender, EventArgs e)
        {
            SetControl();
        }

        private void buttonItem31_Click(object sender, EventArgs e)
        {
            prodCtrl1.Init();
        }
        private void buttonItem12_Click(object sender, EventArgs e)
        {
            try
            {
                GProduct prod = prodCtrl1.GetSelectedCommand();
                if (prod != null)
                    if (Admins.admin.RemoveProduct(prod.ProductID.ToString()))
                        MessageBoxEx.Show("Supprimé avec succès", "Produits", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {

            }
        }

#region Users
        // users
        Findfrm findfrm = new Findfrm();
        private void buttonItem19_Click(object sender, EventArgs e)
        {
            try
            {
                
                findfrm.ShowDialog();
                usrCtrl1.Init(findfrm.textBoxX1.Text);
            }
            catch
            {

            }
        }

        private void buttonItem23_Click(object sender, EventArgs e)
        {
            try
            {
                GigUser u = usrCtrl1.GetSelectedUser();
                if (u != null)
                {
                    usrCtrl1.SetVisible(1);
                    usrCtrl1.itemPanel1.Items.Clear();
                    foreach (GigMessage g in Admins.admin.GetMessages(u.Username))
                    {
                        LabelItem l = new LabelItem();
                        l.Text = g.Sender + ": "+ g.Message;
                        usrCtrl1.itemPanel1.Items.Add(l);
                    }
                }

            }
            catch
            {

            }
        }

        private void buttonItem27_Click(object sender, EventArgs e)
        {
            try
            {
                GigUser u = usrCtrl1.GetSelectedUser();
                if (u != null)
                {
                    usrCtrl1.SetVisible(3);
                    usrCtrl1.itemPanel2.Items.Clear();
                    foreach (string g in Admins.admin.GetNotifications(u.Username))
                    {
                        LabelItem l = new LabelItem();
                        l.Text = g;
                        usrCtrl1.itemPanel2.Items.Add(l);
                    }
                }

            }
            catch
            {

            }
        }

        private void buttonItem26_Click(object sender, EventArgs e)
        {
            try
            {
                GigUser u = usrCtrl1.GetSelectedUser();
                if (u != null)
                {
                    usrCtrl1.SetVisible(2);
                    usrCtrl1.nlist.Items.Clear();
                    foreach (string g in Admins.admin.GetFriends(u.Username))
                    {
                        LabelItem l = new LabelItem();
                        l.Text = g;
                        usrCtrl1.nlist.Items.Add(l);
                    }
                }

            }
            catch
            {

            }
        }

        private void buttonItem4_Click(object sender, EventArgs e)
        {
            try{
                 GigUser u = usrCtrl1.GetSelectedUser();
                if (u != null)
                {
                    usrCtrl1.SetVisible(5);
                    usrCtrl1.itemPanel4.Items.Clear();
                    foreach (GFriendRequests g in Admins.admin.GetFriendRequests(u.Username))
                    {
                        LabelItem l = new LabelItem();
                        l.Text = g.Friend;
                        usrCtrl1.itemPanel4.Items.Add(l);
                    }
                }

            }
            catch
            {

            }
        }

        private void buttonItem24_Click(object sender, EventArgs e)
        {
            try
            {
                GigUser u = usrCtrl1.GetSelectedUser();
                if (u != null)
                {
                    usrCtrl1.SetVisible(4);
                    usrCtrl1.itemPanel3.Items.Clear();
                    foreach (GLog g in Admins.admin.GetLogs(u.Username))
                    {
                        LabelItem l = new LabelItem();
                        l.Text = u.Username+"@"+g.LogDate+":  "+ g.LogText ;
                        usrCtrl1.itemPanel3.Items.Add(l);
                    }
                }

            }
            catch
            {

            }
        }

        private void buttonItem21_Click(object sender, EventArgs e)
        {
            try
            {
                   GigUser u = usrCtrl1.GetSelectedUser();
                   if (u != null)
                   {
                       if (Admins.admin.RemoveUsers(u.Username))
                           MessageBoxEx.Show("Supprimé avec succès", "Utilisateurs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   }
            }
            catch
            {

            }
        }

        private void buttonItem20_Click(object sender, EventArgs e)
        {
            try
            {
                GigUser u = usrCtrl1.GetSelectedUser();
                if (u != null)
                {
                    UsrEditFrm frm = new UsrEditFrm(u);
                    frm.ShowDialog();
                    if (frm.Done)
                    {
                        if (Admins.admin.ModifyUser(u.Username, frm.user.Name, frm.user.Email, ((byte)frm.user.Role).ToString()))
                            MessageBoxEx.Show("Modifié avec succès", "Utilisateurs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }


                }
            }
            catch
            {

            }
        }

        private void buttonItem13_Click(object sender, EventArgs e)
        {
   
    
                try
                {

             
                    usrCtrl1.Init(findfrm.textBoxX1.Text);
                }
                catch
                {

                }
         
        }

        private void buttonItem16_Click(object sender, EventArgs e)
        {
            try
            {
                     GigUser u = usrCtrl1.GetSelectedUser();
                if (u != null)
                {
                CreateGtafrm frm = new CreateGtafrm();
                frm.ShowDialog();

                if (Admins.admin.CreateChar(u.Username, frm.textBoxX1.Text))
                    MessageBoxEx.Show("Crée avec succès", "Utilisateurs", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch
            {

            }
        }
#endregion

        private void buttonItem10_Click(object sender, EventArgs e)
        {
            try
            {

                ProdFrm frm = new ProdFrm();
                frm.ShowDialog();
                if (frm.Done)
                {
                    if (Admins.admin.AddProduct(frm.prod.Name, frm.prod.Description, (int)frm.prod.Quantity, frm.prod.Price, (byte)frm.prod.ProductType))
                        MessageBoxEx.Show("Produit crée avec succès", "Produits", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch
            {

            }
        }

        private void buttonItem15_Click(object sender, EventArgs e)
        {
            try
            {
                GProduct prod = prodCtrl1.GetSelectedCommand();
                if (prod != null)
                {
                    Addinfrm frm = new Addinfrm(prod.ProductID, (byte)prod.ProductType);
                    frm.ShowDialog();
                    if (frm.Done)
                    {
                        if ((byte)prod.ProductType <= 4)
                        {
                            Vehicle v = frm.v;
                            if(Admins.admin.AddVehicle(prod.ProductID.ToString(), v.Color1, v.Color2,v.Sale, v.Featured,v.Speed, v.Fuel,v.Places,v.Tuning,v.PrixIG,v.Water,v.Category))
                                MessageBoxEx.Show("Informations du produit "+prod.ProductID.ToString()+" ajouté avec succès", "Produits", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if ((byte)prod.ProductType == 5)
                        {
                            House h = frm.h;
                            if(Admins.admin.AddHouse(prod.ProductID.ToString(),h.Pieces,h.IdInt,h.Sale,h.Featured,h.Ville,h.Popularity,h.Garage,h.GarageMap, h.Wall,h.WallMap,h.PrixIG,h.Jardin,h.Piscine,h.Category))
                                MessageBoxEx.Show("Informations du produit " + prod.ProductID.ToString() + " ajouté avec succès", "Produits", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if ((byte)prod.ProductType <= 7)
                        {
                            Bizz b = frm.b;
                            if(Admins.admin.AddBizz(prod.ProductID.ToString(),b.Stock,b.Sale,b.Featured,b.Ville,b.Popularity,b.Depot,b.DepotMap,b.PrixIG,b.Category))
                                MessageBoxEx.Show("Informations du produit " + prod.ProductID.ToString() + " ajouté avec succès", "Produits", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                    }
                }
            }
            catch
            {

            }
        }

        private void buttonItem11_Click(object sender, EventArgs e)
        {
            try
            {
                      GProduct prod = prodCtrl1.GetSelectedCommand();
                      if (prod != null)
                      {
                          ProdFrm frm = new ProdFrm(prod);
                          frm.ShowDialog();
                          if (frm.Done)
                          {
                              if (Admins.admin.ModifyProduct(prod.ProductID.ToString(),frm.prod.Name, frm.prod.Description, (int)frm.prod.Quantity, frm.prod.Price, (byte)frm.prod.ProductType))
                                  MessageBoxEx.Show("Produit modifié " + prod.ProductID.ToString() + " avec succès", "Produits", MessageBoxButtons.OK, MessageBoxIcon.Information);
                          }
                      }
            }
            catch
            {

            }
        }

        private void buttonItem18_Click(object sender, EventArgs e)
        {
            try
            {
                GProduct prod = prodCtrl1.GetSelectedCommand();
                if (prod != null)
                {
                    AddPicsfrm frm = new AddPicsfrm(prod);
                    frm.ShowDialog();
                    if (frm.Done)
                    {
                        if (Admins.admin.AddPictures(prod.ProductID.ToString(), frm.textBoxX2.Text, frm.textBoxX3.Text, frm.textBoxX4.Text ))
                            MessageBoxEx.Show("Les images du produit ont été modifié (" + prod.ProductID.ToString() + ") avec succès", "Produits", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch
            {

            }
        }

        private void buttonItem17_Click(object sender, EventArgs e)
        {
            try
            {
                GProduct prod = prodCtrl1.GetSelectedCommand();
                if (prod != null)
                {
                    AddPicsfrm frm = new AddPicsfrm(prod,true);
                    frm.ShowDialog();
                    if (frm.Done)
                    {
                        if (Admins.admin.ModifyPictures(prod.ProductID.ToString(), frm.textBoxX2.Text, frm.textBoxX3.Text, frm.textBoxX4.Text))
                            MessageBoxEx.Show("Les images du produit ont été modifié (" + prod.ProductID.ToString() + ") avec succès", "Produits", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch
            {

            }
        }





    }
}