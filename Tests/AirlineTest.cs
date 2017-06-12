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
  }
}
