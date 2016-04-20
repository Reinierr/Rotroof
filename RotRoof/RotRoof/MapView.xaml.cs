
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
        }
      

        private String GeocodeAddress(string address)
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

                //CALL CREATEROUNDPOLY FUNCTION 
                CreateRoundPoly(geocodeResponse.Results[0].Locations[0].Latitude,
                  geocodeResponse.Results[0].Locations[0].Longitude);
            }
            else
                results = "No Results Found";

            return results;
        }

        public void CreateRoundPoly(double latitude, double longitude)
        {
            newPolygon2 = new MapPolygon();

            // Defines the polygon fill details
            newPolygon2.Locations = new LocationCollection();
            newPolygon2.Fill = new SolidColorBrush(Colors.Green);
            newPolygon2.Stroke = new SolidColorBrush(Colors.DarkGreen);
            newPolygon2.StrokeThickness = 3;
            newPolygon2.Opacity = 0.4;

            //Set focus back to the map so that +/- work for zoom in/out
            MapWithPolygon.Focus();
            //LATITUDE == HOOGTE & LONGITUDE == BREEDTE
            newPolygon2.Locations = new LocationCollection()
            {
                new Microsoft.Maps.MapControl.WPF.Location(latitude - 0.001, longitude + 0.003),
                new Microsoft.Maps.MapControl.WPF.Location(latitude + 0.001, longitude + 0.003),
                new Microsoft.Maps.MapControl.WPF.Location(latitude + 0.002, longitude + 0.001),
                new Microsoft.Maps.MapControl.WPF.Location(latitude + 0.002, longitude-0.001),
                new Microsoft.Maps.MapControl.WPF.Location(latitude + 0.001, longitude - 0.003),
                new Microsoft.Maps.MapControl.WPF.Location(latitude - 0.001, longitude - 0.003),
                new Microsoft.Maps.MapControl.WPF.Location(latitude - 0.002, longitude - 0.001),
                new Microsoft.Maps.MapControl.WPF.Location(latitude - 0.002, longitude + 0.001)
            };
            MapWithPolygon.Children.Add(newPolygon2);


            ControlTemplate template = (ControlTemplate)this.FindResource("CutomPushpinTemplate");

            
            
            // The pushpin to add to the map.
            Pushpin pin = new Pushpin();
            pin.Location = new Microsoft.Maps.MapControl.WPF.Location(latitude, longitude);
            pin.Template = template;
            pin.PositionOrigin = PositionOrigin.BottomLeft;

            //TEXT IN PUSHPIN
            pin.Content = "Test";

            pin.MouseDown += new MouseButtonEventHandler(pin_MouseDown);

            // Adds the pushpin to the map.
            MapWithPolygon.Children.Add(pin);


        }
        private void pin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Pinpoint Clicked Aids");
        }
        
        private void CreatePoly(object sender, RoutedEventArgs e)
        {
            double X = 51.91714;
            double Y = 4.49037;

            CreateRoundPoly(X, Y);
        }

        private void GeocodeAddress(object sender, RoutedEventArgs e)
        {
            GeocodeAddress("Middeland");
            /*string test = "0.00";
            Random rnd = new Random();
            int randNumber = rnd.Next(1, 10);

            double add = Convert.ToDouble(test + randNumber);
            Console.WriteLine(add);
            

            double Y = 51.91714 + add;
            double X = 4.49037 + add;

            CreateRoundPoly(Y, X);*/
        }

        private void ClearPolygon(object sender, RoutedEventArgs e)
        {
            labelResults.Content = GeocodeAddress("MIDDELAND");
            //MapWithPolygon.Children.Clear();
            /*MapWithPolygon.Children.Remove(newPolygon);
            MapWithPolygon.Children.Remove(newPolygon2);*/
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
