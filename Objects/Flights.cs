using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FlightPlanner.Objects
{
  public class Flight
  {
    private string _arrival;
    private string _departure;
    private string _status;
    private int _airlineId;
    private int _cityId;
    private int _id;

    public Flight(string Arrival, string Departure, string Status, int AirlineId, int CityId, int Id = 0)
    {
      _arrival = Arrival;
      _departure = Departure;
      _status = Status;
      _airlineId = AirlineId;
      _cityId = CityId;
      _id = Id;
    }

    public int GetId()
    {
      return _id;
    }
    public int GetAirlineId()
    {
      return _airlineId;
    }
    public int GetCityId()
    {
      return _cityId;
    }
    public string GetArrival()
    {
      return _arrival;
    }
    public string GetDeparture()
    {
      return _departure;
    }
    public string GetStatus()
    {
      return _status;
    }

    public void SetId(int id)
    {
      _id = id;
    }

    public override bool Equals(System.Object otherFlight)
    {
      if (!(otherFlight is Flight))
      {
        return false;
      }
      else
      {
        Flight newFlight = (Flight) otherFlight;
        bool idEquality = (this.GetId() == newFlight.GetId());
        bool arrivalEquality = (this.GetArrival() == newFlight.GetArrival());
        bool departureEquality = (this.GetDeparture() == newFlight.GetDeparture());
        bool statusEquality = (this.GetStatus() == newFlight.GetStatus());
        bool airlineIdEquality = (this.GetAirlineId() == newFlight.GetAirlineId());
        bool cityIdEquality = (this.GetCityId() == newFlight.GetCityId());
        return (idEquality && arrivalEquality && departureEquality && statusEquality && airlineIdEquality && cityIdEquality);
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM flights;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<Flight> GetAll()
    {
      List<Flight> allFlights = new List<Flight>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM flights;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int flightId = rdr.GetInt32(0);
        string flightArrival = rdr.GetString(1);
        string flightDeparture = rdr.GetString(2);
        string flightStatus = rdr.GetString(3);
        int flightAirlineId = rdr.GetInt32(4);
        int flightCityId = rdr.GetInt32(5);
        Flight newFlight = new Flight(flightArrival, flightDeparture, flightStatus, flightAirlineId, flightCityId, flightId);
        allFlights.Add(newFlight);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allFlights;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO flights (arrival, departure, status, airline_id, city_id) OUTPUT INSERTED.id VALUES (@FlightArrival, @FlightDeparture, @FlightStatus, @FlightAirlineId, @FlightCityId)", conn);

      SqlParameter arrivalParameter = new SqlParameter();
      arrivalParameter.ParameterName = "@FlightArrival";
      arrivalParameter.Value = this.GetArrival();

      SqlParameter departureParameter = new SqlParameter();
      departureParameter.ParameterName = "@FlightDeparture";
      departureParameter.Value = this.GetDeparture();

      SqlParameter statusParameter = new SqlParameter();
      statusParameter.ParameterName = "@FlightStatus";
      statusParameter.Value = this.GetStatus();

      SqlParameter airlineIdParameter = new SqlParameter();
      airlineIdParameter.ParameterName = "@FlightAirlineId";
      airlineIdParameter.Value = this.GetAirlineId();

      SqlParameter cityIdParameter = new SqlParameter();
      cityIdParameter.ParameterName = "@FlightCityId";
      cityIdParameter.Value = this.GetCityId();

      cmd.Parameters.Add(arrivalParameter);
      cmd.Parameters.Add(departureParameter);
      cmd.Parameters.Add(statusParameter);
      cmd.Parameters.Add(airlineIdParameter);
      cmd.Parameters.Add(cityIdParameter);
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

    public static Flight Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM flights WHERE id = @FlightId", conn);
      SqlParameter flightIdParameter = new SqlParameter();
      flightIdParameter.ParameterName = "@FlightId";
      flightIdParameter.Value = id.ToString();
      cmd.Parameters.Add(flightIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundFlightId = 0;
      string foundFlightArrival = null;
      string foundFlightDeparture = null;
      string foundFlightStatus = null;
      int foundFlightAirlineId = 0;
      int foundFlightCityId = 0;
      while(rdr.Read())
      {
        foundFlightId = rdr.GetInt32(0);
        foundFlightArrival = rdr.GetString(1);
        foundFlightDeparture = rdr.GetString(2);
        foundFlightStatus = rdr.GetString(3);
        foundFlightAirlineId = rdr.GetInt32(4);
        foundFlightCityId = rdr.GetInt32(5);
      }
      Flight foundFlight = new Flight(foundFlightArrival, foundFlightDeparture, foundFlightStatus, foundFlightAirlineId, foundFlightCityId, foundFlightId);

      if (rdr != null)
     {
       rdr.Close();
     }
     if (conn != null)
     {
       conn.Close();
     }

     return foundFlight;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM flights WHERE id = @FlightId;", conn);
      SqlParameter flightIdParameter = new SqlParameter();
      flightIdParameter.ParameterName = "@FlightId";
      flightIdParameter.Value = this.GetId();

      cmd.Parameters.Add(flightIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
