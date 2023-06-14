using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
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
using CoffeeShop.BUS;
using CoffeeShop.DTO;
using Microsoft.Win32;

namespace CoffeeShop.Menu
{
    /// <summary>
    /// Interaction logic for PopupEditMenu.xaml
    /// </summary>
    public partial class PopupEditMenu : UserControl
    {
        BUS_Beverage bus;
        string ID;
        MainWindow window;
        String selectedImageLink = "";
        byte[] imageBytes;
        public PopupEditMenu()
        {
            InitializeComponent();
        }
        public PopupEditMenu(string name, string type, string price, string id, byte[] imageLink, MainWindow context)
        {
            InitializeComponent();
            bus = new BUS_Beverage();
            tbName.Text = name;
            tbPrice.Text = price;
            cbBeverageType.ItemsSource = bus.getBeverageType();
            cbBeverageType.SelectedItem = type;
            try
            {
                image.Source = ToImageSource(ConvertByteArrayToImage(imageLink));
                imageBytes = imageLink;
            }
            catch (Exception) { }
            
            ID = id;
            window = context;
        }

        public ImageFormat GetImageFormat(System.Drawing.Image img)
        {
            if (img.RawFormat.Equals(ImageFormat.Jpeg))
                return ImageFormat.Jpeg;
            if (img.RawFormat.Equals(ImageFormat.Bmp))
                return ImageFormat.Bmp;
            if (img.RawFormat.Equals(ImageFormat.Png))
                return ImageFormat.Png;
            if (img.RawFormat.Equals(ImageFormat.Emf))
                return ImageFormat.Emf;
            if (img.RawFormat.Equals(ImageFormat.Exif))
                return ImageFormat.Exif;
            if (img.RawFormat.Equals(ImageFormat.Gif))
                return ImageFormat.Gif;
            if (img.RawFormat.Equals(ImageFormat.Icon))
                return ImageFormat.Icon;
            if (img.RawFormat.Equals(ImageFormat.MemoryBmp))
                return ImageFormat.MemoryBmp;
            if (img.RawFormat.Equals(ImageFormat.Tiff))
                return ImageFormat.Tiff;
            else
                return ImageFormat.Wmf;
        }

        public ImageSource ToImageSource(System.Drawing.Image image)
        {
            BitmapImage bitmap = new BitmapImage();

            using (MemoryStream stream = new MemoryStream())
            {
                // Save to the stream
                image.Save(stream, GetImageFormat(image));

                // Rewind the stream
                stream.Seek(0, SeekOrigin.Begin);

                // Tell the WPF BitmapImage to use this stream
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
            }

            return bitmap;
        }

        public System.Drawing.Image ConvertByteArrayToImage(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            return System.Drawing.Image.FromStream(stream);
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            tbNameValidation.Text = tbPriceValidation.Text = "";
            if (tbName.Text == "")
            {
                tbNameValidation.Text = "Tên món không được để trống.";
                return;
            }

            if (tbPrice.Text == "")
            {
                tbPriceValidation.Text = "Giá không được để trống.";
                return;
            }

            DTO_Beverage beverage = new DTO_Beverage();
            beverage.BeverageID = ID;
            beverage.BeverageName = tbName.Text;
            beverage.BeverageTypeID = bus.getBeverageTypeID(cbBeverageType.Text);
            beverage.Price = Int32.Parse(tbPrice.Text);
            beverage.Link = imageBytes;
            if (bus.editBeverage(beverage) > 0)
            {
                MessageBox.Show($"Đã sửa thông tin của {tbName.Text}");
                Window.GetWindow(this).Close();
            }
            else
                MessageBox.Show($"Đã có lỗi trong quá trình chỉnh sửa {tbName.Text}");
        }

        private void tbPrice_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberCheck.IsNumber(e.Text);
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void btSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                byte[] x = File.ReadAllBytes(op.FileName);
                image.Source = ToImageSource(ConvertByteArrayToImage(x));
                imageBytes = x;
                //MessageBox.Show(op.FileName);
                //image.Source = new BitmapImage(new Uri(op.FileName));
                //selectedImageLink = op.FileName;
            }
        }
    }
}
