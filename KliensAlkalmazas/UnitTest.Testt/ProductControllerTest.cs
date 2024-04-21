using KliensAlkalmazas;
using KliensAlkalmazas.Controllers;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Testt
{
    public class ProductControllerTest
    {
        /*Product product1 = new Product();
        Product product2 = new Product();

        [Test,
         TestCase(product1, 1000), 
         TestCase(product2, 2000)
         ]
        public void TestModifyPrice(Product product, decimal newPrice)
        {
            //Arrange
            var productcontroller = new ProductController();

            // Act
            var actualProduct = productcontroller.ModifyPrice(product, newPrice);

            // Assert
            ClassicAssert.AreEqual(newPrice, actualProduct.Price);
        }*/

        [Test,
        TestCase("15000,0", false),
        TestCase("-15000", false),
        TestCase("015000", false),
        TestCase("15000", true)
         ]

        public void TestValidatePrice(string Price, bool expectedResult) 
        {
            //Arrange
            var productcontroller = new ProductController();

            // Act
            var actualResult = productcontroller.ValidatePrice(Price);

            //Assert
            ClassicAssert.AreEqual(expectedResult, actualResult);

        }


        [Test,
        TestCase("", false),
        TestCase("ez egy gyűrű", false),
        TestCase("Ez egy gyűrű", false),
        TestCase("Ez egy gyűrű.", false),
        TestCase("ezüst színű elegáns karikagyűrű, apró, fényes kövekkel díszítve", false),
        TestCase("ezüst színű elegáns karikagyűrű, apró, fényes kövekkel díszítve.", false),
        TestCase("Ezüst színű elegáns karikagyűrű, apró, fényes kövekkel díszítve", false),
        TestCase("Ezüst színű elegáns karikagyűrű, apró, fényes kövekkel díszítve.", true)
         ]

        public void TestValidateDesc(string Desc, bool expectedResult)
        {
            //Arrange
            var productcontroller = new ProductController();

            // Act
            var actualResult = productcontroller.ValidateDesc(Desc);

            //Assert
            ClassicAssert.AreEqual(expectedResult, actualResult);

        }
    }
}
