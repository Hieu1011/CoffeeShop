using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoffeeShop.View
{
    /// <summary>
    /// Interaction logic for TitleBar.xaml
    /// </summary>
    public partial class TitleBar : UserControl
    {
        private UserControl _theControl = null;

        public TitleBar()
        {
            InitializeComponent();
            
        }
        private UserControl GetParentUserControl(DependencyObject toFind)
        {
            while (!(toFind is UserControl))
            {
                toFind = LogicalTreeHelper.GetParent(toFind);
            }
            return (UserControl)toFind;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_theControl == null)
            {
                // Get user control hosting this title bar
                _theControl = GetParentUserControl(this.Parent);
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.Close();
        }

        public static readonly DependencyProperty WindowTitleProperty = DependencyProperty.RegisterAttached("WindowTitleProperty",
                typeof(string), typeof(TitleBar),
                new FrameworkPropertyMetadata(null, WindowTitlePropertyChanged));

        public static string GetWindowTitle(DependencyObject element)
        {
            return (string)element.GetValue(WindowTitleProperty);
        }

        public static void SetWindowTitle(DependencyObject element, string value)
        {
            element.SetValue(WindowTitleProperty, value);
        }

        private static void WindowTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Application.Current.MainWindow.Title = (string)e.NewValue;
        }
    }
}
