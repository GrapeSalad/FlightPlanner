using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FlightPlanner.Objects;

namespace FlightPlanner
{
  [Collection("flight_planner_test")]
  public class CityTest : IDisposable
  {
    public CityTest()
    {
      DBConfiguration.ConnectionString  = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=flight_planner_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      City.DeleteAll();
      Airline.DeleteAll();
      Flight.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = City.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfCitysAreTheSame()
    {
      City firstCity = new City("Portland");
      City secondCity = new City("Portland");
      Assert.Equal(firstCity, secondCity);
    }

    [Fact]
    public void Test_Save_ToCityDatabase()
    {
      City testCity = new City("Portland");
      testCity.Save();

      List<City> result = City.GetAll();
      List<City> testList = new List<City>{testCity};
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      City testCity = new City("Portland");
      testCity.Save();
      int testId = testCity.GetId();
      int savedCityId = City.GetAll()[0].GetId();
      Assert.Equal(testId, savedCityId);
    }

    [Fact]
    public void Test_Find_FindsCityInDatabase()
    {
      City testCity = new City("Portland");
      testCity.Save();
      City foundCity = City.Find(testCity.GetId());
      Assert.Equal(testCity, foundCity);
    }
    [Fact]
    public void Test_If_FlightsAreSavedToCity()
    {
      City testCity = new City("PDX");
      testCity.Save();
      Flight testFlight = new Flight("5am", "6am", "On-Time", 1, testCity.GetId());
      //testFlight.Save();
      Flight testFlight2 = new Flight("7am", "8am", "On-Time", 1, testCity.GetId());
      //testFlight2.Save();

      testCity.AddFlight(testFlight);
      testCity.AddFlight(testFlight2);
      List<Flight> result = testCity.GetFlights();
      List<Flight> testList = new List<Flight>{testFlight, testFlight2};
      // Console.WriteLine("result id: {0}, {1}", result[0].GetId(), result[1].GetId());
      // Console.WriteLine("testlist id: {0}, {1}", testList[0].GetId(), testList[1].GetId());

      Assert.Equal(testList, result);
    }
    [Fact]
    public void GetFlights_ReturnsAllCityFlights_FlightList()
    {

      City testCity = new City("PDX");
      testCity.Save();
      Flight testFlight1 = new Flight("5am", "6am", "On-Time", 1, testCity.GetId());
      // testFlight1.Save();
      Flight testFlight2 = new Flight("7am", "8am", "On-Time", 1, testCity.GetId());
      // testFlight2.Save();

      testCity.AddFlight(testFlight1);
      testCity.AddFlight(testFlight2);
      List<Flight> savedFlights = testCity.GetFlights();
      List<Flight> testList = new List<Flight> {testFlight1, testFlight2};

      Assert.Equal(testList, savedFlights);
    }

    // [Fact]
    // public void Delete_DeletesCityAssociationsFromDatabase_CityList()
    // {
    //   string testName = "PDX";
    //   City testCity = new City(testName);
    //   testCity.Save();
    //
    //   Flight testFlight = new Flight("5am", "6am", "On-Time", 1, testCity.GetId());
    //   testFlight.Save();
    //
    //   testCity.AddFlight(testFlight);
    //   testCity.Delete();
    //
    //   List<City> resultFlightCity = testFlight.GetCity();
    //   List<City> testFlightCity = new List<City> {};
    //
    //   Assert.Equal(testFlightCity, resultFlightCity);
    // }
  }
}
