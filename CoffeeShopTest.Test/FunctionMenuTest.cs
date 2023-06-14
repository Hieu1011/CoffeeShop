
using CoffeeShop.Menu;
using NUnit.Framework;

namespace CoffeeShopTest.Test
{
    public class FunctionMenuTest
    {
        [TestFixture]
        public class UnitTestFuncMenu
        {


            [SetUp]


            [Test]
            public void NoFill_WhenAllFieldIsBlank()
            {
                //arange
                PopupAddMenu addMenu = new PopupAddMenu();
                string FieldMenuName = null;
                string FieldMenuKind = null;
                string FieldMenuPrice = null;
                int expected = 0;

                //act
                int check = addMenu.addMenuItem(FieldMenuName, FieldMenuKind, FieldMenuPrice);

                //assert
                Assert.AreEqual(expected, check);
            }


            [Test]
            public void TwoPlusTwoEqualFour()
            {
                Assert.AreEqual(4, _cal.Add(2, 2));
            }

            [Test]
            public void FourPlusOneEqualFive()
            {
                Assert.AreEqual(5, _cal.Add(4, 1));
            }
        }
}