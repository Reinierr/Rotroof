using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotRoof
{
  class Fiets 
  {
    public string Brand { get; set; }
    public int Amount { get; set; }
  }
  class PieCollection : System.Collections.ObjectModel.Collection<Fiets>
  {
    public PieCollection()
    {
      DBConnection test = new DBConnection();
      List<List<string>> information = test.Select("SELECT Brand, Count(Brand) as TotMerk FROM item WHERE Brand IS NOT NULL AND Brand <> 'Onbekend' AND Name = 'Fiets' GROUP BY Brand ORDER BY TotMerk DESC LIMIT 10");
      foreach(List<string> entry in information) 
      {
        Add(new Fiets { Brand = entry[0], Amount = Convert.ToInt32(entry[1]) });
      }
    }
  }
}
