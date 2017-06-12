using System;
using System.Collections.Generic;
using FlightPlanner.Objects;
using System.Data;
using System.Data.SqlClient;
using Nancy;

namespace FlightPlanner
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      Get["/flights"] = _ => {
        List<Flight> allFlights = Flight.GetAll();
        return View["flights.cshtml", allFlights];
      };
      Get["/airlines/add"] = _ => {
        return View["airline_form.cshtml"];
      };
      Get["/cities/add"] = _ => {
        return View["city_form.cshtml"];
      };
      Get["/flights/add"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<City> newCity = City.GetAll();
        List<Airline> newAirline = Airline.GetAll();
        model.Add("city", newCity);
        model.Add("airline", newAirline);
        return View["flight_form.cshtml", model];
      };

      // Get["/test"] = _ => {
      //   List<City> testCity = City.GetFlightsByAirlineToCity();
      //   return View["test.cshtml", testCity];
      // };

      Post["/airlines/add"] = _ => {
        Airline newAirline = new Airline(Request.Form["airlineName"], Request.Form["fare"]);
        newAirline.Save();
        return View["success.cshtml"];
      };
      Post["/cities/add"] = _ => {
        City newCity = new City(Request.Form["cityName"]);
        newCity.Save();
        return View["success.cshtml"];
      };
      Post["/flights/add"] = _ => {
        Flight newFlight = new Flight(Request.Form["arrival"], Request.Form["departure"], Request.Form["status"], Request.Form["airline-id"], Request.Form["city-id"]);
        newFlight.Save();
        return View["success.cshtml"];
      };
    }
  }
}
