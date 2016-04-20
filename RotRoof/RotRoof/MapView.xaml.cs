
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Maps.MapControl.WPF;
using RotRoof.GeocodeService;


namespace RotRoof
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        // The user defined polygon to add to the map.
        MapPolygon newPolygon = null;
        MapPolygon newPolygon2 = null;

        // The map layer containing the polygon points defined by the user.
        MapLayer polygonPointLayer = new MapLayer();

        public MapView()
        {

            //ConsoleManager.Show();
            InitializeComponent();
            
            
            //Set focus to map
            MapWithPolygon.Focus();

            
            DBConnection test = new DBConnection();
            List<string> properties = new List<string>();
            var MaxRoof = test.Select("SELECT MAX(name) FROM (SELECT neighbourhood.name from location, street, neighbourhood, robbery, item WHERE item.fk_robbery = robbery.uid AND item.fk_itemtype = 2 AND robbery.fk_location = location.uid AND location.fk_street = street.uid AND street.fk_neighbourhood = neighbourhood.uid) as name");
            var MinRoof = test.Select("SELECT MIN(name) FROM (SELECT neighbourhood.name from location, street, neighbourhood, robbery, item WHERE item.fk_robbery = robbery.uid AND item.fk_itemtype = 2 AND robbery.fk_location = location.uid AND location.fk_street = street.uid AND street.fk_neighbourhood = neighbourhood.uid) as name");
            var MaxFietsRoof = test.Select("SELECT MAX(name) FROM (SELECT neighbourhood.name from location, street, neighbourhood, robbery, item WHERE item.fk_robbery = robbery.uid AND item.fk_itemtype = 1 AND robbery.fk_location = location.uid AND location.fk_street = street.uid AND street.fk_neighbourhood = neighbourhood.uid) as name");
            var MinFietsRoof = test.Select("SELECT MIN(name) FROM (SELECT neighbourhood.name from location, street, neighbourhood, robbery, item WHERE item.fk_robbery = robbery.uid AND item.fk_itemtype = 1 AND robbery.fk_location = location.uid AND location.fk_street = street.uid AND street.fk_neighbourhood = neighbourhood.uid) as name");
            /*
            foreach (List<string> entry in MinFietsRoof)
            {
                //roof 
                Console.WriteLine(entry[0]);
                GeocodeAddress("Schiebroek", 1, 1);


            }
            foreach (List<string> entry in MinRoof)
            {
                //roof 
                Console.WriteLine(entry[0]);
                GeocodeAddress("Vogelenzang Rotterdam", 1, 1);


            }*/
            foreach (List<string> entry in MaxRoof)
            {
                //roof 
                    //Console.WriteLine("Buurtnaam waar het meeste roof word geshit = " + entry[0]);
                GeocodeAddress(entry[0], 0 ,1);
            }

            foreach (List<string> entry in MaxFietsRoof)
            {
                //fietsroof 
                //Console.WriteLine("Buurtnaam waar het meeste fietsroof word geshit = " + entry[0]);
                GeocodeAddress(entry[0],0,1);
            }
            

        }


        private String GeocodeAddress(string address, int amount, int type)
        {
            string results = "";
            string key = "AtpRhAYzndch3AqUr1PzBZN3cmcnOremPqynI7oxAuwTya6SQ1582q7N0nBXSucT";
            GeocodeRequest geocodeRequest = new GeocodeRequest();
            // Set the credentials using a valid Bing Maps key
            geocodeRequest.Credentials = new Microsoft.Maps.MapControl.WPF.Credentials();
            geocodeRequest.Credentials.ApplicationId = key;

            // Set the full address query
            geocodeRequest.Query = address;

            // Set the options to only return high confidence results 
            ConfidenceFilter[] filters = new ConfidenceFilter[1];
            filters[0] = new ConfidenceFilter();
            filters[0].MinimumConfidence = GeocodeService.Confidence.High;

            // Add the filters to the options
            GeocodeOptions geocodeOptions = new GeocodeOptions();
            geocodeOptions.Filters = filters;
            geocodeRequest.Options = geocodeOptions;

            // Make the geocode request
            GeocodeServiceClient geocodeService = new GeocodeServiceClient();
            GeocodeResponse geocodeResponse = geocodeService.Geocode(geocodeRequest);

            if (geocodeResponse.Results.Length > 0)
            {
                results = String.Format("Latitude: {0}\nLongitude: {1}",
                  geocodeResponse.Results[0].Locations[0].Latitude,
                  geocodeResponse.Results[0].Locations[0].Longitude);
                if(type == 1)
                {
                    if(amount > 0)
                    {
                        CreateBigRoundPoly(geocodeResponse.Results[0].Locations[0].Latitude, geocodeResponse.Results[0].Locations[0].Longitude, address, "FFFFFF", "008651");
                    }
                    else
                    {
                        //CALL CREATEROUNDPOLY FUNCTION 
                        CreateBigRoundPoly(geocodeResponse.Results[0].Locations[0].Latitude, geocodeResponse.Results[0].Locations[0].Longitude, address, "FFFFFF", "ed1212");
                    }
                    
                }
                else if(type == 2)
                {
                    CreateSmallRoundPoly(geocodeResponse.Results[0].Locations[0].Latitude, geocodeResponse.Results[0].Locations[0].Longitude, address,amount, "FFFFFF", "008651");
                }
            }
            else
                results = "No Results Found";

            return results;
        }

        public void CreateSmallRoundPoly(double latitude, double longitude, string name,int amount, string borderColor, string fillColor)
        {
            newPolygon2 = new MapPolygon();

            // Defines the polygon fill details
            newPolygon2.Locations = new LocationCollection();
            newPolygon2.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#" + fillColor));
            newPolygon2.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#" + borderColor));
            newPolygon2.StrokeThickness = 3;
            newPolygon2.Opacity = 0.4;

            //Set focus back to the map so that +/- work for zoom in/out
            MapWithPolygon.Focus();
            //LATITUDE == HOOGTE & LONGITUDE == BREEDTE
            newPolygon2.Locations = new LocationCollection()
            {
                new Microsoft.Maps.MapControl.WPF.Location(latitude - 0.002, longitude + 0.004),
                new Microsoft.Maps.MapControl.WPF.Location(latitude + 0.002, longitude + 0.004),
                new Microsoft.Maps.MapControl.WPF.Location(latitude + 0.003, longitude + 0.002),
                new Microsoft.Maps.MapControl.WPF.Location(latitude + 0.003, longitude-0.002),
                new Microsoft.Maps.MapControl.WPF.Location(latitude + 0.002, longitude - 0.004),
                new Microsoft.Maps.MapControl.WPF.Location(latitude - 0.002, longitude - 0.004),
                new Microsoft.Maps.MapControl.WPF.Location(latitude - 0.003, longitude - 0.002),
                new Microsoft.Maps.MapControl.WPF.Location(latitude - 0.003, longitude + 0.002)
            };
            MapWithPolygon.Children.Add(newPolygon2);


            ControlTemplate template = (ControlTemplate)this.FindResource("CutomPushpinTemplate");

            
            
            // The pushpin to add to the map.
            Pushpin pin = new Pushpin();
            pin.Location = new Microsoft.Maps.MapControl.WPF.Location(latitude, longitude);
            pin.Template = template;
            pin.PositionOrigin = PositionOrigin.BottomLeft;

            //TEXT IN PUSHPIN
            pin.Content = name;

            pin.MouseDown += new MouseButtonEventHandler(pin_MouseDown);

            // Adds the pushpin to the map.
            MapWithPolygon.Children.Add(pin);


        }
        public void CreateBigRoundPoly(double latitude, double longitude, string name, string borderColor, string fillColor)
        {
            newPolygon2 = new MapPolygon();

            // Defines the polygon fill details
            newPolygon2.Locations = new LocationCollection();
            newPolygon2.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#"+ fillColor));
            newPolygon2.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#"+ borderColor));
            newPolygon2.StrokeThickness = 3;
            newPolygon2.Opacity = 0.4;

            //Set focus back to the map so that +/- work for zoom in/out
            MapWithPolygon.Focus();
            //LATITUDE == HOOGTE & LONGITUDE == BREEDTE
            newPolygon2.Locations = new LocationCollection()
            {
                new Microsoft.Maps.MapControl.WPF.Location(latitude - 0.01, longitude + 0.03),
                new Microsoft.Maps.MapControl.WPF.Location(latitude + 0.01, longitude + 0.03),
                new Microsoft.Maps.MapControl.WPF.Location(latitude + 0.02, longitude + 0.01),
                new Microsoft.Maps.MapControl.WPF.Location(latitude + 0.02, longitude-0.01),
                new Microsoft.Maps.MapControl.WPF.Location(latitude + 0.01, longitude - 0.03),
                new Microsoft.Maps.MapControl.WPF.Location(latitude - 0.01, longitude - 0.03),
                new Microsoft.Maps.MapControl.WPF.Location(latitude - 0.02, longitude - 0.01),
                new Microsoft.Maps.MapControl.WPF.Location(latitude - 0.02, longitude + 0.01)
            };
            MapWithPolygon.Children.Add(newPolygon2);


            ControlTemplate template = (ControlTemplate)this.FindResource("CutomPushpinTemplate");



            // The pushpin to add to the map.
            Pushpin pin = new Pushpin();
            pin.Location = new Microsoft.Maps.MapControl.WPF.Location(latitude, longitude);
            pin.Template = template;
            pin.PositionOrigin = PositionOrigin.BottomLeft;

            //TEXT IN PUSHPIN
            pin.Content = name;

            pin.MouseDown += new MouseButtonEventHandler(pin_MouseDown);

            // Adds the pushpin to the map.
            MapWithPolygon.Children.Add(pin);


        }
        private void pin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Pinpoint Clicked Aids");
        }
        
        
        
 


        [SuppressUnmanagedCodeSecurity]
        public static class ConsoleManager
        {
            private const string Kernel32_DllName = "kernel32.dll";

            [DllImport(Kernel32_DllName)]
            private static extern bool AllocConsole();

            [DllImport(Kernel32_DllName)]
            private static extern bool FreeConsole();

            [DllImport(Kernel32_DllName)]
            private static extern IntPtr GetConsoleWindow();

            [DllImport(Kernel32_DllName)]
            private static extern int GetConsoleOutputCP();

            public static bool HasConsole
            {
                get { return GetConsoleWindow() != IntPtr.Zero; }
            }

            /// <summary>
            /// Creates a new console instance if the process is not attached to a console already.
            /// </summary>
            public static void Show()
            {
                //#if DEBUG
                if (!HasConsole)
                {
                    AllocConsole();
                    InvalidateOutAndError();
                }
                //#endif
            }

            /// <summary>
            /// If the process has a console attached to it, it will be detached and no longer visible. Writing to the System.Console is still possible, but no output will be shown.
            /// </summary>
            public static void Hide()
            {
                //#if DEBUG
                if (HasConsole)
                {
                    SetOutAndErrorNull();
                    FreeConsole();
                }
                //#endif
            }

            public static void Toggle()
            {
                if (HasConsole)
                {
                    Hide();
                }
                else
                {
                    Show();
                }
            }

            static void InvalidateOutAndError()
            {
                Type type = typeof(System.Console);

                System.Reflection.FieldInfo _out = type.GetField("_out",
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

                System.Reflection.FieldInfo _error = type.GetField("_error",
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

                System.Reflection.MethodInfo _InitializeStdOutError = type.GetMethod("InitializeStdOutError",
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

                Debug.Assert(_out != null);
                Debug.Assert(_error != null);

                Debug.Assert(_InitializeStdOutError != null);

                _out.SetValue(null, null);
                _error.SetValue(null, null);

                _InitializeStdOutError.Invoke(null, new object[] { true });
            }

            static void SetOutAndErrorNull()
            {
                Console.SetOut(TextWriter.Null);
                Console.SetError(TextWriter.Null);
            }
        }

        
    }
}
