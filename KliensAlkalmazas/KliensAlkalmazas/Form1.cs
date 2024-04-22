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
using System.Net;
using System.Runtime.Remoting.Contexts;

namespace KliensAlkalmazas
{
    public partial class Form1 : Form
    {
        BindingList<Product> bindingList = new BindingList<Product>();
        public Form1()
        {
            InitializeComponent();
            Listazas();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Navigalas();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Mentes();
        }

        private void Listazas()
        {

            var url = string.Empty;
            var key = string.Empty;

            if (url == string.Empty) url = "http://20.234.113.211:8088";
            if (key == string.Empty) key = "1-4ce1a804-7cba-4a55-9a83-3ef3104c2908";



            var proxy = new Api(url, key);

            var s = proxy.ProductsFindAll();



            for (int i = 1; i <= 110; i++)
            {

                Product product = new Product
                {
                    Sku = s.Content[i].Sku,
                    Name = s.Content[i].ProductName,
                    Desc = s.Content[i].LongDescription,
                    Price = s.Content[i].SitePrice,
                    bvin = s.Content[i].Bvin
                };
                bindingList.Add(product);
            }

            listBox1.DataSource = bindingList;
            listBox1.DisplayMember = "Name";
        }

        

        private void Navigalas()
        {
            var selected = listBox1.SelectedIndex;
            var inventoryId = bindingList[selected].bvin;
            //MessageBox.Show(inventoryId); //teszt hogy látszódjon hogy jó bvin-t ad vissza

            var url = string.Empty;
            var key = string.Empty;

            if (url == string.Empty) url = "http://20.234.113.211:8088";
            if (key == string.Empty) key = "1-4ce1a804-7cba-4a55-9a83-3ef3104c2908";




            var proxy = new Api(url, key);
            ApiResponse<ProductInventoryDTO> response = proxy.ProductInventoryFind(inventoryId);
            //var response = proxy.ProductInventoryFind(inventoryId); //így sem jó



            //textBoxKeszlet.Text = response.Content.QuantityOnHand.ToString(); //emiatt nem fut le, üres objectet ad vissza az api
            //hívás valamiért. ha ezt kikommenteljük lefut, de ugyanúgy üres lesz a response object

            textBoxTermeknev.Text = bindingList[selected].Name;
            textBoxLeiras.Text = WebUtility.HtmlDecode(bindingList[selected].Desc).Replace("<p>", "").Replace("</p>", "");
            var segedAr = (int)bindingList[selected].Price;
            textBoxAr.Text = segedAr.ToString();
        }

        private void Mentes()
        {
            var selected = listBox1.SelectedIndex;
            var inventoryId = bindingList[selected].bvin;

            var url = string.Empty;
            var key = string.Empty;

            if (url == string.Empty) url = "http://20.234.113.211:8088";
            if (key == string.Empty) key = "1-4ce1a804-7cba-4a55-9a83-3ef3104c2908";



            var proxy = new Api(url, key);

            //var inventory = proxy.ProductInventoryFind(inventoryId).Content;
            var product = proxy.ProductsFind(inventoryId).Content;

            product.ProductName = textBoxTermeknev.Text;
            product.LongDescription = textBoxLeiras.Text;
            product.SitePrice = decimal.Parse(textBoxAr.Text);

            //lokális példány frissítés
            bindingList[selected].Name = textBoxTermeknev.Text;
            bindingList[selected].Desc = textBoxLeiras.Text;
            bindingList[selected].Price = decimal.Parse(textBoxAr.Text);
            //inventory.QuantityOnHand = int.Parse(textBoxKeszlet.Text);

            //ApiResponse<ProductInventoryDTO> response = proxy.ProductInventoryUpdate(inventory);
            ApiResponse<ProductDTO> response2 = proxy.ProductsUpdate(product);

            if (response2.Errors.Count == 0)
            {
                MessageBox.Show("Sikeres módosítás!");
            }
            else
            {
                MessageBox.Show("Nem sikerült a módosítás!");
            }

            //lokális UI frissítés
            textBoxTermeknev.Text = bindingList[selected].Name;
            textBoxLeiras.Text = WebUtility.HtmlDecode(bindingList[selected].Desc).Replace("<p>", "").Replace("</p>", "");
            var segedAr = (int)bindingList[selected].Price;
            textBoxAr.Text = segedAr.ToString();


        }

        void TermekFilter()
        {
            var termek = from x in bindingList
                         where x.Name.ToLower().Contains(textBoxFilter.Text)
                         select x;

            listBox1.DataSource = termek.ToList();
            listBox1.DisplayMember = "Name";
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            TermekFilter();
        }
    }
}
