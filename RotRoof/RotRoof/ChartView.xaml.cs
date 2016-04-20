using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace RotRoof
{
    /// <summary>
    /// Interaction logic for ChartView.xaml
    /// </summary>
    public partial class ChartView : UserControl
    {
        public ChartView()
        {
            InitializeComponent();
            DBConnection test = new DBConnection();
            List<string> properties = new List<string>();
            properties.Add("MaandNaam");
            properties.Add("TotalMaand");
            var information = test.Select("SELECT date_format(datetime, '%b') AS MaandNaam, Count(*) AS TotalMaand FROM robbery WHERE datetime IS NOT NULL and YEAR(datetime) = '2012'  group by MONTH(datetime)");

            this.createGraph(information);
        }
    private void _canvasPlaceSingleColor(Canvas canvas, Color color, int height, int i, int col, string colName, int max, int min)
    {
      //column
      Rectangle rect = new Rectangle();
      Brush paint = new SolidColorBrush(color);
      double exp = canvas.Height / max;
      rect.Fill = paint;
      rect.Width = ((canvas.Width / (col + 1)) - 2);
      rect.Height = height * exp;
      Canvas.SetLeft(rect, (i + 1) * (rect.Width + 2));
      Canvas.SetBottom(rect, 20);

      // columnName
      TextBlock txt = new TextBlock();
      txt.Text = colName;
      txt.HorizontalAlignment = HorizontalAlignment.Center;
      Canvas.SetLeft(txt, (i + 1) * (rect.Width + 2));
      Canvas.SetBottom(txt, 0);

      // columnInt
      TextBlock txt2 = new TextBlock();
      txt2.Text = Convert.ToString(height);
      Canvas.SetLeft(txt2, 0);
      Canvas.SetBottom(txt2, ((height + 20)*exp));

      myCanvas.Children.Add(rect);
      myCanvas.Children.Add(txt);
      if (rect.Height == max * exp || rect.Height == min * exp)
      {
        myCanvas.Children.Add(txt2);
      }
    }

    public void createGraph(List<List<string>> information)
    {
      int i = 0;
      int min = 999999;
      int max = 0;
      // lengthe loop is array
      TextBlock Title = new TextBlock();
      Title.FontFamily = new FontFamily("Century Gothic");
      Title.FontSize = 15;
      Title.Text = "Criminaliteit per maand in het jaar 2012";
      myTitle.Children.Add(Title);

       foreach (List<string> entry in information)
       {
         if (Convert.ToInt32(entry[1]) > max)
         {
           max = Convert.ToInt32(entry[1]);
         }
         if (Convert.ToInt32(entry[1]) < min)
         {
           min = Convert.ToInt32(entry[1]);
         }
       }

      foreach (List<string> entry in information)
      {
        Color color = i % 2 == 0 ? (Color)ColorConverter.ConvertFromString("#007555") : (Color)ColorConverter.ConvertFromString("#008651");
        _canvasPlaceSingleColor(myCanvas, color, Convert.ToInt32(entry[1]), i, information.Count, entry[0], max, min);
        i++;
      }
      
    }
  }


}
