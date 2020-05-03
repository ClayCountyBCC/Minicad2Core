using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minicad2Core.Models
{
  public class Advisory
  {


    public List<Advisory> GetAdvisories()
    {
      string query = @"
        SELECT
          ISNULL(expires
                 ,'12/31/9999') AS expires
          ,addtime
          ,ISNULL(title
                  ,'') AS title
          ,ISNULL(location
                  ,'') AS location
          ,ISNULL(notes
                  ,'') AS notes
        FROM
          cad.dbo.advisory
        WHERE
          status = 'ACTIVE'
          AND ( expires > GETDATE()
                 OR expires IS NULL )
        ORDER  BY
          title ASC
        ";

      return new List<Advisory>();
    }
  }
}
