using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FlightPlanner.Objects
{
  public class Airline
  {
    private string _name;
    private int _fare;
    private int _id;

    public Airline(string Name, int Fare, int Id = 0)
    {
      _name = Name;
      _fare = Fare;
      _id = Id;
    }

    public int GetId()
    {
      return _id;
    }
    public int GetFare()
    {
      return _fare;
    }
    public string GetName()
    {
      return _name;
    }

    public override bool Equals(System.Object otherAirline)
    {
      if (!(otherAirline is Airline))
      {
        return false;
      }
      else
      {
        Airline newAirline = (Airline) otherAirline;
        bool idEquality = (this.GetId() == newAirline.GetId());
        bool nameEquality = (this.GetName() == newAirline.GetName());
        bool fareEquality = (this.GetFare() == newAirline.GetFare());
        return (idEquality && nameEquality && fareEquality );
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM airlines;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<Airline> GetAll()
    {
      List<Airline> allAirlines = new List<Airline>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM airlines;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int airlineId = rdr.GetInt32(0);
        string airlineName = rdr.GetString(1);
        int airlineFare = rdr.GetInt32(2);
        Airline newAirline = new Airline(airlineName, airlineFare, airlineId);
        allAirlines.Add(newAirline);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allAirlines;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO airlines (name, fare) OUTPUT INSERTED.id VALUES (@AirlineName, @AirlineFare)", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@AirlineName";
      nameParameter.Value = this.GetName();

      SqlParameter fareParameter = new SqlParameter();
      fareParameter.ParameterName = "@AirlineFare";
      fareParameter.Value = this.GetFare();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(fareParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static Airline Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM airlines WHERE id = @AirlineId", conn);
      SqlParameter airlineIdParameter = new SqlParameter();
      airlineIdParameter.ParameterName = "@AirlineId";
      airlineIdParameter.Value = id.ToString();
      cmd.Parameters.Add(airlineIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundAirlineId = 0;
      string foundAirlineName = null;
      int foundAirlineFare = 0;

      while(rdr.Read())
      {
        foundAirlineId = rdr.GetInt32(0);
        foundAirlineName = rdr.GetString(1);
        foundAirlineFare = rdr.GetInt32(2);
      }
      Airline foundAirline = new Airline(foundAirlineName, foundAirlineFare, foundAirlineId);

      if (rdr != null)
     {
       rdr.Close();
     }
     if (conn != null)
     {
       conn.Close();
     }

     return foundAirline;
    }

  }
}
