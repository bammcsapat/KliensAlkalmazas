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
using KliensAlkalmazas.Controllers;
using Hotcakes.Commerce.Catalog;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace KliensAlkalmazas
{
    public partial class Form1 : Form
    {
        private ProductController productController = new ProductController();
        BindingList<Product> bindingList = new BindingList<Product>();
        public Form1()
        {
            InitializeComponent();
            Listazas();
            textBoxFilter.GotFocus += (sender, e) =>
            {
                if (textBoxFilter.Text == "Keresés...")
                {
                    textBoxFilter.Text = "";
                    textBoxFilter.ForeColor = System.Drawing.Color.Black; // Set text color to black
                }
            };
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
                var prodinv = proxy.ProductInventoryFindForProduct(product.bvin);
                product.Stock = prodinv.Content[0].QuantityOnHand;
                bindingList.Add(product);
            }


            listBox1.DataSource = bindingList;
            listBox1.DisplayMember = "Name";
        }

        

        private void Navigalas()
        {
            var selected = listBox1.SelectedIndex;
            var inventoryId = bindingList[selected].bvin;

            var url = string.Empty;
            var key = string.Empty;

            if (url == string.Empty) url = "http://20.234.113.211:8088";
            if (key == string.Empty) key = "1-4ce1a804-7cba-4a55-9a83-3ef3104c2908";




            var proxy = new Api(url, key);

            ApiResponse<ProductInventoryDTO> response = proxy.ProductInventoryFind(inventoryId);
            var prodinv = proxy.ProductInventoryFindForProduct(inventoryId);


            textBoxTermeknev.Text = bindingList[selected].Name;
            textBoxLeiras.Text = WebUtility.HtmlDecode(bindingList[selected].Desc).Replace("<p>", "").Replace("</p>", "");
            var segedAr = (int)bindingList[selected].Price;
            textBoxAr.Text = segedAr.ToString();
            textBoxDb.Text = bindingList[selected].Stock.ToString();
        }

        private void Mentes()
        {
            var selected = listBox1.SelectedIndex;
            var inventoryId = bindingList[selected].bvin;

            var controller = new ProductController();

            
            var name = textBoxTermeknev.Text;
            var desc = textBoxLeiras.Text;
            var price = decimal.Parse(textBoxAr.Text);
            var stock = int.Parse(textBoxDb.Text);

            var modifyResponse = controller.ModifyProduct(name, desc, price, stock, inventoryId);

            if (modifyResponse)
            {
                MessageBox.Show("Sikeres módosítás!");
                //lokalis adatbazis valtozása új értékekre
                bindingList[selected].Name = textBoxTermeknev.Text;
                bindingList[selected].Desc = textBoxLeiras.Text;
                bindingList[selected].Price = decimal.Parse(textBoxAr.Text);
                bindingList[selected].Stock = int.Parse(textBoxDb.Text);

                //textboxok frissítése új értékekre
                textBoxTermeknev.Text = bindingList[selected].Name;
                textBoxLeiras.Text = WebUtility.HtmlDecode(bindingList[selected].Desc).Replace("<p>", "").Replace("</p>", "");
                var segedAr = (int)bindingList[selected].Price;
                textBoxAr.Text = segedAr.ToString();
                textBoxDb.Text = bindingList[selected].Stock.ToString();
            }
            else
            {
                MessageBox.Show("Nem sikerült a módosítás!");
                //textbox visszaállítása eredeti értékekre
                textBoxTermeknev.Text = bindingList[selected].Name;
                textBoxLeiras.Text = WebUtility.HtmlDecode(bindingList[selected].Desc).Replace("<p>", "").Replace("</p>", "");
                var segedAr = (int)bindingList[selected].Price;
                textBoxAr.Text = segedAr.ToString();
                textBoxDb.Text = bindingList[selected].Stock.ToString();
            }



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
