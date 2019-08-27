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
    public partial class ProdFrm : DevComponents.DotNetBar.Metro.MetroForm
    {
        public bool Done = false;

        public ProdFrm(GProduct prod)
        {
            InitializeComponent();
            this.Text = "Modification du produit : ID_" + prod.ProductID.ToString();
            comboBoxEx1.SelectedItem = comboItem1;
            textBoxX1.Text = prod.Name;
            textBoxX2.Text = prod.Description;
            integerInput1.Value = (int)prod.Price;
            integerInput2.Value = (int)prod.Quantity;

            if (prod.ProductType == GProductType.Voitures)
                comboBoxEx1.SelectedItem = comboItem1;
            else if (prod.ProductType == GProductType.Bateaux)
                comboBoxEx1.SelectedItem = comboItem2;
            else if (prod.ProductType == GProductType.Avions)
                comboBoxEx1.SelectedItem = comboItem3;
            else if (prod.ProductType == GProductType.Semi)
                comboBoxEx1.SelectedItem = comboItem4;
            else if (prod.ProductType == GProductType.Maisons)
                comboBoxEx1.SelectedItem = comboItem5;
            else if (prod.ProductType == GProductType.Bizz)
                comboBoxEx1.SelectedItem = comboItem6;
            else if (prod.ProductType == GProductType.Usines)
                comboBoxEx1.SelectedItem = comboItem7;
        }
        public ProdFrm()
        {
            InitializeComponent();
            comboBoxEx1.SelectedItem = comboItem1;
        }
        public GProduct prod;
        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            prod = new GProduct();
            prod.Name = textBoxX1.Text;
            prod.Description = textBoxX2.Text;
            prod.Quantity = (ushort)integerInput2.Value;
            prod.Price = (ushort)integerInput1.Value;
          
            prod.ProductType = (GProductType)byte.Parse(((ComboItem)comboBoxEx1.SelectedItem).Value.ToString());
            Done = true;
            this.Close();
        }
    }
}