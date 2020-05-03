using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minicad2Core.Models
{
  public class Point
  {
    private const int BLOCK_SIZE = 100000;
    private const int GRIDSQUARE_SET_COL_SIZE = 8;
    private const int GRIDSQUARE_SET_ROW_SIZE = 20;
    private const double NORTHING_OFFSET = 10000000;
    private const string row1 = "ABCDEFGHJKLMNPQRSTUV";
    private const string row2 = "FGHJKLMNPQRSTUVABCDE";
    private const string col1 = "ABCDEFGH";
    private const string col2 = "JKLMNPQR";
    private const string col3 = "STUVWXYZ";
    private const string local_state_plane_wkt = @"PROJCS[""NAD_1983_HARN_StatePlane_Florida_East_FIPS_0901_Feet"", GEOGCS[""GCS_North_American_1983_HARN"", DATUM[""NAD83_High_Accuracy_Regional_Network"", SPHEROID[""GRS_1980"", 6378137.0, 298.257222101]], PRIMEM[""Greenwich"", 0.0], UNIT[""Degree"", 0.0174532925199433]], PROJECTION[""Transverse_Mercator""], PARAMETER[""False_Easting"", 656166.6666666665], PARAMETER[""False_Northing"", 0.0], PARAMETER[""Central_Meridian"", -81.0], PARAMETER[""Scale_Factor"", 0.9999411764705882], PARAMETER[""Latitude_Of_Origin"", 24.33333333333333], UNIT[""Foot_US"", 0.3048006096012192]]";
    public decimal? latitude { get; set; } = 0;
    public decimal? longitude { get; set; } = 0;
    public double state_plane_x { get; set; } = 0;
    public double state_plane_y { get; set; } = 0;
    public string UTM { get; set; } = "";
    public string USNG { get; set; } = "";
    public string location_type { get; set; } = "";
    public Int16? direction { get; set; } = 0;
    public Int16? speed { get; set; } = 0;
    public DateTime? timestamp { get; set; }    

    public Point() { }

    public Point(decimal latitude, decimal longitude, string location_type, Int16 direction, Int16 speed, DateTime timestamp)
    {
      this.latitude = latitude;
      this.longitude = longitude;
      this.location_type = location_type;
      this.direction = direction;
      this.speed = speed;
      this.timestamp = timestamp;
      state_plane_x = 0;
      state_plane_y = 0;
      Convert_To_State_Plane();
      Convert_To_USNG();
    }

    public Point(decimal latitude, decimal longitude)
    {
      this.latitude = latitude;
      this.longitude = longitude;
      state_plane_x = 0;
      state_plane_y = 0;
      Convert_To_State_Plane();
      Convert_To_USNG();
    }

    public Point(double state_plane_x, double state_plane_y)
    {
      this.state_plane_x = state_plane_x;
      this.state_plane_y = state_plane_y;
      latitude = 0;
      longitude = 0;
      Convert_To_Lat_Long();
      Convert_To_USNG();
    }

    private void Convert_To_State_Plane()
    {

      var x = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
      var projtarget = x.CreateFromWkt(Point.local_state_plane_wkt);
      try
      {
        var projsource = ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84;
        var t = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();
        var trans = t.CreateFromCoordinateSystems(projsource, projtarget);
        double[] point = { (double)longitude, (double)latitude };
        double[] convpoint = trans.MathTransform.Transform(point);
        state_plane_x = convpoint[0];
        state_plane_y = convpoint[1];
      }
      catch (Exception ex)
      {
        state_plane_x = 0;
        state_plane_y = 0;
        new ErrorLog(ex);
      }
    }

    private void Convert_To_Lat_Long()
    {
      var x = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
      var projsource = x.CreateFromWkt(Point.local_state_plane_wkt);
      try
      {
        var projtarget = ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84;
        var t = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();
        var trans = t.CreateFromCoordinateSystems(projsource, projtarget);
        double[] point = { state_plane_x, state_plane_y };
        double[] convpoint = trans.MathTransform.Transform(point);
        latitude = (decimal)convpoint[1];
        longitude = (decimal)convpoint[0];
      }
      catch (Exception ex)
      {
        latitude = 0;
        longitude = 0;
        new ErrorLog(ex);
      }
    }

    private void Convert_To_USNG()
    {
      // this function is going to convert our lat/long into both UTM and USNG
      // we do both here because we need to break up the UTM to convert it into USNG.
      if (latitude < -80 || latitude > 84) return;

      int zone = Get_Zone();
      string band = Get_Band();

      var utmFactory = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();
      var wgs84CS = ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84;
      var utmCS = ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WGS84_UTM(zone, latitude > 0);
      var utmTrans = utmFactory.CreateFromCoordinateSystems(wgs84CS, utmCS);
      double[] utmPoints = utmTrans.MathTransform.Transform(new double[] { (double)longitude, (double)latitude });
      double easting = utmPoints[0];
      double northing = utmPoints[1];
      var ZoneNumber = Get_Zonenumber();
      UTM = String.Format("{0}{1} {2:0} {3:0}", zone, band, easting, northing);
      if (latitude < 0) northing += NORTHING_OFFSET;
      string USNGLetters = Find_Grid_Letters(ZoneNumber, northing, easting);
      string USNGNorthing = Math.Round(northing).ToString();
      USNGNorthing = USNGNorthing.Substring(USNGNorthing.Length - 5).Substring(0, 4);
      string USNGEasting = Math.Round(easting).ToString();
      USNGEasting = USNGEasting.Substring(USNGEasting.Length - 5).Substring(0, 4);
      USNG = ZoneNumber.ToString() + Get_Band() + USNGLetters + " " + USNGEasting + " " + USNGNorthing;
    }

    private int Get_Zonenumber()
    {
      decimal longitude_temp = (longitude.Value + 180) - (int)((longitude.Value + 180) / 360) * 360 - 180;
      int zoneNumber = (int)((longitude_temp + 180) / 6) + 1;
      // handle west coast of norway
      if (latitude >= 56 && latitude < 64 && longitude_temp >= 3 && longitude_temp < 12) return 32;
      // handle Svalbard
      if(latitude >= 72 && latitude < 84)
      {
        if (longitude_temp >= 0 && longitude_temp < 9) return 31;
        if (longitude_temp >= 9 && longitude_temp < 21) return 33;
        if (longitude_temp >= 21 && longitude_temp < 33) return 35;
        if (longitude_temp >= 33 && longitude_temp < 42) return 37;
      }
      return zoneNumber;
    }

    private int Get_Zone()
    {
      // Norway handling
      if (latitude >= 56 && latitude < 64 && longitude >= 3 && longitude < 13) return 32;

      // Svalbard / Spitsbergen
      if (latitude >= 72 && latitude < 84)
      {
        if (longitude >= 0 && longitude < 9) return 31;
        if (longitude >= 9 && longitude < 21) return 33;
        if (longitude >= 21 && longitude < 33) return 35;
        if (longitude >= 33 && longitude < 42) return 37;
      }
      return (int)Math.Ceiling((longitude.Value + 180) / 6);
    }

    private string Get_Band()
    {
      if (latitude <= 84 && latitude >= 72) return "X";
      if (latitude < 72 && latitude >= 64) return "W";
      if (latitude < 64 && latitude >= 56) return "V";
      if (latitude < 56 && latitude >= 48) return "U";
      if (latitude < 48 && latitude >= 40) return "T";
      if (latitude < 40 && latitude >= 32) return "S";
      if (latitude < 32 && latitude >= 24) return "R";
      if (latitude < 24 && latitude >= 16) return "Q";
      if (latitude < 16 && latitude >= 8) return "P";
      if (latitude < 8 && latitude >= 0) return "N";
      if (latitude < 0 && latitude >= -8) return "M";
      if (latitude < -8 && latitude >= -16) return "L";
      if (latitude < -16 && latitude >= -24) return "K";
      if (latitude < -24 && latitude >= -32) return "J";
      if (latitude < -32 && latitude >= -40) return "H";
      if (latitude < -40 && latitude >= -48) return "G";
      if (latitude < -48 && latitude >= -56) return "F";
      if (latitude < -56 && latitude >= -64) return "E";
      if (latitude < -64 && latitude >= -72) return "D";
      if (latitude < -72 && latitude >= -80) return "C";
      return "Z";
    }

    private string Find_Grid_Letters(int ZoneNumber, double Northing, double Easting)
    {
      int row = 1;
      double north_1m = Math.Round(Northing);
      do
      {
        north_1m -= BLOCK_SIZE;
        row++;
      }
      while (north_1m > BLOCK_SIZE);
      row %= GRIDSQUARE_SET_ROW_SIZE;

      int col = 0;
      double east_1m = Math.Round(Easting);
      do
      {
        east_1m -= BLOCK_SIZE;
        col++;
      }
      while (east_1m > BLOCK_SIZE);
      col %= GRIDSQUARE_SET_COL_SIZE;

      return Letters_Helper(Find_Set(ZoneNumber), row, col);
    }

    private string Letters_Helper(int letterset, int row, int col)
    {
      row = Math.Max(row, GRIDSQUARE_SET_ROW_SIZE) - 1;
      col = Math.Max(col, GRIDSQUARE_SET_COL_SIZE) - 1;
      switch (letterset)
      {
        case 1:
          var test1 = Get_Row_Col(row1, col1, row, col);
          var actual1 = col1[col].ToString() + row1[row].ToString();
          return col1[col].ToString() + row1[row].ToString();
        case 2:
          var test2 = Get_Row_Col(row2, col2, row, col);
          var actual2 = col2[col].ToString() + row2[row].ToString();
          return col2[col].ToString() + row2[row].ToString();
        case 3:
          var test3 = Get_Row_Col(row1, col3, row, col);
          var actual3 = col3[col].ToString() + row1[row].ToString();
          return col3[col].ToString() + row1[row].ToString();
        case 4:
          var test4 = Get_Row_Col(row2, col1, row, col);
          var actual4 = col1[col].ToString() + row2[row].ToString();
          return col1[col].ToString() + row2[row].ToString();
        case 5:
          var test5 = Get_Row_Col(row1, col2, row, col);
          var actual5 = col2[col].ToString() + row1[row].ToString();
          return col2[col].ToString() + row1[row].ToString();
        case 6:
          var test6 = Get_Row_Col(row2, col3, row, col);
          var actual6 = col3[col].ToString() + row2[row].ToString();
          return col3[col].ToString() + row2[row].ToString();
        default: 
          return "";
      }
     
    }

    private int Find_Set(int ZoneNumber)
    {
      int tmp = ZoneNumber % 6;
      if (tmp == 0) return 6;
      if (tmp > 0 && tmp < 6) return tmp;
      return -1;
    }

    private string Get_Row_Col(string row_values, string col_values, int row, int col)
    {
      return col_values.Substring(col, 1) + row_values.Substring(row, 1);
    }

  }
}
