using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    internal class Driver
    {
        public int id { set; get; }
        public string Name { set; get; }
        public int Age { set; get; }
        public string Gender { set; get; }
        public string Address { set; get; }
        public string Phone_no { set; get; }

        public Location Curr_location { set; get; }
        public Vehicle vehicle { set; get; }
        public List<int> Rating { set; get; }

        public bool Availability { set; get; }

        public Driver()
        {
            Curr_location = new Location();
            vehicle = new Vehicle();
            Availability = true;
            Rating = new List<int>();
            Rating = null;

        }

        private static void UpdateDriverField(int id, string field, string value)
        {

            string conString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookRide;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            SqlConnection conn = new SqlConnection(conString);

            string query = $"UPDATE Driver SET [{field}] = '{value}' WHERE driver_Id = {id}";

            SqlCommand cm = new SqlCommand(query, conn);
            conn.Open();
            cm.ExecuteNonQuery();
            conn.Close();
            Console.WriteLine($"Driver with id {id} has been updated.");
        }




        public void UpdateAvailability(int ID)
        {
            Console.Write("To change your Availbility ,If your are available then Press 1 else press 0:");
            int num = int.Parse(Console.ReadLine());
            Console.WriteLine();
            if (num > 0)
            {
                Availability = true;

                UpdateDriverField(ID, "Availability", "true");
            }
            else
            {
                Availability = false;
                UpdateDriverField(ID, "Availability", "false");

            }

        }
        public void updateLocation(int ID)
        {
            Console.Write("Enter your Langitude position:");
            float latit = float.Parse(Console.ReadLine());
            Console.WriteLine();

            Console.Write("Enter your Longitude position:");
            float longit = float.Parse(Console.ReadLine());
            Console.WriteLine();
            Curr_location.Latitude = latit;
            Curr_location.Longitude = longit;

            string loc = $"{latit},{longit}";

            UpdateDriverField(ID, "location",loc);


        }

        public double GetRating()
        {
            double sum = 0;
            foreach (int rating in Rating)
            {
                sum += rating;
            }
            return sum / Rating.Count;
        }

    }
}
