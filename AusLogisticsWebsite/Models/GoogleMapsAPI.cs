using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;

namespace AusLogisticsWebsite.Models
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 1
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       27/03/16
    /// </summary>

    public class GoogleMapsAPI
    {
        public GeoRoute GetDirections(GeoRoute route)
        {
            return route;
        }

        public string CreateApiDirectionsRequest(GeoRoute route)
        {
            string origin = string.Format("origin={0},{1}",route.Origin.Latitude,route.Origin.Longitude);
            string destination = string.Format("destination={0},{1}", route.Destination.Latitude, route.Destination.Longitude);

            string urlRequest = string.Format("https://maps.googleapis.com/maps/api/directions/xml?{0}&{1}", origin, destination);
                                       
            return urlRequest;
        }

        public string CreateApiGeoCodeRequest(Geolocation location)
        {
            string locationAddress = "";
            locationAddress += location.Unit != null ? location.Unit + "+" : "";
            locationAddress += location.Number != null ? location.Number + "+" : "";
            locationAddress += location.Address != null ? location.Address.Replace(' ', '+') + "+" : "";
            locationAddress += location.Suburb != null ? location.Suburb + "+" : "";
            locationAddress += location.State != null ? location.State + "+" : "";
            locationAddress += location.PostCode != null ? location.PostCode + "+" : "";
            locationAddress += "AU";

            string urlRequest = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}", locationAddress);

            return urlRequest;
        }

        public string CreateApiReverseGeoCodeRequest(Geolocation location)
        {
            string urlRequest = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}", location.Latitude, location.Longitude);

            return urlRequest;
        }


        public XmlDocument MakeApiRequest(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                XmlDocument xmlResponse = new XmlDocument();
                xmlResponse.Load(response.GetResponseStream());

                return xmlResponse;

            }
            catch (Exception e)
            {
                throw new Exception("Make Google Maps API Request Exception", e.InnerException);
            }
        }

        public List<GeoRoute> ParseGeoRouteSteps(XmlDocument xmlResponse)
        {
            try
            {
                XmlNode root = xmlResponse.DocumentElement;

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlResponse.NameTable);

                XmlNodeList stepElements = xmlResponse.SelectNodes("//step", nsmgr);

                List<GeoRoute> routeSteps = new List<GeoRoute>();
                foreach (XmlNode step in stepElements)
                {

                    Geolocation stepOrigin = new Geolocation();
                    Geolocation stepDestination = new Geolocation();
                    GeoRoute stepRoute = new GeoRoute(stepOrigin, stepDestination, DateTime.Now);

                    string originLatitude = step.SelectSingleNode(".//start_location").SelectSingleNode(".//lat").InnerText;
                    stepOrigin.Latitude = Convert.ToDecimal(originLatitude);

                    string originLongitude = step.SelectSingleNode(".//start_location").SelectSingleNode(".//lng").InnerText;
                    stepOrigin.Longitude = Convert.ToDecimal(originLongitude);                 
                    
                    string destinationLatitude = step.SelectSingleNode(".//end_location").SelectSingleNode(".//lat").InnerText;
                    stepDestination.Latitude = Convert.ToDecimal(destinationLatitude);

                    string destinationLongitude = step.SelectSingleNode(".//end_location").SelectSingleNode(".//lng").InnerText;
                    stepDestination.Longitude = Convert.ToDecimal(destinationLongitude);

                    string stepSeconds = step.SelectSingleNode(".//duration").SelectSingleNode(".//value").InnerText;
                    stepRoute.RouteSeconds = Convert.ToInt32(stepSeconds);

                    string stepMeters = step.SelectSingleNode(".//distance").SelectSingleNode(".//value").InnerText;
                    stepRoute.RouteMeters = Convert.ToInt32(stepMeters);

                    routeSteps.Add(stepRoute);
                }

                return routeSteps;
            }
            catch (Exception e)
            {
                throw new Exception("Parse Route Steps Exception", e.InnerException);
            }
        }

        public Geolocation ParseReverseGeoCode(XmlDocument xmlResponse, Geolocation location)
        {
            try
            {
                XmlNode root = xmlResponse.DocumentElement;

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlResponse.NameTable);

                XmlNodeList response = xmlResponse.SelectNodes("//GeocodeResponse", nsmgr);
                try
                {
                    location.PostCode = response[0].SelectSingleNode("//result[./type ='postal_code']/address_component/short_name").InnerText;
                }
                catch
                { 
                    // No geocode postal code
                }

                try
                {
                    location.State = response[0].SelectSingleNode("//result[./type ='administrative_area_level_1']/address_component/short_name").InnerText;
                }
                catch
                { 
                    // No geocode state 
                }

                return location;
            }
            catch (Exception e)
            {
                throw new Exception("Parse Reverse GeoCode Exception", e.InnerException);
            }
        }

        public Geolocation ParseGeoCode(XmlDocument xmlResponse, Geolocation location)
        {

            try
            {
                XmlNode root = xmlResponse.DocumentElement;

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlResponse.NameTable);

                XmlNodeList response = xmlResponse.SelectNodes("//GeocodeResponse", nsmgr);
                try
                {
                    string latitudeText = response[0].SelectSingleNode("//result/geometry/location/lat").InnerText;
                    location.Latitude = Convert.ToDecimal(latitudeText);
                }
                catch
                {
                    // No geocode latitude
                }

                try
                {
                    string longitudeText = response[0].SelectSingleNode("//result/geometry/location/lng").InnerText;
                    location.Longitude = Convert.ToDecimal(longitudeText);
                }
                catch
                {
                    // No geocode longitude
                }

                return location;
            }
            catch (Exception e)
            {
                throw new Exception("Parse GeoCode Exception", e.InnerException);
            }
        }

        public GeoRoute GetGeoRoute(GeoRoute route)
        {
            string apiRequest = CreateApiDirectionsRequest(route);
            XmlDocument xmlResponse = MakeApiRequest(apiRequest);

            List<GeoRoute> routeSteps = ParseGeoRouteSteps(xmlResponse);

            List<string> stepStates = new List<string>();

            // Only Reverse GeoCode 1 in 10 steps for effeciency
            for (int i = 0; i < routeSteps.Count; i += 10)
            {

                routeSteps[i].Origin = ReverseGeoCode(routeSteps[i].Origin);
                if (routeSteps[i].Origin.State != null)
                {
                    stepStates.Add(routeSteps[i].Origin.State);
                }

                routeSteps[i].Destination = ReverseGeoCode(routeSteps[i].Destination);
                if (routeSteps[i].Destination.State != null)
                {
                    stepStates.Add(routeSteps[i].Destination.State);
                }
            }


            route.RouteMeters = routeSteps.Sum(s => s.RouteMeters);
            route.RouteSeconds = routeSteps.Sum(s => s.RouteSeconds);
            route.RouteStates = stepStates.Distinct().ToList();

            return route;
        }

        public Geolocation ReverseGeoCode(Geolocation location)
        {
            string apiRequest = CreateApiReverseGeoCodeRequest(location);
            XmlDocument xmlResponse = MakeApiRequest(apiRequest);
            location = ParseReverseGeoCode(xmlResponse, location);            

            return location;
        }

        public Geolocation GetGeoCode(Geolocation location)
        {
            string apiRequest = CreateApiGeoCodeRequest(location);
            XmlDocument xmlResponse = MakeApiRequest(apiRequest);
            location = ParseGeoCode(xmlResponse, location);

            return location;
        }
    }
}