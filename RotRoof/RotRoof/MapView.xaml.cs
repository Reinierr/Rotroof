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
                new Microsoft.Maps.MapControl.WPF.Location(longitude + 0.002, latitude - 0.001),
                new Microsoft.Maps.MapControl.WPF.Location(longitude + 0.002, latitude + 0.001),
                new Microsoft.Maps.MapControl.WPF.Location(longitude + 0.001, latitude + 0.0025),
                new Microsoft.Maps.MapControl.WPF.Location(longitude, latitude + 0.0025),
                new Microsoft.Maps.MapControl.WPF.Location(longitude - 0.001, latitude + 0.001),
                new Microsoft.Maps.MapControl.WPF.Location(longitude - 0.001, latitude - 0.001),
                new Microsoft.Maps.MapControl.WPF.Location(longitude, latitude - 0.0025),
                new Microsoft.Maps.MapControl.WPF.Location(longitude + 0.001, latitude - 0.0025)
            };
            MapWithPolygon.Children.Add(newPolygon2);

            ControlTemplate template = (ControlTemplate)this.FindResource("CutomPushpinTemplate");

            // The pushpin to add to the map.
            Pushpin pin = new Pushpin();
            pin.Location = new Microsoft.Maps.MapControl.WPF.Location(longitude, latitude);
            pin.Template = template;
            pin.PositionOrigin = PositionOrigin.BottomLeft;
            pin.Content = "Veilig";
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
