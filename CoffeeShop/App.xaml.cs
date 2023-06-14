using CoffeeShop.Database;
using System.Windows;
using System.Linq;
using Dietapp;

namespace CoffeeShop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            if (DBConfig.EnsureSqlLocalDb(true))
            {
                using (var context = DBContext.CreateInstance())
                {
                    if (!context.Database.Exists())
                    {
                        DBConfig.InitDBWithSampleData(context);
                    }
                }
            }
            else
            {
                var window = new MissingFeatureWindow();
                window.ShowDialog();
                MessageBox.Show("Khởi động lại ứng dụng để áp dụng thay đổi");
                this.App_Exit(this, null);

            }

            Exit += App_Exit;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            DBConfig.Release();
        }
    }
}
