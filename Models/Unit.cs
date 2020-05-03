using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Minicad2Core.Models
{
  public class Unit
  {
    public string unitcode { get; set; }
    public string home_station { get; set; } = "";
    public string unit_status { get; set; } = "";
    public string cad_location { get; set; } = "";
    public string avcomments { get; set; } = "";
    public string inci_id { get; set; } = "";
    public string primary_officer { get; set; } = "";
    public string current_station { get; set; } = "";
    public string unit_type { get; set; } = "";
    public string data_source { get; set; } = "";
    public Point current_point { get; set; }

    public Unit()
    {
    }

    public static List<Unit> GetAllCurrentUnits(string cs)
    {
      string query = @"
        SELECT
          [unitcode]
          ,[home_station]
          ,[unit_status]
          ,[cad_location]
          ,[avcomments]
          ,[inci_id]
          ,[primary_officer]
          ,[current_station]
          ,[unit_type]
          ,ISNULL([data_source], '') data_source
          ,[longitude]
          ,[latitude]
          ,[date_last_communicated] timestamp
          ,[direction]
          ,[speed]
        FROM
          [Tracking].[dbo].[MinicadUnits]";
      try
      {
        using (IDbConnection db = new SqlConnection(cs))
        {
          var units = db.Query<Unit, Point, Unit>(query, 
            (u, p) => 
            {
              if (p != null && p.latitude.HasValue)
              {
                u.current_point = new Point(p.latitude.Value, p.longitude.Value, p.location_type, p.direction.Value, p.speed.Value, p.timestamp.Value);
              }
              else
              {
                u.current_point = null;
              }
              return u; 
            }, 
            splitOn:"longitude").ToList();
          return units;
        }
      }
      catch (Exception ex)
      {
        new ErrorLog(ex, query);
        return null;
      }

    }

    //public static List<Unit> GetUpdatedUnits(DateTime dateUpdated, string cs)
    //{

    //}

  }
}
