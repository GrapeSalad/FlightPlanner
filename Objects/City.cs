using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FlightPlanner.Objects
{
  public class City
  {
    private string _name;
    private int _id;

    public City(string Name, int Id = 0)
    {
      _name = Name;
      _id = Id;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public override bool Equals(System.Object otherCity)
    {
      if (!(otherCity is City))
      {
        return false;
      }
      else
      {
        City newCity = (City) otherCity;
        bool idEquality = (this.GetId() == newCity.GetId());
        bool nameEquality = (this.GetName() == newCity.GetName());
        return (idEquality && nameEquality);
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM cities;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<City> GetAll()
    {
      List<City> allCities = new List<City>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cities;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string cityName = rdr.GetString(1);
        City newCity = new City(cityName, cityId);
        allCities.Add(newCity);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allCities;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO cities (name) OUTPUT INSERTED.id VALUES (@CityName)", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@CityName";
      nameParameter.Value = this.GetName();

      cmd.Parameters.Add(nameParameter);

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

    public static City Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cities WHERE id = @CityId", conn);
      SqlParameter cityIdParameter = new SqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = id.ToString();
      cmd.Parameters.Add(cityIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundCityId = 0;
      string foundCityName = null;

      while(rdr.Read())
      {
        foundCityId = rdr.GetInt32(0);
        foundCityName = rdr.GetString(1);
      }
      City foundCity = new City(foundCityName, foundCityId);

      if (rdr != null)
     {
       rdr.Close();
     }
     if (conn != null)
     {
       conn.Close();
     }

     return foundCity;
    }
    public void AddFlight(Flight newFlight)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO flights (arrival, departure, status, airline_id, city_id)  OUTPUT INSERTED.id VALUES (@FlightArrival, @FlightDeparture, @FlightStatus, @AirlineId, @CityId);", conn);
      SqlParameter cityIdParameter = new SqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = this.GetId();
      cmd.Parameters.Add(cityIdParameter);

      SqlParameter flightArrivalParameter = new SqlParameter();
      flightArrivalParameter.ParameterName = "@FlightArrival";
      flightArrivalParameter.Value = newFlight.GetArrival();
      cmd.Parameters.Add(flightArrivalParameter);

      SqlParameter FlightDepartureParameter = new SqlParameter();
      FlightDepartureParameter.ParameterName = "@FlightDeparture";
      FlightDepartureParameter.Value = newFlight.GetDeparture();
      cmd.Parameters.Add(FlightDepartureParameter);

      SqlParameter flightStatusParameter = new SqlParameter();
      flightStatusParameter.ParameterName = "@FlightStatus";
      flightStatusParameter.Value = newFlight.GetStatus();
      cmd.Parameters.Add(flightStatusParameter);

      SqlParameter airlineIdParameter = new SqlParameter();
      airlineIdParameter.ParameterName = "@AirlineId";
      airlineIdParameter.Value = newFlight.GetAirlineId();
      cmd.Parameters.Add(airlineIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        newFlight.SetId(rdr.GetInt32(0));
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    public List<Flight> GetFlights()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT id FROM flights WHERE city_id = @CityId;", conn);
      SqlParameter cityIdParameter = new SqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = this.GetId();
      cmd.Parameters.Add(cityIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<int> flightIds = new List<int> {};
      while(rdr.Read())
      {
        int flightId = rdr.GetInt32(0);
        flightIds.Add(flightId);
      }
      if (rdr != null)
      {
        rdr.Close();
      }

      List<Flight> flights = new List<Flight> {};
      foreach (int flightId in flightIds)
      {
        SqlCommand flightQuery = new SqlCommand("SELECT * FROM flights WHERE id = @FlightId;", conn);

        SqlParameter flightIdParameter = new SqlParameter();
        flightIdParameter.ParameterName = "@FlightId";
        flightIdParameter.Value = flightId;
        flightQuery.Parameters.Add(flightIdParameter);

        SqlDataReader queryReader = flightQuery.ExecuteReader();
        while(queryReader.Read())
        {
              int thisFlightId = queryReader.GetInt32(0);
              string thisFlightArrival = queryReader.GetString(1);
              string thisFlightDeparture = queryReader.GetString(2);
              string thisFlightStatus = queryReader.GetString(3);
              int thisFlightAirlineId = queryReader.GetInt32(4);
              int thisFlightCityId = queryReader.GetInt32(5);
              Flight foundFlight = new Flight(thisFlightArrival, thisFlightDeparture, thisFlightStatus, thisFlightAirlineId, thisFlightCityId, thisFlightId);
              flights.Add(foundFlight);
        }
        if (queryReader != null)
        {
          queryReader.Close();
        }
      }
      if (conn != null)
      {
        conn.Close();
      }
      return flights;
    }

    // public List<Flight> GetFlightsByAirlineToCity()
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("SELECT flights.* FROM airlines JOIN flights ON (airline.id = flights.airline_id) JOIN cities ON (flights.city_id = city.id) WHERE cities.id = @CityId;", conn);
    //   SqlParameter CityIdParam = new SqlParameter();
    //   CityIdParam.ParameterName = "@CityId";
    //   CityIdParam.Value = this.GetId().ToString();
    //
    //   cmd.Parameters.Add(CityIdParam);
    //
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //
    //   List<Flight> flights = new List<Flight>{};
    //
    //   while(rdr.Read())
    //   {
    //     int flightId = rdr.GetInt32(0);
    //     string flightArrival = rdr.GetString(1);
    //     string flightDeparture = rdr.GetString(2);
    //     string flightStatus = rdr.GetString(3);
    //     int flightAirlineId = rdr.GetInt32(4);
    //     int flightCityId = rdr.GetInt32(5);
    //     Flight foundFlight = new Flight(flightArrival, flightDeparture, flightStatus, flightAirlineId, flightCityId, flightId);
    //     flights.Add(foundFlight);
    //   }
    //
    //   if (rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    //   return flights;
    // }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM cities WHERE id = @CityId; DELETE FROM flights WHERE city_id = @CityId;", conn);
      SqlParameter cityIdParameter = new SqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = this.GetId();

      cmd.Parameters.Add(cityIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
