using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minicad2Core.Models
{
  public class ConnectionConfig
  {
    public string CAD { get; set; }
    public string Tracking { get; set; }
    public string GIS { get; set; }
    public string LogProduction { get; set; }
    public string LogBackup { get; set; }
  }
}
