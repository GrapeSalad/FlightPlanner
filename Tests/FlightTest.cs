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
  }
}
