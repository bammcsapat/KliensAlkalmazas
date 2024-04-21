using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KliensAlkalmazas.Controllers
{
    public class ProductController
    {
        public Product ModifyPrice(Product product, decimal Price)
        {

            try
            {
                if (!ValidatePrice(Price.ToString()));
            }
            catch (Exception)
            {

                throw;
            }

            product.Price = Price;
            return product;

        }

        public Product ModifyDescription(Product product, string Desc)
        {

            try
            {
                if (!ValidatePrice(Desc.ToString()));
            }
            catch (Exception)
            {

                throw;
            }

            product.Desc = Desc;
            return product;

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