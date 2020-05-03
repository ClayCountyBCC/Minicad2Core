using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Text.RegularExpressions;


namespace Minicad2Core.Models
{
  public class Note
  {

    public int note_id { get; set; } = 0;
    public int log_id { get; set; } = 0;
    public DateTime timestamp { get; set; }
    public string inci_id { get; set; } = "";
    public string raw_note { get; set; } = "";
    public string note
    {
      get
      {
        return Regex.Replace(raw_note, @"\s+\[\d\d/\d\d/\d\d\s\d\d:\d\d:\d\d\s\w+]|\[\w+\-\w+\] {\w+}\s+", "");
      }
    }

    public string raw_unitcode { get; set; } = "";
    public string unitcode
    {
      get
      {
        return "";
      }
    }



    public static List<Note> GetActiveCallNotes(string cs)
    {
      string query = @"
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        SELECT
          N.id note_id  
          ,0 log_id
          ,N.datetime timestamp
          ,N.eventid inci_id
          ,N.notes note  
          ,'' unitcode
        FROM cad.dbo.incinotes N
        INNER JOIN cad.dbo.incident I ON N.eventid = I.inci_id AND I.inci_id != ''

        UNION ALL

        SELECT
          0 note_id
          ,L.logid log_id
          ,L.timestamp
          ,L.inci_id
          ,L.comments note
          ,L.unitcode
        FROM cad.dbo.log L
        INNER JOIN cad.dbo.incident I ON L.inci_id = I.inci_id AND I.inci_id != ''
        WHERE 
          transtype='M'  
        ORDER BY timestamp DESC";

      try
      {
        using (IDbConnection db = new SqlConnection(cs))
        {
          return (List<Note>)db.Query<Note>(query);
        }
      }
      catch (Exception ex)
      {
        new ErrorLog(ex, query);
        return null;
      }

    }
  }
}
