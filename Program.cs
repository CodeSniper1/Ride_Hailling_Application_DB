using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Azure;
using System.Net;
using System.Reflection;

namespace Assignment3
{

    class Program
    {
        static void Main(string[] args)
        {
            Admin x = new Admin();
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;



                Console.ResetColor();

                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("                      Welocome To My Ride                          ");
                Console.WriteLine("-------------------------------------------------------------------");

                Console.WriteLine("1.  Book a Ride\n");
                Console.WriteLine("2.  Enter as Driver\n");
                Console.WriteLine("3.  Enter as Admin\n");

                Console.Write("\n \nSelect your choice from (1 to 3) :");
                Console.ForegroundColor = ConsoleColor.Green;
                int choice = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.ResetColor();

                if (choice == 1)
                {

                    Console.WriteLine("-------------------------------------------------------------------");
                    Console.WriteLine("                      BOOK A RIDE                           ");
                    Console.WriteLine("-------------------------------------------------------------------");

                    Console.WriteLine();
                    Ride ride = new Ride();
                    ride.AssignPassenger();
                 
                    ride.SetLocation();

                    ride.assignDriver();
                    ride.CalculatePrice();


                    Console.WriteLine("\n--------------Thank You--------------");
                    Console.WriteLine("\nPrice For this ride is : " + ride.price);
                   
                    char s;
                    Console.Write("\nEnter Y if you want to book th ride,Enter 'N' if you want to cancel : ");
                    Console.ForegroundColor = ConsoleColor.Green;



                  
                    s = char.Parse(Console.ReadLine());
                    Console.ResetColor();
                    Console.WriteLine();
                    string json = "";

                    string st_loc = $"{ride.start_location.Latitude},{ride.start_location.Longitude}";
                    string end_loc = $"{ride.end_location.Latitude},{ride.end_location.Longitude}";
                    if (s == 'Y')
                    {
                        Console.WriteLine("\nHappy Travel-----!");
                        Console.Write("\nGive Rating (out of 5) :");
                        Console.WriteLine();
                        Console.WriteLine();

                        string conString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookRide;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
                        SqlConnection conn = new SqlConnection(conString);

                        string query = $"insert into Ride(P_name,P_Pn,P_st_loc,P_end_loc,P_rideType,D_name,D_id,D_Pn) values('{ride.passenger.Name}','{ride.passenger.Phone_no}','{st_loc}','{end_loc}','{ride.driver.vehicle.Type}','{ride.driver.Name}','{ride.driver.id}','{ride.driver.Phone_no}');SELECT CAST(scope_identity() AS int);";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                       


                        conn.Close();



                    }
                    else if (s == 'N')
                    {
                        Console.WriteLine("\nYour Ride has been cancelled Now");
                    }


                   


                }
                else if (choice == 2)
                {
                    x.SearchDriver();
                  




                }
                else if (choice == 3)
                {
                    Console.WriteLine("\n 1.  Add Driver");
                    Console.WriteLine("\n 2.  Remove Driver");
                    Console.WriteLine("\n 3.  Update Driver");
                    Console.WriteLine("\n 4.  search Driver");
                    Console.WriteLine("\n 5.  Exit as Admin");

                    Console.Write("Enter your choice :");




                    Console.ForegroundColor = ConsoleColor.Green;



                  
                    int ch = int.Parse(Console.ReadLine());
                    Console.ResetColor();

                    while (ch < 5)
                    {



                        if (ch == 1)
                        {
                            x.AddDriver();

                        }
                        else if (ch == 2)
                        {
                            x.RemoveDriver();

                        }
                        else if (ch == 3)
                        {
                            x.UpdateDriver();


                        }

                        else if (ch == 4)
                        {
                            x.Search();


                        }
                        else if (ch == 5)
                        {
                            break;
                        }


                        Console.WriteLine("\n 1.  Add Driver");
                        Console.WriteLine("\n 2.  Remove Driver");
                        Console.WriteLine("\n 3.  Update Driver");
                        Console.WriteLine("\n 4.  search Driver");
                        Console.WriteLine("\n 5.  Exit as Admin");


                        Console.Write("\n Enter your choice :");
                        Console.ForegroundColor = ConsoleColor.Green;
                        ch = int.Parse(Console.ReadLine());
                        Console.ResetColor();

                        Console.WriteLine();
                    }


                }
            }




        }



    }
}











