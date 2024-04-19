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

            //MessageBox.Show(s.GetType().ToString());

            //listBox1.DataSource = s.Content.ToList();

            
            for (int i = 1; i <= 100; i++)
            {
                //bindingList.Add(s.Content[i].ProductName);

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
            //listBox1.SelectedIndex = -1;

            //string url = "http://20.234.113.211:8088";
            //string key = "1-4ce1a804-7cba-4a55-9a83-3ef3104c2908";

            //Api proxy = new Api(url, key);

            //// find all categories in the store
            //ApiResponse<List<ProductDTO>> response = proxy.ProductsFindAll();

        }

        //private async void button1_Click(object sender, EventArgs e)
        //{
        //    var apiUrl = "http://20.234.113.211:8088/DesktopModules/Hotcakes/API/rest/v1/products";
        //    try
        //    {
        //        var products = await GetProductsAsync(apiUrl);

        //        // Clear the existing items
        //        listBox1.Items.Clear();

        //        // Populate the ListBox with product names
        //        foreach (var product in products)
        //        {
        //            listBox1.Items.Add(product.Name);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error fetching products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //private async Task<List<Product>> GetProductsAsync(string apiUrl)
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        var response = await httpClient.GetStringAsync(apiUrl);
        //        var products = JsonConvert.DeserializeObject<List<Product>>(response);
        //        return products;
        //    }
        //}

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
            //var ProductInventoryDTO = proxy.ProductInventoryFind(inventoryId);
            //var s = proxy.ProductInventoryFind().Content.QuantityOnHand;


            //if (ProductInventoryDTO.Content.QuantityOnHand == null)
            //{
            //    ProductInventoryDTO.Content.QuantityOnHand = 0;
            //}
            textBox2.Text = response.Content.QuantityOnHand.ToString();
            //var i = ProductInventoryDTO.Content.QuantityReserved;
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
