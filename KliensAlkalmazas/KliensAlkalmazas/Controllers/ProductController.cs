using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1;
using System;
using System.Text.RegularExpressions;
using Hotcakes.CommerceDTO.v1.Client;
using System.Windows.Forms;



namespace KliensAlkalmazas.Controllers
{
    public class ProductController
    {
       
        public bool ModifyProduct(string Name, string Desc, decimal Price, int Stock, string inventoryId) 
        {
            try
            {
                if (!ValidateDesc(Desc)) 
                {
                    MessageBox.Show("Érvénytelen leírás: Nagybetűvel kezdődő, ponttal végződő, legalább 50 karakteres leírást adj meg!");
                    return false;
                }


                if (!ValidatePrice(Price.ToString())) 
                {
                    MessageBox.Show("Érvénytelen ár. Ne adj meg negatív, nullával kezdődő vagy nem egész értékeket");
                    return false;
                }

                if (!ValidateStock(Stock.ToString()))
                {
                    MessageBox.Show("Érvénytelen készletérték. Ne adj meg negatív, nullával kezdődő vagy nem egész értékeket");
                    return false;
                }

            }
            catch (Exception err)
            {
                MessageBox.Show($"Nem sikerült a módosítás: {err.Message}");
                //throw;
            }

            var url = string.Empty;
            var key = string.Empty;

            if (url == string.Empty) url = "http://20.234.113.211:8088";
            if (key == string.Empty) key = "1-4ce1a804-7cba-4a55-9a83-3ef3104c2908";


            var proxy = new Api(url, key);

            var product = proxy.ProductsFind(inventoryId).Content;
            var prodinv = proxy.ProductInventoryFindForProduct(inventoryId).Content;

            product.ProductName = Name;
            product.LongDescription = Desc;
            product.SitePrice = Price;
            prodinv[0].QuantityOnHand = Stock;
            

            ApiResponse<ProductDTO> response = proxy.ProductsUpdate(product);
            ApiResponse<ProductInventoryDTO> response2 = proxy.ProductInventoryUpdate(prodinv[0]);

            if (response.Errors.Count == 0 && response2.Errors.Count == 0)
                return true;
            else
                return false;

        }

        public bool ValidatePrice(string Price)
        {
            Regex regex = new Regex(@"^(0|[1-9]\d*)$");
            return regex.IsMatch(Price);
        }

        public bool ValidateDesc(string Desc)
        {
            Regex regex = new Regex(@"^[A-Z0-9].{48,}\.$");
            return regex.IsMatch(Desc);
        }

        public bool ValidateStock(string Stock)
        {
            Regex regex = new Regex(@"^(?!0\d)\d+$");
            return regex.IsMatch(Stock);

        }

    }
}