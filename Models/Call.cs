using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Minicad2Core.Models
{
  public class Call
  {
    public string inci_id { get; set; }
    public string district { get; set; }
    public string statbeat { get; set; }
    public string street { get; set; }
    public string cross_street { get; set; }
    public double x { get; set; }
    public double y { get; set; }
    public bool is_active { get; set; }
    public Point point { get; set; } = null;


    public static List<Call> GetActiveCalls(string cs)
    {
      string query = $@"
        SELECT
          inci_id
          ,district
          ,statbeat
          ,street
          ,crossroad1 cross_street
          , geox x
          , geoy y
        FROM
          cad.dbo.incident
        WHERE
          inci_id != ''
        ";

      try
      {
        using (IDbConnection db = new SqlConnection(cs))
        {
          var results = (List<Call>)db.Query<Call>(query);

          foreach (Call c in results)
          {
            c.point = new Point(c.x, c.y);
          }
          return results;
        }

      }
      catch (Exception ex)
      {
        new ErrorLog(ex);
        return null;
      }     
    }

  }
}
