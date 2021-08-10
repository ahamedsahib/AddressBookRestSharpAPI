using AddressBookRestSharpAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace AddressBookRestSharpTestProject
{
    [TestClass]
    public class AddressBookRestSharpTest
    {
        //Initializing the restclient 
        RestClient client;

        [TestInitialize]

        public void Setup()
        {
            client = new RestClient("http://localhost:3000");
        }

        /// <summary>
        /// Method to get all person details from server
        /// </summary>
        /// <returns></returns>
        public IRestResponse GetAllPersons()
        {
            IRestResponse response = default;
            try
            {
                //Get request from json server
                RestRequest request = new RestRequest("/persons", Method.GET);
                //Requesting server and execute 
                response = client.Execute(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
         return response;
        }
        /// <summary>
        /// Test method to get all person details
        /// </summary>
        [TestMethod]
        public void TestMethodToGetAllPersonsFromJSONServer()
        {
            try
            {
                //calling get all persom method 
                IRestResponse response = GetAllPersons();
                //converting response to list og objects
                var res = JsonConvert.DeserializeObject<List<Person>>(response.Content);
                //Check Retrieve data and Local data are same
                Assert.AreEqual(4, res.Count);
                //Checking the response statuscode 200-ok
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                res.ForEach((x) =>
                {
                    Console.WriteLine($"id = {x.id} ,First name = {x.FirstName} , Last name = {x.LastName} , Phone number = {x.PhoneNumber} , address = {x.Address} , city ={x.City} , state = {x.State} , zipcode = {x.ZipCode} , emailid = {x.EmailId} ");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Method to add a json object to json server
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IRestResponse AddToJsonServer(JsonObject json)
        {
            IRestResponse response = default;
            try
            {
                RestRequest request = new RestRequest("/persons", Method.POST);
                //adding type as json in request and passing the json object 
                request.AddParameter("application/json", json, ParameterType.RequestBody);

                //Execute the request
                response = client.Execute(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;

        }

        /// <summary>
        /// test Method to add multiple data to the json server
        /// </summary>
        [TestMethod]
        public void TestForAddMultipleDataToJsonServerFile()
        {
            try
            {
                //list for storing person  data json objects
                List<JsonObject> personList = new List<JsonObject>();
                JsonObject json = new JsonObject();
                json.Add("FirstName", "Andres");
                json.Add("LastName", "Iniesta");
                json.Add("PhoneNumber", 908709876);
                json.Add("Address", "Egmore");
                json.Add("City", "chennai");
                json.Add("State", "TN");
                json.Add("ZipCode", 852963);
                json.Add("EmailId", "adini6@gmail.com");

                //add object to list
                personList.Add(json);

                JsonObject json1 = new JsonObject();
                json1.Add("FirstName", "Neymar");
                json1.Add("LastName", "jr");
                json1.Add("PhoneNumber", 9874509876);
                json1.Add("Address", "Port");
                json1.Add("City", "chennai");
                json1.Add("State", "TN");
                json1.Add("ZipCode", 231963);
                json1.Add("EmailId", "njr@mail.com");
                personList.Add(json1);

                personList.ForEach((x) =>
                {
                    AddToJsonServer(x);
                });
                //Check by gettting all person details
                IRestResponse response = GetAllPersons();
                //convert json object to person object
                var res = JsonConvert.DeserializeObject<List<Person>>(response.Content);

                res.ForEach((x) =>
                {
                    Console.WriteLine($"id = {x.id} ,First name = {x.FirstName} , Last name = {x.LastName} , Phone number = {x.PhoneNumber} , address = {x.Address} , city ={x.City} , state = {x.State} , zipcode = {x.ZipCode} , emailid = {x.EmailId} ");
                });
                //Checking the response statuscode 200-ok
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

    }
}
