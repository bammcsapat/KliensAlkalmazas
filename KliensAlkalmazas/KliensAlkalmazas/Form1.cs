using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hotcakes.CommerceDTO.v1.Client;
using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using Newtonsoft.Json;

namespace KliensAlkalmazas
{
    public partial class Form1 : Form
    {
        BindingList<Product> bindingList = new BindingList<Product>();
        public Form1()
        {
            InitializeComponent();

            var url = string.Empty;
            var key = string.Empty;

            if (url == string.Empty) url = "http://20.234.113.211:8088";
            if (key == string.Empty) key = "1-4ce1a804-7cba-4a55-9a83-3ef3104c2908";



            var proxy = new Api(url, key);

            var s = proxy.ProductsFindAll();


            
            for (int i = 1; i <= 100; i++)
            {

                Product product = new Product
                {
                    Sku = s.Content[i].Sku,
                    Name = s.Content[i].ProductName,
                    bvin = s.Content[i].Bvin
                };
                bindingList.Add(product);
            }

            listBox1.DataSource = bindingList;
            listBox1.DisplayMember = "Name";

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = listBox1.SelectedIndex;
            var inventoryId = bindingList[selected].bvin;
            MessageBox.Show(inventoryId);

            var url = string.Empty;
            var key = string.Empty;

            if (url == string.Empty) url = "http://20.234.113.211:8088";
            if (key == string.Empty) key = "1-4ce1a804-7cba-4a55-9a83-3ef3104c2908";




            var proxy = new Api(url, key);
            ApiResponse<ProductInventoryDTO> response = proxy.ProductInventoryFind(inventoryId);
            //var response = proxy.ProductInventoryFind(inventoryId); //így sem jó



            textBox2.Text = response.Content.QuantityOnHand.ToString(); //

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var selected = listBox1.SelectedIndex;
            var inventoryId = bindingList[selected].bvin;

            var url = string.Empty;
            var key = string.Empty;

            if (url == string.Empty) url = "http://20.234.113.211:8088";
            if (key == string.Empty) key = "1-4ce1a804-7cba-4a55-9a83-3ef3104c2908";



            var proxy = new Api(url, key);

            var inventory = proxy.ProductInventoryFind(inventoryId).Content;

            // update one or more inventory properties
            inventory.QuantityReserved = int.Parse(textBox2.Text);

            // call the API to create the new product inventory record
            ApiResponse<ProductInventoryDTO> response = proxy.ProductInventoryUpdate(inventory);
        }
    }
}
