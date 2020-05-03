using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minicad2Core.Models
{
  public class CallDetail
  {
    public string inci_id { get; set; }
    public string userid { get; set; }
    public string descript { get; set; }
    public DateTime timestamp { get; set; }
    public string commentst { get; set; }
    public string usertyped { get; set; }
    public string unitcode { get; set; }


    public static List<CallDetail> GetActiveCallDetail()
    {
      string query = $@"
        SELECT
          L.inci_id
          ,userid
          ,descript
          ,timestamp
          ,comments
          ,usertyped
          ,unitcode
        FROM
          cad.dbo.log L
          INNER JOIN cad.dbo.incident I ON I.inci_id=L.inci_id AND I.inci_id != ''
        ";

      return new List<CallDetail>();
    }


  }
}
