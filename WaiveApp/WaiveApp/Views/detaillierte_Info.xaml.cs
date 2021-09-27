
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiveApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class detaillierte_Info : ContentPage
    {
        public detaillierte_Info()
        {
            InitializeComponent();
            location_name_global.Text = Constants.location_name_global;
            listView.ItemsSource = Constants.items_global;
        }
    }
}