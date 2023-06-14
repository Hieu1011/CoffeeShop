using CoffeeShop.Menu;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CoffeeShop.Menu.Tests {
    public class PopupAddMenuTests {
        [Fact]
        public void FillAll_When_Everything_FillWell() {
            Thread thread = new Thread(() => {
                //arange
                PopupAddMenu addMenu = new PopupAddMenu();
                string FieldMenuName = "TestFail";
                string FieldMenuKind = "Tea";
                string FieldMenuPrice = "22000";
                int expected = 1;

                //act
                int check = addMenu.addMenuItem(FieldMenuName, FieldMenuKind, FieldMenuPrice);
                //assert
                Assert.Equal(expected, check);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [Fact]
        public void FillAll_When_Name_OutOfRange_AndAllLeft_IsNull() {
            Thread thread = new Thread(() => {
                //arange
                PopupAddMenu addMenu = new PopupAddMenu();
                string FieldMenuName = "yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy";
                string FieldMenuKind = null;
                string FieldMenuPrice = null;
                int expected = 0;

                //act
                int check = addMenu.addMenuItem(FieldMenuName, FieldMenuKind, FieldMenuPrice);
                //assert
                Assert.Equal(expected, check);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [Fact]
        public void FillAll_When_Name_OutOfRange_AndPrice_IsNull() {
            Thread thread = new Thread(() => {
                //arange
                PopupAddMenu addMenu = new PopupAddMenu();
                string FieldMenuName = "yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy";
                string FieldMenuKind = "Tea";
                string FieldMenuPrice = null;
                int expected = 0;

                //act
                int check = addMenu.addMenuItem(FieldMenuName, FieldMenuKind, FieldMenuPrice);
                //assert
                Assert.Equal(expected, check);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [Fact]
        public void FillAll_When_NameAndPrice_OutOfRange_AndType_IsNull() {
            Thread thread = new Thread(() => {
                //arange
                PopupAddMenu addMenu = new PopupAddMenu();
                string FieldMenuName = "yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy";
                string FieldMenuKind = null;
                string FieldMenuPrice = "Fail";
                int expected = 0;

                // Act
                int check = addMenu.addMenuItem(FieldMenuName, FieldMenuKind, FieldMenuPrice);

                // Assert
                Assert.Equal(expected, check);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [Fact]
        public void FillAll_When_NameField_OutOfRange() {
            Thread thread = new Thread(() => {
                //arange
                PopupAddMenu addMenu = new PopupAddMenu();
                string FieldMenuName = "yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy";
                string FieldMenuKind = "Tea";
                string FieldMenuPrice = "22000";
                int expected = 1;

                //act
                int check = addMenu.addMenuItem(FieldMenuName, FieldMenuKind, FieldMenuPrice);
                //assert
                Assert.Equal(expected, check);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [Fact]
        public void FillAll_When_PriceField_AndName_OutOfRange() {
            Thread thread = new Thread(() => {
                //arange
                PopupAddMenu addMenu = new PopupAddMenu();
                string FieldMenuName = "yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy";
                string FieldMenuKind = "Tea";
                string FieldMenuPrice = "Fail";
                int expected = 0;

                // Act & Assert
                Assert.Throws<FormatException>(() => {
                    int check = addMenu.addMenuItem(FieldMenuName, FieldMenuKind, FieldMenuPrice);
                    Assert.Equal(expected, check);
                });
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [Fact]
        public void FillAll_When_PriceField_OutOfRange() {
            Thread thread = new Thread(() => {
                // Arrange
                PopupAddMenu addMenu = new PopupAddMenu();
                string FieldMenuName = "Test";
                string FieldMenuKind = "Tea";
                string FieldMenuPrice = "Fail";
                int expected = 0;

                // Act & Assert
                Assert.Throws<FormatException>(() => {
                    int check = addMenu.addMenuItem(FieldMenuName, FieldMenuKind, FieldMenuPrice);
                    Assert.Equal(expected, check);
                });
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [Fact]
        public void FillAll_When_PriceField_OutOfRange_AndTypeNull() {
            Thread thread = new Thread(() => {
                // Arrange
                PopupAddMenu addMenu = new PopupAddMenu();
                string FieldMenuName = "Test";
                string FieldMenuKind = null;
                string FieldMenuPrice = "Fail";
                int expected = 0;

                // Act & Assert
                Assert.Throws<FormatException>(() => {
                    int check = addMenu.addMenuItem(FieldMenuName, FieldMenuKind, FieldMenuPrice);
                    Assert.Equal(expected, check);
                });
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [Fact]
        public void FillPart_WhenOnly_Name_FieldIs_NotFillWell() {
            Thread thread = new Thread(() => {
                //arange
                PopupAddMenu addMenu = new PopupAddMenu();
                string FieldMenuName = null;
                string FieldMenuKind = "Coffee";
                string FieldMenuPrice = "22000";
                int expected = 0;

                //act
                int check = addMenu.addMenuItem(FieldMenuName, FieldMenuKind, FieldMenuPrice);
                //assert
                Assert.Equal(expected, check);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [Fact]
        public void FillPart_WhenOnly_NameFieldIs_FillWell() {
            Thread thread = new Thread(() => {
                //arange
                PopupAddMenu addMenu = new PopupAddMenu();
                string FieldMenuName = "Test";
                string FieldMenuKind = null;
                string FieldMenuPrice = null;
                int expected = 0;

                //act
                int check = addMenu.addMenuItem(FieldMenuName, FieldMenuKind, FieldMenuPrice);
                //assert
                Assert.Equal(expected, check);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [Fact]
        public void FillPart_WhenOnly_Price_FieldIs_NotFillWell() {
            Thread thread = new Thread(() => {
                //arange
                PopupAddMenu addMenu = new PopupAddMenu();
                string FieldMenuName = "Test";
                string FieldMenuKind = "Coffee";
                string FieldMenuPrice = null;
                int expected = 0;

                //act
                int check = addMenu.addMenuItem(FieldMenuName, FieldMenuKind, FieldMenuPrice);
                //assert
                Assert.Equal(expected, check);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [Fact]
        public void FillPart_WhenOnly_PriceFieldIs_FillWell() {
            Thread thread = new Thread(() => {
                //arange
                PopupAddMenu addMenu = new PopupAddMenu();
                string FieldMenuName = null;
                string FieldMenuKind = null;
                string FieldMenuPrice = "22000";
                int expected = 0;

                //act
                int check = addMenu.addMenuItem(FieldMenuName, FieldMenuKind, FieldMenuPrice);
                //assert
                Assert.Equal(expected, check);

            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [Fact]
        public void FillPart_WhenOnly_Type_FieldIs_FillWell() {
            Thread thread = new Thread(() => {
                //arange
                PopupAddMenu addMenu = new PopupAddMenu();
                string FieldMenuName = null;
                string FieldMenuKind = "Coffee";
                string FieldMenuPrice = null;
                int expected = 0;

                //act
                int check = addMenu.addMenuItem(FieldMenuName, FieldMenuKind, FieldMenuPrice);
                //assert
                Assert.Equal(expected, check);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [Fact]
        public void FillPart_WhenOnly_Type_FieldIs_NotFillWell() {
            Thread thread = new Thread(() => {
                //arange
                PopupAddMenu addMenu = new PopupAddMenu();
                string FieldMenuName = "Test";
                string FieldMenuKind = null;
                string FieldMenuPrice = "22000";
                int expected = 0;

                //act
                int check = addMenu.addMenuItem(FieldMenuName, FieldMenuKind, FieldMenuPrice);
                //assert
                Assert.Equal(expected, check);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [Fact]
        public void NoFill_WhenAllFieldIsBlank() {
            Thread thread = new Thread(() => {
                //arange
                PopupAddMenu addMenu = new PopupAddMenu();
                string FieldMenuName = null;
                string FieldMenuKind = null;
                string FieldMenuPrice = null;
                int expected = 0;

                //act
                int check = addMenu.addMenuItem(FieldMenuName, FieldMenuKind, FieldMenuPrice);
                Assert.Equal(expected, check);

            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
    }
}