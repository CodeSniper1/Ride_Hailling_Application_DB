using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;
using System.Reflection;
using Azure;

namespace Assignment3
{
    internal class Ride
    {
        public int driver_id;
        private int p_id;
        public Location start_location { set; get; }
        public Location end_location { set; get; }
        public int price { set; get; }
        public Driver driver { set; get; }
        public Passenger passenger { set; get; }

        public Ride()
        {
            start_location = new Location();
            end_location = new Location();
            driver = new Driver();
            passenger = new Passenger();
            price=0;
            

        }

        public void AssignPassenger()
        {
            


            Console.Write("Enter name :");
            Console.ForegroundColor = ConsoleColor.Green;
            string name = Console.ReadLine();
            Console.ResetColor();
            Console.WriteLine();


            Console.Write("Enter phone_No :");
            Console.ForegroundColor = ConsoleColor.Green;
            string p_no = Console.ReadLine();
            Console.ResetColor();
            Console.WriteLine();
            
            passenger.Name = name;
            passenger.Phone_no = p_no;




        }


        public void SetLocation()
        {
            Console.Write("Enter your Starting Location in the form of (Laatitude,Longitude) :");
            Console.ForegroundColor = ConsoleColor.Green;
            String s_loc = Console.ReadLine();
            Console.ResetColor();
            Console.WriteLine();


            string[] start = s_loc.Split(',');

            start_location.Latitude = float.Parse(start[0]);
            start_location.Longitude = float.Parse(start[1]);





            Console.Write("Enter your Ending Location in the form of (Laatitude,Longitude) :");
            Console.ForegroundColor = ConsoleColor.Green;
            String e_loc = Console.ReadLine();
            Console.ResetColor();
            Console.WriteLine();

            string[] end = e_loc.Split(',');

            end_location.Latitude = float.Parse(end[0]);
            end_location.Longitude = float.Parse(end[1]);


            




        }


        public void assignDriver()
        {
            Console.Write("Enter Ride type(Car,Bike,Rikshaw):");
            Console.ForegroundColor = ConsoleColor.Green;
            String r_type = Console.ReadLine();
            Console.ResetColor();
            Console.WriteLine();

            string conString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookRide;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            using SqlConnection conn = new SqlConnection(conString);
            conn.Open();


            string query = @"SELECT d.*, v.Type, v.Model, v.License_Plate 
                 FROM Driver d
                 INNER JOIN Vehicle v ON d.driver_ID = v.driver_ID";

            using SqlCommand cmd = new SqlCommand(query, conn);
            using SqlDataReader dr = cmd.ExecuteReader();

            bool val = false;
            Driver closestDriver = new Driver();
            double minDistance = double.MaxValue;

            string name = "";
            int age;
            string gender = "";
            string p_n="";
            string address = "";
            bool available = true;
            string loc = "";
            string v_type = "";
            string v_model = "";
            string v_l_p = "";
            int d_id;

            
            while (dr.Read())
            {



                Vehicle v = new Vehicle();


                Driver d = new Driver();


                if (!dr.IsDBNull(7))  //lolcation of driver \must not be null
                {
                    
                    name = (string)dr[1];
                    age = (int)dr[2];
                    gender = (string)dr[3];
                    p_n = (string)dr[4];
                    address = (string)dr[5];
                    available = (bool)dr[6];
                    loc = (string)dr[7];
                    v_type= (string)dr[8];
                    v_model= (string)dr[9];
                    v_l_p = (string)dr[10];
                    d_id = (int)dr[0];
                   
                    

                   

                    v.Type = v_type;
                    v.Model = v_model;
                    v.License_plate = v_l_p;

                    

                    string[] loca = loc.Split(',');

                   


                    d.Name = name;
                    d.Age = age;
                    d.Phone_no = p_n;
                    d.Gender = gender;
                    d.Address = address;
                    d.Availability = available;
                    d.Curr_location.Latitude = float.Parse(loca[0]);
                    d.Curr_location.Longitude = float.Parse(loca[1]);
                    d.vehicle = v;
                   
                    if (d.Availability  && d.vehicle.Type == r_type)
                    {
                       
                        double distance = Math.Sqrt(Math.Pow(d.Curr_location.Latitude - start_location.Latitude, 2) + Math.Pow(d.Curr_location.Longitude - start_location.Longitude, 2));
                      
                        
                        
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closestDriver = d;
                            val = true;
                        }
                    }


                }
            }

            if (val == false)
            {
                Console.WriteLine("Driver not found ");
            }
            else
            {
                driver = closestDriver;
            }
        }











        public void CalculatePrice()
        {
            int fuel_price = 260;

           
            double distance = Math.Sqrt(Math.Pow(end_location.Latitude - start_location.Latitude, 2) +
                                    Math.Pow(end_location.Longitude - start_location.Longitude, 2));

            

            if (driver.vehicle.Type == "car" || driver.vehicle.Type == "Car")
            {

                 price = (int)((distance * fuel_price) / 15 + 20);
                //Console.WriteLine("The Price is :" + price);
                Console.WriteLine();

            }
            else if (driver.vehicle.Type == "bike" || driver.vehicle.Type == "Bike")

            {
                price = (int)((distance * fuel_price) / 50 + 5);
                //Console.WriteLine("The price is :" + price);
                Console.WriteLine();

            }

            else if (driver.vehicle.Type == "rikshaw" || driver.vehicle.Type == "Rikshaw")

            {
                price = (int)((distance * fuel_price) / 35 + 10);
               // Console.WriteLine("The Price is :" + price);
                Console.WriteLine();

            }

        }

    }
}
