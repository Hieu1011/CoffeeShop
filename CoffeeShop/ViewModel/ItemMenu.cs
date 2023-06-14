using CoffeeShop.View;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CoffeeShop.ViewModel {
    public class ItemMenu {
        public ItemMenu(string header, List<SubItem> subItems, PackIconKind icon) {
            Header = header;
            SubItems = subItems;
            Icon = icon;
            APID = "";
        }

        public ItemMenu(string header, string id, UserControl screen, PackIconKind icon) {
            Header = header;
            Screen = screen;
            Icon = icon;
            APID = id;
        }

        public ItemMenu(string header, string id, Cashier cashier, PackIconKind icon) {
            Header = header;
            _Cashier = cashier;
            Icon = icon;
            APID = id;
        }

        public string Header { get; private set; }
        public PackIconKind Icon { get; private set; }
        public List<SubItem> SubItems { get; private set; }
        public UserControl Screen { get; private set; }

        public Cashier _Cashier { get; private set; }
        public string APID { get; private set; }
    }
}