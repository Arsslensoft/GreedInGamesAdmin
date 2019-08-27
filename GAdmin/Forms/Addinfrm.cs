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
    public partial class Addinfrm : DevComponents.DotNetBar.Metro.MetroForm
    {
        public Vehicle v;
        public House h;
        public Bizz b;
        public bool Done = false;
        public Addinfrm(ulong pid, byte type)
        {
            InitializeComponent();
            this.Text += pid.ToString();
            if (type <= 4)
                Veh.Visible = true;
            else if (type == 5)
                superTabItem1.Visible = true;
            else if (type <= 7)
                superTabItem2.Visible = true;

        }

        private void superTabControlPanel1_Click(object sender, EventArgs e)
        {

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            v = new Vehicle();
            v.Color1 = color.Text;
            v.Color2 = color2.Text;
            v.Category = categv.Text;
            v.Featured = (byte)featv.Value;
            v.Fuel = (ushort)fuel.Value;
            v.Places = (byte)palces.Value;
            v.PrixIG = (ulong)prixigv.Value;
            v.Sale = (byte)salev.Value;
            v.Speed = (ushort)speed.Value;
            v.Tuning = (byte)tuning.Value;
            v.Water = (byte)eau.Value;
            Done = true;
            this.Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            h = new House();
            h.Category = categh.Text;
            h.Featured = (byte)feath.Value;
            h.PrixIG = (ulong)prixigh.Value;
            h.Sale =saleh.Value.ToString();

            h.IdInt = idint.Value.ToString();
            h.Garage = gar.Value.ToString();
            h.GarageMap = garm.Value.ToString();
            h.Jardin = jardin.Value.ToString();
            h.Pieces = pieces.Value.ToString();
            h.Piscine = pisci.Value.ToString();
            h.Popularity = (ushort)poph.Value;
            h.Ville = villeh.Text;
            h.Wall = wall.Value.ToString();
             h.WallMap = wallm.Value.ToString();
             Done = true;
            this.Close();

        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            b = new Bizz();
            b.Category = categb.Text;
            b.Featured = (byte)featb.Value;
            b.PrixIG = (ulong)prixigb.Value;
            b.Sale = saleb.Value.ToString();

            b.Depot = depot.Value.ToString();
            b.DepotMap = depotmap.Value.ToString();
            b.Popularity = (ushort)popb.Value;
            b.Stock = stock.Text;
            b.Ville = villeb.Text;
            Done = true;
            this.Close();
        }
    }
}