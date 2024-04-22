using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1;
using System;
using System.Text.RegularExpressions;
using Hotcakes.CommerceDTO.v1.Client;


namespace KliensAlkalmazas.Controllers
{
    public class ProductController
    {
       
        public bool ModifyProduct(string Name, string Desc, decimal Price, string inventoryId) 
        {
            try
            {
                if (!ValidateDesc(Desc))
                    throw new ArgumentException("Érvénytelen leírás");

                if (!ValidatePrice((Price.ToString())))
                    throw new ArgumentException("Érvénytelen ár");
            }
            catch (Exception)
            {
                throw;
            }

            var url = string.Empty;
            var key = string.Empty;

            if (url == string.Empty) url = "http://20.234.113.211:8088";
            if (key == string.Empty) key = "1-4ce1a804-7cba-4a55-9a83-3ef3104c2908";


            var proxy = new Api(url, key);

            var product = proxy.ProductsFind(inventoryId).Content;

            product.ProductName = Name;
            product.LongDescription = Desc;
            product.SitePrice = Price;

            ApiResponse<ProductDTO> response = proxy.ProductsUpdate(product);

            if (response.Errors.Count == 0)
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
            Regex regex = new Regex(@"^[A-Z].{48,}\.$");
            return regex.IsMatch(Desc);
        }
    }
}