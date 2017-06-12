using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FlightPlanner.Objects;

namespace FlightPlanner
{
  [Collection("flight_planner_test")]
  public class AirlineTest : IDisposable
  {
    public AirlineTest()
    {
      DBConfiguration.ConnectionString  = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=flight_planner_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Airline.DeleteAll();
      Flight.DeleteAll();
      City.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Airline.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfAirlinesAreTheSame()
    {
      Airline firstAirline = new Airline("Horizon", 120);
      Airline secondAirline = new Airline("Horizon", 120);
      Assert.Equal(firstAirline, secondAirline);
    }

    [Fact]
    public void Test_Save_ToAirlineDatabase()
    {
      Airline testAirline = new Airline("Horizon", 120);
      testAirline.Save();

      List<Airline> result = Airline.GetAll();
      List<Airline> testList = new List<Airline>{testAirline};
      Assert.Equal(testList, result);
    }

    [Fact]
     public void Test_Save_AssignsIdToObject()
     {
      Airline testAirline = new Airline("Horizon", 120);
       testAirline.Save();
       int testId = testAirline.GetId();
       int savedAirlineId = Airline.GetAll()[0].GetId();
       Assert.Equal(testId, savedAirlineId);
     }

     [Fact]
     public void Test_Find_FindsAirlineInDatabase()
     {
      Airline testAirline = new Airline("Horizon", 120);
      testAirline.Save();
      Airline foundAirline = Airline.Find(testAirline.GetId());
      Assert.Equal(testAirline, foundAirline);
     }

     [Fact]
     public void Test_If_FlightsAreSavedToAirline()
     {
       Airline testAirline = new Airline("Alaskan", 250);
       testAirline.Save();
       Flight testFlight = new Flight("5am", "6am", "On-Time", testAirline.GetId(), 1);
       //testFlight.Save();
       Flight testFlight2 = new Flight("7am", "8am", "On-Time", testAirline.GetId(), 1);
       //testFlight2.Save();

       testAirline.AddFlight(testFlight);
       testAirline.AddFlight(testFlight2);
       List<Flight> result = testAirline.GetFlights();
       List<Flight> testList = new List<Flight>{testFlight, testFlight2};
       Console.WriteLine("result id: {0}, {1}", result[0].GetId(), result[1].GetId());
       Console.WriteLine("testlist id: {0}, {1}", testList[0].GetId(), testList[1].GetId());

       Assert.Equal(testList, result);
     }
     [Fact]
     public void GetFlights_ReturnsAllAirlineFlights_FlightList()
     {

       Airline testAirline = new Airline("Alaskan", 250);
       testAirline.Save();
       Flight testFlight1 = new Flight("5am", "6am", "On-Time", testAirline.GetId(), 1);
       // testFlight1.Save();
       Flight testFlight2 = new Flight("7am", "8am", "On-Time", testAirline.GetId(), 1);
       // testFlight2.Save();

       testAirline.AddFlight(testFlight1);
       testAirline.AddFlight(testFlight2);
       List<Flight> savedFlights = testAirline.GetFlights();
       List<Flight> testList = new List<Flight> {testFlight1, testFlight2};

       Assert.Equal(testList, savedFlights);
     }
  }
}
