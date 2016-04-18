using Microsoft.Maps.MapControl.WPF;
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
            
            
            

            /*newPolygon.Locations = new LocationCollection()
            {
                //51.917460, 4.485536
                //51.9174, 4.4855 center
                /*
                Y  , X
                78 , 55
                76 , 57
                74 , 59
                72 , 57
                70 , 55
                72 , 53
                74 , 51
                76 , 53
                78 , 55

                
                
                
                
                //Y,X
                //stappen van 4
                //51.9174, 4.4855 center
                new Location(51.9178, 4.4853),
                new Location(51.9178, 4.4857),
                new Location(51.9176, 4.4860),
                new Location(51.9174, 4.4860),
                new Location(51.9172, 4.4857),
                new Location(51.9172, 4.4853),
                new Location(51.9174, 4.4850),
                new Location(51.9176, 4.4850)
                
            };
            MapWithPolygon.Children.Add(newPolygon);*/

        }

        public void CreateRoundPoly(double longitude, double latitude )
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

            newPolygon2.Locations = new LocationCollection()
            {
                new Microsoft.Maps.MapControl.WPF.Location(longitude + 0.004, latitude - 0.002),
                new Microsoft.Maps.MapControl.WPF.Location(longitude + 0.004, latitude + 0.002),
                new Microsoft.Maps.MapControl.WPF.Location(longitude + 0.002, latitude + 0.005),
                new Microsoft.Maps.MapControl.WPF.Location(longitude, latitude + 0.005),
                new Microsoft.Maps.MapControl.WPF.Location(longitude - 0.002, latitude + 0.002),
                new Microsoft.Maps.MapControl.WPF.Location(longitude - 0.002, latitude - 0.002),
                new Microsoft.Maps.MapControl.WPF.Location(longitude, latitude - 0.005),
                new Microsoft.Maps.MapControl.WPF.Location(longitude + 0.002, latitude - 0.005)
            };
            MapWithPolygon.Children.Add(newPolygon2);


            ControlTemplate template = (ControlTemplate)this.FindResource("CutomPushpinTemplate");

            
            
            // The pushpin to add to the map.
            Pushpin pin = new Pushpin();
            pin.Location = new Microsoft.Maps.MapControl.WPF.Location(longitude, latitude);
            pin.Template = template;
            pin.PositionOrigin = PositionOrigin.BottomLeft;

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
            double Y = 51.91714;
            double X = 4.49037;

            CreateRoundPoly(Y, X);
        }

        private void CreateRandPoly(object sender, RoutedEventArgs e)
        {
            string test = "0.00";
            Random rnd = new Random();
            int randNumber = rnd.Next(1, 10);

            double add = Convert.ToDouble(test + randNumber);
            Console.WriteLine(add);
            

            double Y = 51.91714 + add;
            double X = 4.49037 + add;

            CreateRoundPoly(Y, X);
        }

        private void ClearPolygon(object sender, RoutedEventArgs e)
        {
            //MapWithPolygon.Children.Clear();
            MapWithPolygon.Children.Remove(newPolygon);
            MapWithPolygon.Children.Remove(newPolygon2);
        }


        /*private void SetUpNewPolygon()
        {
            newPolygon = new MapPolygon();
            // Defines the polygon fill details
            newPolygon.Locations = new LocationCollection();
            newPolygon.Fill = new SolidColorBrush(Colors.Blue);
            newPolygon.Stroke = new SolidColorBrush(Colors.Green);
            newPolygon.StrokeThickness = 3;
            newPolygon.Opacity = 0.4;
            //Set focus back to the map so that +/- work for zoom in/out
            MapWithPolygon.Focus();
        }*/

        /*private void MapWithPolygon_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            // Creates a location for a single polygon point and adds it to
            // the polygon's point location list.
            Point mousePosition = e.GetPosition(this);
            //Convert the mouse coordinates to a location on the map
            Location polygonPointLocation = MapWithPolygon.ViewportPointToLocation(
                mousePosition);
            
            newPolygon.Locations.Add(polygonPointLocation);

            
            

            // A visual representation of a polygon point.
            Rectangle r = new Rectangle();
            r.Fill = new SolidColorBrush(Colors.Red);
            r.Stroke = new SolidColorBrush(Colors.Yellow);
            r.StrokeThickness = 1;
            r.Width = 8;
            r.Height = 8;

            // Adds a small square where the user clicked, to mark the polygon point.
            polygonPointLayer.AddChild(r, polygonPointLocation);
            // Adds a small square where the user clicked, to mark the polygon point.
            //Set focus back to the map so that +/- work for zoom in/out
            MapWithPolygon.Focus();

        }*/

        /*private void btnCreatePolygon_Click(object sender, RoutedEventArgs e)
        {
            //If there are two or more points, add the polygon layer to the map
            if (newPolygon.Locations.Count >= 2)
            {
                // Removes the polygon points layer.
                polygonPointLayer.Children.Clear();

                // Adds the filled polygon layer to the map.
                NewPolygonLayer.Children.Add(newPolygon);
                SetUpNewPolygon();
            }
        }*/



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
