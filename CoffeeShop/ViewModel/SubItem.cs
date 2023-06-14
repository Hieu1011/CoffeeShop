using System.Windows.Controls;

namespace CoffeeShop.ViewModel {
    public class SubItem {
        public SubItem(string name, string id, UserControl screen = null) {
            Name = name;
            Screen = screen;
            APID = id;
        }
        public string Name { get; private set; }
        public UserControl Screen { get; private set; }
        public string APID { get; private set; }
    }
}