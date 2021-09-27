using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading;
using WaiveApp.Views;
using Xamarin.Forms.Xaml;
using System.Reflection;
using WaiveApp.Models;
using System.Diagnostics;

namespace WaiveApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private string location_name = "Location unknown";
        private string control_area = "Location unknown";
        private bool DoOrDont_finished = false;
        CancellationTokenSource cts;

        private  int duration_DoORDont { get;  set; }
        private bool DoORDont { get;  set; }
        public Task Initialization { get; private set; }
        public MainPage()
        {
            InitializeComponent();
            Initialization = InitAsync();

        }

        private async Task InitAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.None)
            {
                if ((Preferences.ContainsKey("control_area")) & (Preferences.ContainsKey("location_name")))
                {
                    //code for communicating with  the backend
                    location_name = Preferences.Get("control_area", "Location unknown");
                    control_area = Preferences.Get("control_area", "Location unknown");
                    Constants.location_name_global = location_name;
                    DoOrDont_finished =await DoOrDontUseEnergy(control_area);

                    location_Button.Text = location_name;
                    if (duration_DoORDont > 1)
                    {
                        duration_Label.Text = "Noch " + duration_DoORDont.ToString() + " Stunden";
                    }
                    else
                    {
                        duration_Label.Text = "Noch " + duration_DoORDont.ToString() + " Stunde";
                    }

                    if (DoORDont)
                    {
                        location_Button.TextColor = Color.FromRgb(24, 91, 59);
                        learn_more_Button.BackgroundColor = Color.FromRgb(24, 91, 59);
                        DoORDont_Image.Source = ImageSource.FromResource("WaiveApp.Images.DO.png", typeof(MainPage).GetTypeInfo().Assembly);
                        if (duration_DoORDont <= 2)
                        {
                            //duration_Label.
                            Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivegreen_peak_left.png", typeof(MainPage).GetTypeInfo().Assembly);
                        }
                        else if ((duration_DoORDont < 6) & (duration_DoORDont > 2))
                        {
                            Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivegreen_peak_center.png", typeof(MainPage).GetTypeInfo().Assembly);
                        }
                        else if (duration_DoORDont >= 6)
                        {
                            Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivegreen_peak_right.png", typeof(MainPage).GetTypeInfo().Assembly);
                        }
                    }
                    else
                    {
                        location_Button.TextColor = Color.FromRgb(149, 35, 35);
                        learn_more_Button.BackgroundColor = Color.FromRgb(149, 35, 35);
                        DoORDont_Image.Source = ImageSource.FromResource("WaiveApp.Images.DONT.png", typeof(MainPage).GetTypeInfo().Assembly);
                        if (duration_DoORDont >= 6)
                        {
                            Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivered_peak_left.png", typeof(MainPage).GetTypeInfo().Assembly);
                        }
                        else if ((duration_DoORDont < 6) & (duration_DoORDont > 2))
                        {
                            Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivered_bottom_center.png", typeof(MainPage).GetTypeInfo().Assembly);
                        }
                        else if (duration_DoORDont <= 2)
                        {
                            Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivered_peak_right.png", typeof(MainPage).GetTypeInfo().Assembly);
                        }
                    }
                }
                else
                {
                    location_Button.Text = location_name;
                    await DetermineLocation();
                }
            }
            else
            {
                NoInternetDisplayAlert();
            }

        }

        async void NoInternetDisplayAlert()
        {
            await DisplayAlert("No connection", "There is no internet connectivity", "OK");
        }
        async void ChangeLocationButtonClicked(object sender, EventArgs args)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.None)
            {
                var actionSheet = await DisplayActionSheet("Determine your new location through:", "Cancel", null, "GPS", "State");
                Location location;
                GeolocationRequest request;
                string state;

                switch (actionSheet)
                {
                    case "Cancel":

                        break;

                    case "GPS":

                        request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                        cts = new CancellationTokenSource();
                        location = await Geolocation.GetLocationAsync(request, cts.Token);
                        if (location != null)
                        {

                            //var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);

                            //                    var placemarks = await Geocoding.GetPlacemarksAsync(47.773227556152754, 10.410373664954003);
                            var placemarks = await Geocoding.GetPlacemarksAsync(52.496084, 13.323164);
                            var placemark = placemarks?.FirstOrDefault();
                            if (placemark != null)
                            {
                                state = placemark.AdminArea;
                                if (state == "Berlin")
                                    location_name = state + ", " + placemark.SubLocality;
                                else
                                    location_name = state + ", " + placemark.Locality;


                                switch (state)
                                {
                                    case "Bayern":
                                        switch (placemark.SubAdminArea)
                                        {
                                            case "Schwaben":
                                                control_area = "Amprion";
                                                break;

                                            default:
                                                control_area = "Tennet";
                                                break;

                                        }

                                        break;

                                    case "Baden-Württemberg":
                                        location_name = "Baden-Württemberg";
                                        control_area = "TransnetBW";
                                        break;

                                    case "Berlin":
                                        control_area = "50Hertz";
                                        break;
                                    case "Brandenburg":
                                        control_area = "50Hertz";
                                        break;
                                    case "Bremen":
                                        control_area = "Tennet";
                                        break;
                                    case "Hamburg":
                                        control_area = "50Hertz";
                                        break;
                                    case "Hessen":
                                        control_area = "Tennet";
                                        break;
                                    case "Mecklenburg-Vorpommern":
                                        control_area = "50Hertz";
                                        break;
                                    case "Niedersachsen":
                                        control_area = "Tennet";
                                        break;
                                    case "Nordrhein-Westfalen":
                                        control_area = "Amprion";
                                        break;
                                    case "Rheinland-Pfalz":
                                        control_area = "Amprion";
                                        break;
                                    case "Saarland":
                                        control_area = "Amprion";
                                        break;
                                    case "Sachsen":
                                        control_area = "50Hertz";
                                        break;
                                    case "Sachsen-Anhalt":
                                        control_area = "50Hertz";
                                        break;
                                    case "Schleswig-Holstein":
                                        control_area = "Tennet";
                                        break;
                                    case "Thüringen":
                                        control_area = "50Hertz";
                                        break;

                                    default:
                                        break;

                                }
                            }
                        }
                        Constants.location_name_global = location_name;
                        DoOrDont_finished =await DoOrDontUseEnergy(control_area);

                        location_Button.Text = location_name;
                        if (duration_DoORDont > 1)
                        {
                            duration_Label.Text = "Noch " + duration_DoORDont.ToString() + " Stunden";
                        }
                        else
                        {
                            duration_Label.Text = "Noch " + duration_DoORDont.ToString() + " Stunde";
                        }

                        if (DoORDont)
                        {
                            location_Button.TextColor = Color.FromRgb(24, 91, 59);
                            learn_more_Button.BackgroundColor = Color.FromRgb(24, 91, 59);
                            DoORDont_Image.Source = ImageSource.FromResource("WaiveApp.Images.DO.png", typeof(MainPage).GetTypeInfo().Assembly);
                            if (duration_DoORDont <= 2)
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivegreen_peak_left.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                            else if ((duration_DoORDont < 6) & (duration_DoORDont > 2))
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivegreen_peak_center.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                            else if (duration_DoORDont >= 6)
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivegreen_peak_right.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                        }
                        else
                        {
                            location_Button.TextColor = Color.FromRgb(149, 35, 35);
                            learn_more_Button.BackgroundColor = Color.FromRgb(149, 35, 35);
                            DoORDont_Image.Source = ImageSource.FromResource("WaiveApp.Images.DONT.png", typeof(MainPage).GetTypeInfo().Assembly);
                            if (duration_DoORDont >= 6)
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivered_peak_left.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                            else if ((duration_DoORDont < 6) & (duration_DoORDont > 2))
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivered_bottom_center.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                            else if (duration_DoORDont <= 2)
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivered_peak_right.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                        }

                        Preferences.Set("control_area", location_name);
                        Preferences.Set("control_area", control_area);

                        break;

                    case "State":

                        var federal_state = await DisplayActionSheet("Choose your federal state:", "Cancel", null, "Baden-Württemberg", "Bayern", "Berlin", "Brandenburg", "Bremen", "Hamburg", "Hessen",
                            "Mecklenburg-Vorpommern", "Niedersachsen", "Nordrhein-Westfalen", "Rheinland-Pfalz", "Saarland", "Sachsen", "Sachsen-Anhalt", "Schleswig-Holstein", "Thüringen");


                        switch (federal_state)
                        {
                            case "Bayern":
                                var ifBayern = await DisplayAlert("Administrative region", "Is the administrative region Schwaben?", "Yes", "No");
                                switch (ifBayern)
                                {
                                    case true:
                                        location_name = "Schwaben, Bayern";
                                        control_area = "Amprion";
                                        break;
                                    case false:
                                        location_name = "Bayern";
                                        control_area = "Tennet";
                                        break;
                                    default:
                                        break;

                                }

                                break;

                            case "Baden-Württemberg":
                                location_name = "Baden-Württemberg";
                                control_area = "TransnetBW";
                                break;

                            case "Berlin":
                                location_name = "Berlin";
                                control_area = "50Hertz";
                                break;
                            case "Brandenburg":
                                location_name = "Brandenburg";
                                control_area = "50Hertz";
                                break;
                            case "Bremen":
                                location_name = "Bremen";
                                control_area = "Tennet";
                                break;
                            case "Hamburg":
                                location_name = "Hamburg";
                                control_area = "50Hertz";
                                break;
                            case "Hessen":
                                location_name = "Hessen";
                                control_area = "Tennet";
                                break;
                            case "Mecklenburg-Vorpommern":
                                location_name = "Mecklenburg-Vorpommern";
                                control_area = "50Hertz";
                                break;
                            case "Niedersachsen":
                                location_name = "Niedersachsen";
                                control_area = "Tennet";
                                break;
                            case "Nordrhein-Westfalen":
                                location_name = "Nordrhein-Westfalen";
                                control_area = "Amprion";
                                break;
                            case "Rheinland-Pfalz":
                                location_name = "Rheinland-Pfalz";
                                control_area = "Amprion";
                                break;
                            case "Saarland":
                                location_name = "Saarland";
                                control_area = "Amprion";
                                break;
                            case "Sachsen":
                                location_name = "Sachsen";
                                control_area = "50Hertz";
                                break;
                            case "Sachsen-Anhalt":
                                location_name = "Sachsen-Anhalt";
                                control_area = "50Hertz";
                                break;
                            case "Schleswig-Holstein":
                                location_name = "Schleswig-Holstein";
                                control_area = "Tennet";
                                break;
                            case "Thüringen":
                                location_name = "Thüringen";
                                control_area = "50Hertz";
                                break;

                            default:
                                break;



                        }
                        Constants.location_name_global = location_name;
                        DoOrDont_finished =await DoOrDontUseEnergy(control_area);

                        location_Button.Text = location_name;

                        if (duration_DoORDont > 1)
                        {
                            duration_Label.Text = "Noch " + duration_DoORDont.ToString() + " Stunden";
                        }
                        else
                        {
                            duration_Label.Text = "Noch " + duration_DoORDont.ToString() + " Stunde";
                        }

                        if (DoORDont)
                        {
                            location_Button.TextColor = Color.FromRgb(24, 91, 59);
                            learn_more_Button.BackgroundColor = Color.FromRgb(24, 91, 59);
                            DoORDont_Image.Source = ImageSource.FromResource("WaiveApp.Images.DO.png", typeof(MainPage).GetTypeInfo().Assembly);
                            if (duration_DoORDont <= 2)
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivegreen_peak_left.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                            else if ((duration_DoORDont < 6) & (duration_DoORDont > 2))
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivegreen_peak_center.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                            else if (duration_DoORDont >= 6)
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivegreen_peak_right.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                        }
                        else
                        {
                            location_Button.TextColor = Color.FromRgb(149, 35, 35);
                            learn_more_Button.BackgroundColor = Color.FromRgb(149, 35, 35);
                            DoORDont_Image.Source = ImageSource.FromResource("WaiveApp.Images.DONT.png", typeof(MainPage).GetTypeInfo().Assembly);
                            if (duration_DoORDont >= 6)
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivered_peak_left.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                            else if ((duration_DoORDont < 6) & (duration_DoORDont > 2))
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivered_bottom_center.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                            else if (duration_DoORDont <= 2)
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivered_peak_right.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                        }

                        Preferences.Set("control_area", location_name);
                        Preferences.Set("control_area", control_area);

                        break;


                    default:
                        break;


                }
            }
            else
            {
                NoInternetDisplayAlert();
            }

        }

        public async Task<bool>DoOrDontUseEnergy(string region)
        {

            DoORDont = false;
            duration_DoORDont = 0;
            List<RenewableShareData> items = await App.RenewableShareManager.GetRenewableShareDataAsync(region);
            DateTime now = DateTime.Now;
            Constants.items_global = items.Where(item => item.dateTime_of_Prediction >= now).ToList();
            List<double> RenewableShares = new List<double>();
            List<int> RenewableShares_binary = new List<int>();

            if (items != null & items.Any())
            {
                Debug.WriteLine("enters the if");
                Debug.WriteLine(DateTime.Now.ToString());
                foreach (RenewableShareData item in items)
                {
                    
                    if (item.dateTime_of_Prediction>=DateTime.Now)
                    {
                        Debug.WriteLine(item.dateTime_of_Prediction);
                        RenewableShares.Add(item.renewableShare);
                    }
                    
                }
                double renewability_Border = RenewableShares.Min() + ((RenewableShares.Max() - RenewableShares.Min()) / 2);
                Constants.renewability_border_global = renewability_Border;
                foreach (double share in RenewableShares)
                {
                    if (share >= renewability_Border)
                        RenewableShares_binary.Add(1);
                    else
                        RenewableShares_binary.Add(0);
                }

                int RenewableSharesLength = RenewableShares_binary.Count();
                if (RenewableSharesLength > 0)
                {
                    Debug.WriteLine("enters the second if and "+ RenewableShares_binary[0]+" " +RenewableSharesLength);
                    if ((RenewableShares_binary[0] == 0) & (RenewableSharesLength > 1))
                    {
                        Debug.WriteLine("enters the third if and " );

                        duration_DoORDont = 1;
                        int i = 1;
                        DoORDont = false;
                        while (i < RenewableSharesLength)
                        {
                            Debug.WriteLine("enters the while" + RenewableShares[i]);
                            if (RenewableShares_binary[i] == 0)
                            {
                                Debug.WriteLine("enters the 5th if");
                                duration_DoORDont++;
                                Debug.WriteLine(duration_DoORDont);
                            }
                            else
                            {
                                i = RenewableSharesLength;
                            }
                            i++;
                        }

                    }

                    else if ((RenewableShares_binary[0] == 1) & (RenewableSharesLength > 1))
                    {
                        Debug.WriteLine("enters the fourth if and ");

                        duration_DoORDont = 1;
                        int i = 1;
                        DoORDont = true;
                        while (i < RenewableSharesLength)
                        {
                            Debug.WriteLine("enters the while"+ RenewableShares[i]);
                            if (RenewableShares_binary[i] == 1)
                            {
                                Debug.WriteLine("enters the 5th if");
                                duration_DoORDont++;
                                Debug.WriteLine(duration_DoORDont);
                            }
                            else
                            {
                                i = RenewableSharesLength;
                            }
                            i++;
                        }
                    }
                    else if ((RenewableShares_binary[0] == 1) & (RenewableSharesLength <= 1))
                    {
                        DoORDont = true;
                        duration_DoORDont = 1;
                    }
                    else if ((RenewableShares_binary[0] == 0) & (RenewableSharesLength <= 1))
                    {
                        DoORDont = false;
                        duration_DoORDont = 1;
                    }
                }

            }
            Debug.WriteLine(DoORDont.ToString() + " " + duration_DoORDont);
            return true;
            
        }
        

    


        private async Task DetermineLocation()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.None)
            {
                var actionSheet = await DisplayActionSheet("Determine your new location through:", "Cancel", null, "GPS", "State");
                Location location;
                GeolocationRequest request;
                string location_name = "";
                string control_area = "";
                string state;

                switch (actionSheet)
                {
                    case "Cancel":

                        break;

                    case "GPS":

                        request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                        cts = new CancellationTokenSource();
                        location = await Geolocation.GetLocationAsync(request, cts.Token);
                        if (location != null)
                        {

                            //var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);

                            //                    var placemarks = await Geocoding.GetPlacemarksAsync(47.773227556152754, 10.410373664954003);
                            var placemarks = await Geocoding.GetPlacemarksAsync(52.496084, 13.323164);
                            var placemark = placemarks?.FirstOrDefault();
                            if (placemark != null)
                            {
                                state = placemark.AdminArea;
                                if (state == "Berlin")
                                    location_name = state + ", " + placemark.SubLocality;
                                else
                                    location_name = state + ", " + placemark.Locality;


                                switch (state)
                                {
                                    case "Bayern":
                                        switch (placemark.SubAdminArea)
                                        {
                                            case "Schwaben":
                                                control_area = "Amprion";
                                                break;

                                            default:
                                                control_area = "Tennet";
                                                break;

                                        }

                                        break;

                                    case "Baden-Württemberg":
                                        location_name = "Baden-Württemberg";
                                        control_area = "TransnetBW";
                                        break;

                                    case "Berlin":
                                        control_area = "50Hertz";
                                        break;
                                    case "Brandenburg":
                                        control_area = "50Hertz";
                                        break;
                                    case "Bremen":
                                        control_area = "Tennet";
                                        break;
                                    case "Hamburg":
                                        control_area = "50Hertz";
                                        break;
                                    case "Hessen":
                                        control_area = "Tennet";
                                        break;
                                    case "Mecklenburg-Vorpommern":
                                        control_area = "50Hertz";
                                        break;
                                    case "Niedersachsen":
                                        control_area = "Tennet";
                                        break;
                                    case "Nordrhein-Westfalen":
                                        control_area = "Amprion";
                                        break;
                                    case "Rheinland-Pfalz":
                                        control_area = "Amprion";
                                        break;
                                    case "Saarland":
                                        control_area = "Amprion";
                                        break;
                                    case "Sachsen":
                                        control_area = "50Hertz";
                                        break;
                                    case "Sachsen-Anhalt":
                                        control_area = "50Hertz";
                                        break;
                                    case "Schleswig-Holstein":
                                        control_area = "Tennet";
                                        break;
                                    case "Thüringen":
                                        control_area = "50Hertz";
                                        break;

                                    default:
                                        break;

                                }
                            }
                        }
                        Constants.location_name_global = location_name;
                        DoOrDont_finished = await DoOrDontUseEnergy(control_area);

                        location_Button.Text = location_name;
                        if (duration_DoORDont > 1)
                        {
                            duration_Label.Text = "Noch " + duration_DoORDont.ToString() + " Stunden";
                        }
                        else
                        {
                            duration_Label.Text = "Noch " + duration_DoORDont.ToString() + " Stunde";
                        }

                        if (DoORDont)
                        {
                            location_Button.TextColor = Color.FromRgb(24, 91, 59);
                            learn_more_Button.BackgroundColor = Color.FromRgb(24, 91, 59);
                            DoORDont_Image.Source = ImageSource.FromResource("WaiveApp.Images.DO.png", typeof(MainPage).GetTypeInfo().Assembly);
                            if (duration_DoORDont <= 2)
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivegreen_peak_left.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                            else if ((duration_DoORDont < 6) & (duration_DoORDont > 2))
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivegreen_peak_center.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                            else if (duration_DoORDont >= 6)
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivegreen_peak_right.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                        }
                        else 
                        {
                            location_Button.TextColor = Color.FromRgb(149, 35, 35);
                            learn_more_Button.BackgroundColor = Color.FromRgb(149, 35, 35);
                            DoORDont_Image.Source = ImageSource.FromResource("WaiveApp.Images.DONT.png", typeof(MainPage).GetTypeInfo().Assembly);
                            if (duration_DoORDont >= 6)
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivered_peak_left.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                            else if ((duration_DoORDont < 6) & (duration_DoORDont > 2))
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivered_bottom_center.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                            else if (duration_DoORDont <= 2 )
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivered_peak_right.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                        }


                        Preferences.Set("control_area", location_name);
                        Preferences.Set("control_area", control_area);

                        break;

                    case "State":

                        var federal_state = await DisplayActionSheet("Choose your federal state:", "Cancel", null, "Baden-Württemberg", "Bayern", "Berlin", "Brandenburg", "Bremen", "Hamburg", "Hessen",
                            "Mecklenburg-Vorpommern", "Niedersachsen", "Nordrhein-Westfalen", "Rheinland-Pfalz", "Saarland", "Sachsen", "Sachsen-Anhalt", "Schleswig-Holstein", "Thüringen");


                        switch (federal_state)
                        {
                            case "Bayern":
                                var ifBayern = await DisplayAlert("Administrative region", "Is the administrative region Schwaben?", "Yes", "No");
                                switch (ifBayern)
                                {
                                    case true:
                                        location_name = "Schwaben, Bayern";
                                        control_area = "Amprion";
                                        break;
                                    case false:
                                        location_name = "Bayern";
                                        control_area = "Tennet";
                                        break;
                                    default:
                                        break;

                                }

                                break;

                            case "Baden-Württemberg":
                                location_name = "Baden-Württemberg";
                                control_area = "TransnetBW";
                                break;

                            case "Berlin":
                                location_name = "Berlin";
                                control_area = "50Hertz";
                                break;
                            case "Brandenburg":
                                location_name = "Brandenburg";
                                control_area = "50Hertz";
                                break;
                            case "Bremen":
                                location_name = "Bremen";
                                control_area = "Tennet";
                                break;
                            case "Hamburg":
                                location_name = "Hamburg";
                                control_area = "50Hertz";
                                break;
                            case "Hessen":
                                location_name = "Hessen";
                                control_area = "Tennet";
                                break;
                            case "Mecklenburg-Vorpommern":
                                location_name = "Mecklenburg-Vorpommern";
                                control_area = "50Hertz";
                                break;
                            case "Niedersachsen":
                                location_name = "Niedersachsen";
                                control_area = "Tennet";
                                break;
                            case "Nordrhein-Westfalen":
                                location_name = "Nordrhein-Westfalen";
                                control_area = "Amprion";
                                break;
                            case "Rheinland-Pfalz":
                                location_name = "Rheinland-Pfalz";
                                control_area = "Amprion";
                                break;
                            case "Saarland":
                                location_name = "Saarland";
                                control_area = "Amprion";
                                break;
                            case "Sachsen":
                                location_name = "Sachsen";
                                control_area = "50Hertz";
                                break;
                            case "Sachsen-Anhalt":
                                location_name = "Sachsen-Anhalt";
                                control_area = "50Hertz";
                                break;
                            case "Schleswig-Holstein":
                                location_name = "Schleswig-Holstein";
                                control_area = "Tennet";
                                break;
                            case "Thüringen":
                                location_name = "Thüringen";
                                control_area = "50Hertz";
                                break;

                            default:
                                break;



                        }
                        Constants.location_name_global = location_name;

                        DoOrDont_finished = await DoOrDontUseEnergy(control_area);

                        location_Button.Text = location_name;
                        if (duration_DoORDont > 1)
                        {
                            duration_Label.Text = "Noch " + duration_DoORDont.ToString() + " Stunden";
                        }
                        else
                        {
                            duration_Label.Text = "Noch " + duration_DoORDont.ToString() + " Stunde";
                        }

                        if (DoORDont)
                        {
                            location_Button.TextColor = Color.FromRgb(24, 91, 59);
                            learn_more_Button.BackgroundColor = Color.FromRgb(24, 91, 59);
                            DoORDont_Image.Source = ImageSource.FromResource("WaiveApp.Images.DO.png", typeof(MainPage).GetTypeInfo().Assembly);
                            if (duration_DoORDont <= 2)
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivegreen_peak_left.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                            else if ((duration_DoORDont < 6) & (duration_DoORDont > 2))
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivegreen_peak_center.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                            else if (duration_DoORDont >= 6)
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivegreen_peak_right.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                        }
                        else
                        {
                            location_Button.TextColor = Color.FromRgb(149, 35, 35);
                            learn_more_Button.BackgroundColor = Color.FromRgb(149, 35, 35);
                            DoORDont_Image.Source = ImageSource.FromResource("WaiveApp.Images.DONT.png", typeof(MainPage).GetTypeInfo().Assembly);
                            if (duration_DoORDont >= 6)
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivered_peak_left.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                            else if ((duration_DoORDont < 6) & (duration_DoORDont > 2))
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivered_bottom_center.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                            else if (duration_DoORDont <= 2)
                            {
                                Chart_Image.Source = ImageSource.FromResource("WaiveApp.Images.waivered_peak_right.png", typeof(MainPage).GetTypeInfo().Assembly);
                            }
                        }

                        Preferences.Set("control_area", location_name);
                        Preferences.Set("control_area", control_area);

                        break;


                    default:
                        break;


                }
            }
            else
            {
                NoInternetDisplayAlert();
            }

        }

        async void LearnMoreButtonClicked(object sender, EventArgs args)
        {

            await Navigation.PushAsync(new detaillierte_Info());

        }
        async void UeberUnsButtonClicked(object sender, EventArgs args)
        {

            await Navigation.PushAsync(new Impressum());

        }
        
    }
}
