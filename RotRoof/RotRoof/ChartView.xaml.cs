using System;
using System.Collections.Generic;
using System.Data;
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
            var information = test.Select("SELECT MONTH(datetime) AS MaandNaam, Count(*) AS TotalMaand FROM robbery WHERE datetime IS NOT NULL and YEAR(datetime) = '2011'  group by MONTH(datetime)", properties);
            this.createGraph(information);
        }
    private void _canvasPlaceSingleColor(Canvas canvas, Color color, int height, int i, int col, string colName, int max, int min)
    {
      //column
      Rectangle rect = new Rectangle();
      Brush paint = new SolidColorBrush(color);
      rect.Fill = paint;
      rect.Width = ((canvas.Width / (col + 1)) - 2);
      rect.Height = height;
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
      Canvas.SetBottom(txt2, (height + 20));

      myCanvas.Children.Add(rect);
      myCanvas.Children.Add(txt);
      if (rect.Height == max || rect.Height == min)
      {
        myCanvas.Children.Add(txt2);
      }
    }

    public void createGraph(List<Dictionary<string, string>> information)
    {
      int i = 0;
      int min = 999999;
      int max = 0;
      // lengthe loop is array
      /*foreach (KeyValuePair<string, string> entry in information)
      {
        if (sr > max)
        {
          max = sr.Total;
        }
        if (sr.Total < min)
        {
          min = sr.Total;
        }
      }*/

      foreach (Dictionary<string, string> entry in information)
      {
        Color color = i % 2 == 0 ? (Color)ColorConverter.ConvertFromString("#AEAEAE") : (Color)ColorConverter.ConvertFromString("#EAEAEA");
        _canvasPlaceSingleColor(myCanvas, color, Convert.ToInt32(entry["TotalMaand"]), i, information.Count, entry["MaandNaam"], max, min);
        i++;
      }
      
    }
  }


}
