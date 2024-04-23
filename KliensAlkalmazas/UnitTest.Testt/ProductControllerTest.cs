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

        [Test,
         TestCase("Celestial Sparkle Fülbevaló", "Minimalista tervezésű, 18 karátos arany fülbevalók, amelyek egyetlen gyémánttal vannak díszítve, hangsúlyozva a kövek természetes szépségét. Ezek az ékszerek a visszafogott elegancia megtestesítői.", 150000, 10, "0801a19e-a910-4c5a-b948-0f5adf744b8d"), 
         TestCase("Arca Aura Gyűrű", "Visszafogott 18 karátos sárgaarany karikagyűrű, középpontjában gyémánttal, melyet 4 kisebb gyémánt fog közre.", 350000, 10, "e8785bb1-3a19-4543-9d40-46bdcce5ac34")
         ]
        public void TestModifyProduct(string Name, string Desc, decimal Price, int Stock, string inventoryId)
        {
            //Arrange
            var productcontroller = new ProductController();

            // Act
            var actualResponse = productcontroller.ModifyProduct(Name, Desc, Price, Stock, inventoryId);

            // Assert
            ClassicAssert.True(actualResponse);
        }

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
