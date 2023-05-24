using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Azure;
using static Azure.Core.HttpHeader;


namespace Assignment3
{
    internal class Admin
    {
      
        public FileStream ListsDrivers;


        private int counter;

        public Admin()
        {
            
            ListsDrivers = null;
            counter = 0;


        }


        private bool CheckIfDriverExists(int id)
        {
            bool driverExists = false;
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookRide;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"SELECT COUNT(*) FROM Driver WHERE driver_id = {id}";
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    driverExists = (count > 0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error checking driver: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return driverExists;
        }



















        private bool CheckIfDriverExistsById_Name(int id, string name)
        {
            bool driverExists = false;
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookRide;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Driver WHERE driver_id = @id AND Name = @name";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                try
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    driverExists = (count > 0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error checking driver: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return driverExists;
        }


        public void AddDriver()
        {

            Console.ForegroundColor = ConsoleColor.Green;



            Console.ResetColor();

            Console.WriteLine();

            




            Console.Write("Enter name :");
            Console.ForegroundColor = ConsoleColor.Green;

            string name = Console.ReadLine();
            Console.ResetColor();

            Console.WriteLine();

           

            Console.Write("Enter Age :");

            Console.ForegroundColor = ConsoleColor.Green;



            

            int age = int.Parse(Console.ReadLine());
            Console.ResetColor();
            while (age < 0)
            {
                Console.WriteLine("Age could not be negative ");
                Console.Write("Enter Age :");
                Console.ForegroundColor = ConsoleColor.Green;

                age = int.Parse(Console.ReadLine());
                Console.ResetColor();
            }
            Console.WriteLine();

            Console.Write("Enter Address :");
            string address = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Enter Gender :");
            string gender = Console.ReadLine();
            while (gender != "Male" && gender != "Female")
            {
                Console.WriteLine("You have Entered the Wrong Gender");
                Console.Write("Enter Gender (Male or Female):");
                gender = Console.ReadLine();
                Console.WriteLine();
            }
            Console.WriteLine();

            Console.Write("Enter Phone_No :");
            Console.ForegroundColor = ConsoleColor.Green;



            
            string p_No = Console.ReadLine();
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("Enter Vehicle Type:");
            Console.ForegroundColor = ConsoleColor.Green;



            string v_type = Console.ReadLine();

            Console.ResetColor();
            Console.WriteLine();

            Console.Write("Enter Vehicle Model :");
            Console.ForegroundColor = ConsoleColor.Green;



            string v_model = Console.ReadLine();

            Console.ResetColor();
            Console.WriteLine();

            Console.Write("Enter Vehicle License Plate :");
            Console.ForegroundColor = ConsoleColor.Green;



            string v_l_plate = Console.ReadLine();

            Console.ResetColor();
            Console.WriteLine();


            Vehicle v = new Vehicle()
            {
                License_plate = v_l_plate,
                Type = v_type,
                Model = v_model
            };


            string conString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookRide;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            SqlConnection conn = new SqlConnection(conString);

            string query = $"insert into Driver(Name,Age,Gender,P_N,Address) values('{name}','{age}','{gender}','{p_No}','{address}');SELECT CAST(scope_identity() AS int);";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            int d_id= (int)cmd.ExecuteScalar();
            Console.WriteLine(d_id);
            string veh = $"insert into Vehicle(type,model,License_plate,driver_Id) values('{v.Type}','{v.Model}','{v.License_plate}','{d_id}')";

            SqlCommand vehicleData = new SqlCommand(veh, conn);
            vehicleData.ExecuteNonQuery();


            conn.Close();



            Driver x = new Driver()
            {
                id = d_id,
                Name = name,
                Age = age,
                Gender = gender,
                Address = address,
                Phone_no = p_No,
                vehicle = v


            };


        }






        public void SearchDriver()
        {

            Console.Write("enter your id :");
            Console.ForegroundColor = ConsoleColor.Green;
            int id = int.Parse(Console.ReadLine());
            Console.ResetColor();
            
            Console.WriteLine();

            Console.Write("enter your name :");
            Console.ForegroundColor = ConsoleColor.Green;
            string name = (Console.ReadLine());
            Console.ResetColor();
            
            Console.WriteLine();
            
            bool val = false;

            if(CheckIfDriverExistsById_Name(id,name))
            {
                Console.WriteLine("Hello " + name);
            }
            else
            {
                Console.WriteLine("Driver not Found" );
                return;

            }


           






            Console.WriteLine("Enter your Current location in the form of (Latitude,Longitude) :");
            Console.ForegroundColor = ConsoleColor.Green;



            String newloc = Console.ReadLine();
            Console.ResetColor();

            Console.WriteLine();
            string loc = "location";
            UpdateDriverField(id, loc, newloc);
            Driver searchDriver = new Driver();
            
           
           
            int choice = 1;
            while(choice!=3)
            {
                Console.WriteLine("1.  Change Availability");
                Console.WriteLine("2.  Change Location");
                Console.WriteLine("3.  Exit as Driver");

                Console.Write("Select  your Choice from (1 to 3) :");
                Console.ForegroundColor = ConsoleColor.Green;
                choice = int.Parse(Console.ReadLine());
                Console.ResetColor();


                if (choice == 1)
                {
                    searchDriver.UpdateAvailability(id);
                    

                }
                else if (choice == 2)
                {
                    searchDriver.updateLocation(id);
                   


                }
                else if (choice == 3)
                {

                    return;

                }

            }


           

        }




        public void Search()
        {
            bool val = false;

            Console.Write("Enter name :");

            Console.ForegroundColor = ConsoleColor.Green;
            string name = Console.ReadLine();
            Console.ResetColor();
            Console.WriteLine();

            

            Console.Write("Enter Address :");
            Console.ForegroundColor = ConsoleColor.Green;
            string address = Console.ReadLine();
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("Enter Gender :");
            Console.ForegroundColor = ConsoleColor.Green;
            string gender = Console.ReadLine();
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("Enter Phone_No :");
            Console.ForegroundColor = ConsoleColor.Green;
            string p_No = Console.ReadLine();
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("Enter Vehicle Type:");
            Console.ForegroundColor = ConsoleColor.Green;
            string v_type = Console.ReadLine();
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("Enter Vehicle Model :");
            Console.ForegroundColor = ConsoleColor.Green;
            string v_model = Console.ReadLine();
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("Enter Vehicle License Plate :");
            Console.ForegroundColor = ConsoleColor.Green;
            string v_l_plate = Console.ReadLine();
            Console.ResetColor();
            Console.WriteLine();


            string conString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookRide;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            SqlConnection conn = new SqlConnection(conString);
            conn.Open();




            string query = @"SELECT d.*, v.Type, v.Model, v.License_Plate 
                 FROM Driver d
                 INNER JOIN Vehicle v ON d.driver_ID = v.driver_ID";


            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                


                if ((string.IsNullOrEmpty(name) || (string)(dr.GetValue(1)) == name)&&

                    (string.IsNullOrEmpty(gender) || (string)(dr.GetValue(3)) == gender) &&
                    (string.IsNullOrEmpty(p_No) || (string)(dr.GetValue(4)) == p_No) &&
                    (string.IsNullOrEmpty(address) || (string)(dr.GetValue(5)) == address) &&

                    (string.IsNullOrEmpty(v_type) || (string)dr.GetValue(8) == v_type) &&
                    (string.IsNullOrEmpty(v_model) || (string)(dr.GetValue(9)) == v_model) &&
                    (string.IsNullOrEmpty(v_l_plate) || (string)(dr.GetValue(10)) == v_l_plate))
                {
                    val = true;
                    
                    Console.WriteLine("..................................................................................................................");
                    Console.WriteLine("{0,-15}{1,-10}{2,-10}{3,-10}{4,-15}{5,-15}{6,-10}", "Name", "Age", "Gender", "V.Type", "V.Model", "V.Licence", "id");

                    Console.WriteLine("{0,-15}{1,-10}{2,-10}{3,-10}{4,-15}{5,-15}{6,-10}", dr.GetValue(1) ,dr.GetValue(2), dr.GetValue(3),dr.GetValue(8),dr.GetValue(9),dr.GetValue(10), dr.GetValue(0));


                }
            }

            dr.Close();
            if (val == false)
            {
                Console.WriteLine("Driver not found ");
            }

            






















        }









        public void UpdateDriver()
        {
            Console.Write("Enter id: ");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine();

            if (!CheckIfDriverExists(id))
            {
                Console.WriteLine($"Driver with id {id} does not exist. Please try again with a valid id.");
                UpdateDriver();
                return;
            }

            Console.WriteLine("What do you want to update?");
            Console.WriteLine("1. Name");
            Console.WriteLine("2. Age");
            Console.WriteLine("3. Address");
            Console.WriteLine("4. Gender");
            Console.WriteLine("5. Phone Number");
            Console.WriteLine("6. Vehicle Type");
            Console.WriteLine("7. Vehicle Model");
            Console.WriteLine("8. Vehicle License Plate");
            Console.WriteLine("9. Exit as Updation");
            Console.Write("Enter your choice: ");

            Console.ForegroundColor = ConsoleColor.Green;
            int choice = int.Parse(Console.ReadLine());
            Console.ResetColor();
            Console.WriteLine();

            while (choice != 9)
            {




                string fieldToUpdate = "";
                string newValue = "";
                switch (choice)
                {
                    case 1:
                        fieldToUpdate = "Name";
                        Console.Write("Enter new name: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        newValue = Console.ReadLine();
                        Console.ResetColor();
                        break;
                    case 2:
                        fieldToUpdate = "Age";
                        Console.Write("Enter new age: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        newValue = Console.ReadLine();
                        Console.ResetColor();

                        while (!int.TryParse(newValue, out int age) || age < 0)
                        {
                            Console.WriteLine("Age must be a non-negative integer. Please enter again:");
                            Console.ForegroundColor = ConsoleColor.Green;
                            
                            newValue = Console.ReadLine();
                            Console.ResetColor();
                        }
                        break;
                    case 3:
                        fieldToUpdate = "Address";
                        Console.Write("Enter new address: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        newValue = Console.ReadLine();
                        Console.ResetColor();
                        break;
                    case 4:
                        fieldToUpdate = "Gender";
                        Console.Write("Enter new gender (Male or Female): ");
                        newValue = Console.ReadLine();
                        while (newValue != "Male" && newValue != "Female")
                        {
                            Console.WriteLine("Invalid gender. Please enter again (Male or Female):");
                            Console.ForegroundColor = ConsoleColor.Green;
                            newValue = Console.ReadLine();
                            Console.ResetColor();
                        }
                        break;
                    case 5:
                        fieldToUpdate = "P_N";
                        Console.Write("Enter new phone number: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        newValue = Console.ReadLine();
                        Console.ResetColor();
                        break;
                    case 6:
                        fieldToUpdate = "Type";
                        Console.Write("Enter new vehicle type: ");
                        Console.ForegroundColor = ConsoleColor.Green;         
                        newValue = Console.ReadLine();
                        Console.ResetColor();
                        break;
                    case 7:
                        fieldToUpdate = "Model";
                        Console.Write("Enter new vehicle model: ");
                        Console.ForegroundColor = ConsoleColor.Green;                   
                        newValue = Console.ReadLine();
                        Console.ResetColor();
                        break;
                    case 8:
                        fieldToUpdate = "License_plate";
                        Console.Write("Enter new vehicle license plate: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        newValue = Console.ReadLine();
                        Console.ResetColor();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        UpdateDriver();
                        return;
                }


                if (choice >= 6 && choice <= 8)

                {
                    UpdateVehicle(id, fieldToUpdate, newValue);

                }
                else
                {
                    UpdateDriverField(id, fieldToUpdate, newValue);
                }

                Console.WriteLine();
                Console.WriteLine();


                Console.WriteLine("What do you want to update?");
                Console.WriteLine("1. Name");
                Console.WriteLine("2. Age");
                Console.WriteLine("3. Address");
                Console.WriteLine("4. Gender");
                Console.WriteLine("5. Phone Number");
                Console.WriteLine("6. Vehicle Type");
                Console.WriteLine("7. Vehicle Model");
                Console.WriteLine("8. Vehicle License Plate");
                Console.WriteLine("9. Exit as Updation");
                Console.Write("Enter your choice: ");
                Console.ForegroundColor = ConsoleColor.Green;
                
                choice = int.Parse(Console.ReadLine());
                Console.ResetColor();
                Console.WriteLine();

            }



        }




        public static void UpdateDriverField(int id, string field, string value)
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

        public static void UpdateVehicle(int id, string field, string value)
        {
            string conString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookRide;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            SqlConnection conn = new SqlConnection(conString);
            string query = $"UPDATE Vehicle SET [{field}] = '{value}' WHERE driver_Id={id}";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            Console.WriteLine($"Driver with id {id} has been updated.");
        }







        public void RemoveDriver()
        {
            Console.WriteLine("Enter the driver ID:");
            Console.ForegroundColor = ConsoleColor.Green;
            int driverId = int.Parse(Console.ReadLine());
            Console.ResetColor();
            Console.WriteLine();

            string conString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookRide;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            SqlConnection conn = new SqlConnection(conString);

            // Check if there are any related records in the "dbo.Vehicle" table
            string checkQuery = $"SELECT COUNT(*) FROM dbo.Vehicle WHERE driver_Id='{driverId}'";
            SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
            conn.Open();

            int count = (int)checkCmd.ExecuteScalar();
            conn.Close();

            if (count > 0)
            {
                Console.WriteLine($"There are {count} vehicles associated with this driver. Deleting the vehicles...");

                // Delete related records in the "dbo.Vehicle" table
                string deleteQuery = $"DELETE FROM dbo.Vehicle WHERE driver_Id='{driverId}'";
                SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                conn.Open();
                deleteCmd.ExecuteNonQuery();
                conn.Close();

                Console.WriteLine($"Deleted {count} vehicles associated with this driver.");
            }

            // Delete the driver from the "Driver" table
            string deleteDriverQuery = $"DELETE FROM Driver WHERE driver_id='{driverId}'";
            SqlCommand deleteDriverCmd = new SqlCommand(deleteDriverQuery, conn);
            conn.Open();
            deleteDriverCmd.ExecuteNonQuery();
            conn.Close();

            Console.WriteLine($"Driver with ID {driverId} has been deleted.");
        }




       
       




    }
}
