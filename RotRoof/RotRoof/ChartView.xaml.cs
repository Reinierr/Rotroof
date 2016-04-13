using System;
using System.Collections.Generic;
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
            this.createGrid10x10();

    }
    private void _placeSingleColorColumn(Grid grid, Color color, int height, int colNum, int maxHeight)
    {
      Brush brush = new SolidColorBrush(color);

      Rectangle rect = new Rectangle();
      rect.Fill = brush;
      rect.Width = 50;
      Grid.SetColumn(rect, colNum);
      Grid.SetRow(rect, maxHeight - height);
      Grid.SetRowSpan(rect, height);

      grid.Children.Add(rect);
    }

    private void _createLabelsvert(Grid grid, string[] labels)
    {
      RowDefinition rowDefnLabels = new RowDefinition();
      grid.RowDefinitions.Add(rowDefnLabels);

      for (int i = 0; i < labels.Length; i++)
      {
        TextBlock block = new TextBlock();
        block.Text = labels[i];
        block.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
        Grid.SetRow(block, i);
        Grid.SetColumn(block, grid.ColumnDefinitions.Count);
        grid.Children.Add(block);
      }
    }
    private void _createLabelshor(Grid grid, string[] labels)
    {
      RowDefinition rowDefnLabels = new RowDefinition();
      grid.RowDefinitions.Add(rowDefnLabels);

      for (int i = 0; i < labels.Length; i++)
      {
        TextBlock block = new TextBlock();
        block.Text = labels[i];
        block.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
        Grid.SetColumn(block, i);
        Grid.SetRow(block, grid.RowDefinitions.Count);
        grid.Children.Add(block);
      }
    }
    public void createGrid10x10()
    {
      Random random = new Random();

      // lengthe loop is array
      for (int i = 0; i < 10; i++)
      {
        string[] aLabels = "10,9,8,7,6,5,4,3,2,1".Split(',');
        _createLabelsvert(this.myGridLeft, aLabels);

        ColumnDefinition colDef = new ColumnDefinition();
        ColumnDefinition colDef2 = new ColumnDefinition();
        myGridMain.ColumnDefinitions.Add(colDef);
        myGridBottom.ColumnDefinitions.Add(colDef2);

        RowDefinition rowDef = new RowDefinition();
        myGridMain.RowDefinitions.Add(rowDef);


        Color color = i % 2 == 0 ? (Color)ColorConverter.ConvertFromString("#AEAEAE") : (Color)ColorConverter.ConvertFromString("#EAEAEA");

        _placeSingleColorColumn(this.myGridMain, color, random.Next(1, 11), i, 10);
      }
      string[] bLabels = "Jan, Feb, Mrt, Apr, Mei, Jun, Jul, Aug, Sep, Okt, Nov, Dec".Split(',');
      _createLabelshor(this.myGridBottom, bLabels);
    }
  }


}
