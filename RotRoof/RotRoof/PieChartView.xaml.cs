﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RotRoof
{
  /// <summary>
  /// Interaction logic for PieChartView.xaml
  /// </summary>
  public partial class PieChartView : UserControl
  {
    public PieChartView()
    {
      InitializeComponent();
      TextBlock Title = new TextBlock();
      Title.FontFamily = new FontFamily("Century Gothic");
      Title.FontSize = 15;
      Title.Text = "De meest gestolen fietsmerken";
      myTitle.Children.Add(Title);
    }
  }
}
