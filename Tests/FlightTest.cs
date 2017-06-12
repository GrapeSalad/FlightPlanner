using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FlightPlanner.Objects;

namespace FlightPlanner
{
  [Collection("flight_planner_test")]
  public class FlightTest : IDisposable
  {
    public FlightTest()
    {
      DBConfiguration.ConnectionString  = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=flight_planner_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Flight.DeleteAll();
      City.DeleteAll();
      Airline.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Flight.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfFlightsAreTheSame()
    {
      Flight firstFlight = new Flight("5pm", "6pm", "Ankle pants", 1, 1);
      Flight secondFlight = new Flight("5pm", "6pm", "Ankle pants", 1, 1);
      Assert.Equal(firstFlight, secondFlight);
    }

    [Fact]
    public void Test_Save_ToFlightDatabase()
    {
      Flight testFlight = new Flight("12am", "4am", "Cancelled", 1, 1);
      testFlight.Save();

      List<Flight> result = Flight.GetAll();
      List<Flight> testList = new List<Flight>{testFlight};
      Assert.Equal(testList, result);
    }

    [Fact]
     public void Test_Save_AssignsIdToObject()
     {
      Flight testFlight = new Flight("4pm", "5pm", "Cancelled", 1, 1);
       testFlight.Save();
       int testId = testFlight.GetId();
       int savedFlightId =Flight.GetAll()[0].GetId();
       Assert.Equal(testId, savedFlightId);
     }

     [Fact]
     public void Test_Find_FindsFlightInDatabase()
     {
      Flight testFlight = new Flight("1am", "10am", "Delayed",1 ,1);
      testFlight.Save();
      Flight foundFlight =Flight.Find(testFlight.GetId());
      Assert.Equal(testFlight, foundFlight);
     }

     [Fact]
      public void Delete_DeletesFlightAssociationsFromDatabase_FlightList()
      {

        City testCity = new City("PDX");
        testCity.Save();

        Flight testFlight = new Flight("7pm", "9pm", "On-Time", 1, testCity.GetId());
        testFlight.Save();

        // testFlight.AddCity(testCity);
        testFlight.Delete();

        List<Flight> resultCityFlights = testCity.GetFlights();
        List<Flight> testCityFlights = new List<Flight> {};


        Assert.Equal(testCityFlights, resultCityFlights);
      }
  }
}
