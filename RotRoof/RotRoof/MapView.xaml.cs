
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
using System.Collections.ObjectModel;


namespace RotRoof
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        //polygon to add to the map.
        MapPolygon CustomPolygon = null;

        // The map layer containing the polygon points
        MapLayer polygonPointLayer = new MapLayer();

        public MapView()
        {
            InitializeComponent();

            //Set focus to map
            MapWithPolygon.Focus();

            //Setup Database Connection
            DBConnection FRDatabase = new DBConnection();
            List<string> properties = new List<string>();

            //SQL Queries for Database
            var MaxRoof = FRDatabase.Select("SELECT MAX(name) FROM (SELECT neighbourhood.name from location, street, neighbourhood, robbery, item WHERE item.fk_robbery = robbery.uid AND item.fk_itemtype = 2 AND robbery.fk_location = location.uid AND location.fk_street = street.uid AND street.fk_neighbourhood = neighbourhood.uid) as name");
            var MinRoof = FRDatabase.Select("SELECT MIN(name) FROM (SELECT neighbourhood.name from location, street, neighbourhood, robbery, item WHERE item.fk_robbery = robbery.uid AND item.fk_itemtype = 2 AND robbery.fk_location = location.uid AND location.fk_street = street.uid AND street.fk_neighbourhood = neighbourhood.uid) as name");
            var MaxFietsRoof = FRDatabase.Select("SELECT MAX(name) FROM (SELECT neighbourhood.name from location, street, neighbourhood, robbery, item WHERE item.fk_robbery = robbery.uid AND item.fk_itemtype = 1 AND robbery.fk_location = location.uid AND location.fk_street = street.uid AND street.fk_neighbourhood = neighbourhood.uid) as name");
            var MinFietsRoof = FRDatabase.Select("SELECT MIN(name) FROM (SELECT neighbourhood.name from location, street, neighbourhood, robbery, item WHERE item.fk_robbery = robbery.uid AND item.fk_itemtype = 1 AND robbery.fk_location = location.uid AND location.fk_street = street.uid AND street.fk_neighbourhood = neighbourhood.uid) as name");

            foreach (List<string> entry in MaxRoof)
            {
                //Create geolocation(coordinates) for the Adress its given from database
                GeocodeAddress(entry[0], 0, 1);
            }

            foreach (List<string> entry in MaxFietsRoof)
            {

                GeocodeAddress(entry[0], 0, 1);
            }
            GeocodeAddress("Schiebroek", 1, 1);
            GeocodeAddress("Vogelenzang Rotterdam", 1, 1);

            

        }

        //Create Coordinates
        private String GeocodeAddress(string address, int amount, int type)
        {
            string results = "";
            //Bing Maps Key
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

            // If the response is not 0
            if (geocodeResponse.Results.Length > 0)
            {
                //Determines if you'll get a small or big Polygon
                if (type == 1)
                {
                    if (amount > 0)
                    {
                        //Creates Red polygon 
                        CreateBigRoundPoly(geocodeResponse.Results[0].Locations[0].Latitude, geocodeResponse.Results[0].Locations[0].Longitude, address, "FFFFFF", "008651");
                    }
                    else
                    {
                        //Creates green polygon
                        CreateBigRoundPoly(geocodeResponse.Results[0].Locations[0].Latitude, geocodeResponse.Results[0].Locations[0].Longitude, address, "FFFFFF", "ed1212");
                    }

                }
                else if (type == 2)
                {
                    CreateSmallRoundPoly(geocodeResponse.Results[0].Locations[0].Latitude, geocodeResponse.Results[0].Locations[0].Longitude, address, amount, "FFFFFF", "008651");
                }
            }
            else
                results = "No Results Found";

            return results;
        }

        public void CreateSmallRoundPoly(double latitude, double longitude, string name, int amount, string borderColor, string fillColor)
        {
            CustomPolygon = new MapPolygon();

            // Defines the polygon fill details
            CustomPolygon.Locations = new LocationCollection();
            CustomPolygon.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#" + fillColor));
            CustomPolygon.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#" + borderColor));
            CustomPolygon.StrokeThickness = 3;
            CustomPolygon.Opacity = 0.4;

            //Set focus back to the map so that +/- work for zoom in/out
            MapWithPolygon.Focus();

            //Poly Corners
            CustomPolygon.Locations = new LocationCollection()
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
            MapWithPolygon.Children.Add(CustomPolygon);

            //Call the custom template for the pushpin
            ControlTemplate template = (ControlTemplate)this.FindResource("CutomPushpinTemplate");

            // The pushpin to add to the map.
            Pushpin pin = new Pushpin();
            pin.Location = new Microsoft.Maps.MapControl.WPF.Location(latitude, longitude);
            pin.Template = template;
            pin.PositionOrigin = PositionOrigin.BottomLeft;

            //TEXT IN PUSHPIN
            pin.Content = name;

            //Event when clicked on pushpin
            //pin.MouseDown += new MouseButtonEventHandler(pin_MouseDown);

            // Adds the pushpin to the map.
            MapWithPolygon.Children.Add(pin);


        }
        public void CreateBigRoundPoly(double latitude, double longitude, string name, string borderColor, string fillColor)
        {
            CustomPolygon = new MapPolygon();

            // Defines the polygon fill details
            CustomPolygon.Locations = new LocationCollection();
            CustomPolygon.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#" + fillColor));
            CustomPolygon.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#" + borderColor));
            CustomPolygon.StrokeThickness = 3;
            CustomPolygon.Opacity = 0.4;

            //Set focus back to the map so that +/- work for zoom in/out
            MapWithPolygon.Focus();
            //Poly Corners
            CustomPolygon.Locations = new LocationCollection()
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
            MapWithPolygon.Children.Add(CustomPolygon);


            ControlTemplate template = (ControlTemplate)this.FindResource("CutomPushpinTemplate");



            // The pushpin to add to the map.
            Pushpin pin = new Pushpin();
            pin.Location = new Microsoft.Maps.MapControl.WPF.Location(latitude, longitude);
            pin.Template = template;
            pin.PositionOrigin = PositionOrigin.BottomLeft;

            //TEXT IN PUSHPIN
            pin.Content = name;

            //pin.MouseDown += new MouseButtonEventHandler(pin_MouseDown);

            // Adds the pushpin to the map.
            MapWithPolygon.Children.Add(pin);


        }
        private void pin_MouseDown(object sender, MouseButtonEventArgs e)
        {

            MessageBox.Show("User clicked OK");
        }

    }
}


