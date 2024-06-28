using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookByUsingAdo.Net
{
    public class AddressBookServiceImpl:IAdressBookService
    {
        ContactPerson person;
        //Conection string
        string connectionString = "server=(localdb)\\MSSQLLocalDB;Initial Catalog=AddressBookDB;Integrated Security=SSPI";

        //DataBase Crud operation options
        public void choices()
        {
            SqlConnection con;
            string querry;
            SqlCommand command;
            int rows;
            while (true)
            {
                Console.WriteLine("1.Create DataBase\n2.Create Table\n3.Insert a contact person\n"+
                    "4.Update Contact person\n5.Delete a Contact person\n6.Get All Contacts\n"+
                    "7.Get Contact Person Based On City\n8.Get Contact Person Based On State\n9.Logout");
                int choices = Convert.ToInt32(Console.ReadLine());
                if (choices == 9)
                    break;
                switch (choices)
                {
                    //Create a Database
                    case 1:
                        Console.WriteLine("Enter the data Base Name");
                        string dataBase = Convert.ToString(Console.ReadLine());
                        createDataBase(dataBase);
                        break;
                    //Create a table
                    case 2: contactPersonData(); break;
                   //Insert a person
                    case 3:
                        string name;
                        while (true) {
                            Console.WriteLine("Enter the FirstName");
                            name = Console.ReadLine();
                            SqlConnection connection = new SqlConnection(connectionString);
                            string querrie = $"Select * from ContactPerson where FirstName='{name}'";
                            SqlCommand comm = new SqlCommand(querrie, connection);
                            connection.Open();
                            int row = comm.ExecuteNonQuery();
                            connection.Close();
                            if (row != 0)
                            {
                                break;

                            }
                            else
                            {
                                try
                                {
                                    throw new UserNameExistException("Name Already Present.Try Another Name");
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }
                        Console.WriteLine("Enter the LastName");
                        string lastname= Console.ReadLine();
                        
                        Console.WriteLine("Enter Phone Number:");
                        long phoneNumber =Convert.ToInt64(Console.ReadLine());

                        Console.WriteLine("Enter Email:");
                        string email = Console.ReadLine();

                        Console.WriteLine("Enter Address:");
                        string address = Console.ReadLine();

                        Console.WriteLine("Enter Zip Code:");
                        int zipCode=Convert.ToInt32(Console.ReadLine());
                        
                        Console.WriteLine("Enter City:");
                        string city = Console.ReadLine();

                        Console.WriteLine("Enter State:");
                        string state = Console.ReadLine();

                        Console.WriteLine("Enter Country:");
                        string country = Console.ReadLine();
                        person=new ContactPerson(name,lastname,address,city,state,zipCode,phoneNumber,email,country);
                        insertContactPerson(person);    
                        break;
                  
                    //Update a person
                    case 4:Console.WriteLine("Enter the FirstName");
                            name = Console.ReadLine();
                            con = new SqlConnection(connectionString);
                            querry = $"Select * from ContactPerson where FirstName='{name}'";
                            command = new SqlCommand(querry, con);
                            con.Open();
                            rows = command.ExecuteNonQuery();
                            con.Close();
                            if (rows != 0)
                            {
                            updateContactPerson(name);
                                break;

                            }
                            else
                            {
                                try
                                {
                                    throw new UserNameExistException("Username Does Not Exists!!!!");
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        break;
                        //Delete a Person
                    case 5:
                        Console.WriteLine("Enter the FirstName");
                        name = Console.ReadLine();
                        con = new SqlConnection(connectionString);
                        querry = $"Select * from ContactPerson where FirstName='{name}'";
                        command = new SqlCommand(querry, con);
                        con.Open();
                        rows = command.ExecuteNonQuery();
                        con.Close();
                        if (rows != 0)
                        {
                           deleteContactPerson(name);
                            break;

                        }
                        else
                        {
                            try
                            {
                                throw new UserNameExistException("Username Does Not Exists!!!!");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                        break;
                        //Get all Persons
                        case 6: getAllContactPerson(); break;
                    case 7:
                        Console.WriteLine("Enter a City:-");
                        city= Console.ReadLine();    
                        getAllContactPersonBasedOnCity(city);
                        break;
                    case 8:
                        Console.WriteLine("Enter a State:-");
                        state = Console.ReadLine();
                        getAllContactPersonBasedOnState(state);
                        break;
                    default: Console.WriteLine("Invalid Choice");break;
                }
            }
        }
        //Create database if not exists 
        public void createDataBase(string dataBaseName)
        {
            SqlConnection connection=new SqlConnection(connectionString);
            string dbQuerry = $"select * from sys.databases WHERE name = '{dataBaseName}'";
            SqlCommand command = new SqlCommand(dbQuerry, connection);
            connection.Open();
            command.ExecuteNonQuery();
            if (dbQuerry != null) {
                string querry = $"CREATE Database {dataBaseName}";
                command= new SqlCommand(querry, connection);    
                command.ExecuteNonQuery();
                connectionString =$"server=(localdb)\\MSSQLLocalDB;Initial Catalog={dataBaseName};Integrated Security=SSPI";
                Console.WriteLine("Data Base Created");
            }
            else
            {
                try
                {
                    throw new DatabaseAlreadyExistException("DataBase Already Exists");
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
           
            connection.Close(); 
        }
        
        //Create table of contactperson
        public void contactPersonData()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string querry = $"CREATE Table ContactPerson(FirstName varchar(20) primary key,LastName varchar(20) not null,"+
                            "Address varchar(40) not null,PhoneNumber varchar(10) check(len(PhoneNumber)=10),"+
                            "Email varchar(20) not null,ZipCode INT CHECK (LEN(CONVERT(VARCHAR, ZipCode)) = 6)," +
                            "City varchar(20) not null,State varchar(20) not null,Country varchar(20) not null)";
            SqlCommand command = new SqlCommand(querry,connection);
            connection.Open();
            try
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Table created Successfully");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            connection.Close() ;    
            
        }

        //Inserting a contact person into dadbase
        public void insertContactPerson(ContactPerson person)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string querry = "INSERT INTO ContactPerson(FirstName,LastName,Address,PhoneNumber,Email,ZipCode,City,State,Country)" +
                            "Values(@FirstName,@LastName,@Address,@PhoneNumber,@Email,@ZipCode,@City,@State,@Country)";
            using (SqlCommand command = new SqlCommand(querry, connection)) 
            {
                command.Parameters.AddWithValue("@FirstName", person.FirstName);
                command.Parameters.AddWithValue("@LastName", person.LastName);
                command.Parameters.AddWithValue("@Address", person.Address);
                command.Parameters.AddWithValue("@PhoneNumber", person.Phone_Number);
                command.Parameters.AddWithValue("@Email",person.Email);
                command.Parameters.AddWithValue("@ZipCode", person.Zip);
                command.Parameters.AddWithValue("@City", person.City);
                command.Parameters.AddWithValue("@State", person.State);
                command.Parameters.AddWithValue("@Country",person.Country);
                connection.Open();
                int rows=0;
                try
                {
                    rows = command.ExecuteNonQuery();
                }
                catch(SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
                if(rows<0)
                {
                    try
                    {
                        throw new InsertionUnsuccesfullException("Insertion Unsuccessfull!!!");
                    }
                    catch(Exception e) { Console.WriteLine(e.Message); }
                }
                else
                {
                    Console.WriteLine("Rows Effected:- "+rows);
                    Console.WriteLine("Insertion Successfull");
                    connection.Close();
                }
            }
        }
        // Update a Contact Person
        public void updateContactPerson(string firstName)
        {
            int rows = 0;
            string querry;
            SqlCommand command;
            SqlConnection connection = new SqlConnection(connectionString);//Creating a connection with DataBase
            Console.WriteLine("Select from options to update a contact");
            Console.WriteLine("1.LastName\n2.Address\n3.PhoneNumber\n4.Email\n5.ZipCode\n6.City\n7.State\n8.Country");
            int choice=Convert.ToInt32(Console.ReadLine());
            
            switch(choice)
            {
                case 1:
                    Console.WriteLine("Enter the LastName");
                    string lastname = Console.ReadLine();
                    querry = $"UPDATE ContactPerson SET Lastname='{lastname}' WHERE FirstName='{firstName}'"; //Querry for updation
                    connection.Open();
                    command = new SqlCommand(querry, connection);
                   
                    try
                    {
                        rows=command.ExecuteNonQuery();
                    }
                    catch(SqlException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    if (rows != 0)
                    {
                        Console.WriteLine("Rows Effected:-" + rows);
                        Console.WriteLine("Update succcessfully");
                        
                    }
                    connection.Close ();    
                    break;
                case 2:
                    Console.WriteLine("Enter Address:");
                    string address = Console.ReadLine();
                    querry = $"UPDATE ContactPerson SET Address='{address}' WHERE FirstName='{firstName}'";
                    connection.Open();
                   command = new SqlCommand(querry, connection);
                    try
                    {
                        rows = command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    if (rows != 0)
                    {
                        Console.WriteLine("Rows Effected:-" + rows);
                        Console.WriteLine("Update succcessfully");

                    }
                    connection.Close();
                    break;
                case 3:
                    Console.WriteLine("Enter Phone Number:");
                    long phoneNumber = Convert.ToInt64(Console.ReadLine());
                    querry = $"UPDATE ContactPerson SET PhoneNumber='{phoneNumber}' WHERE FirstName='{firstName}'";
                    connection.Open();
                    command = new SqlCommand(querry, connection);
                    try
                    {
                        rows = command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    if (rows != 0)
                    {
                        Console.WriteLine("Rows Effected:-" + rows);
                        Console.WriteLine("Update succcessfully");
                     }
                    connection.Close();
                    break;
                case 4:
                    Console.WriteLine("Enter Email:");
                    string email = Console.ReadLine();
                    querry = $"UPDATE ContactPerson SET Email='{email}' WHERE FirstName='{firstName}'";
                    connection.Open();
                    command = new SqlCommand(querry, connection);
                    try
                    {
                        rows = command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    if (rows != 0)
                    {
                        Console.WriteLine("Rows Effected:-" + rows);
                        Console.WriteLine("Update succcessfully");

                    }
                    connection.Close();
                    break;
                case 5:
                    Console.WriteLine("Enter Zip Code:");
                    int zipCode = Convert.ToInt32(Console.ReadLine());
                    querry = $"UPDATE ContactPerson SET ZipCode={zipCode} WHERE FirstName='{firstName}'";
                    connection.Open();
                    command = new SqlCommand(querry, connection);
                    try
                    {
                        rows = command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    if (rows != 0)
                    {
                        Console.WriteLine("Rows Effected:-" + rows);
                        Console.WriteLine("Update succcessfully");

                    }
                    connection.Close();
                    break;
                case 6:
                    Console.WriteLine("Enter City:");
                    string city = Console.ReadLine();
                    querry = $"UPDATE ContactPerson SET City='{city}' WHERE FirstName='{firstName}'";
                    connection.Open();
                    command = new SqlCommand(querry, connection);
                    try
                    {
                        rows = command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    if (rows != 0)
                    {
                        Console.WriteLine("Rows Effected:-" + rows);
                        Console.WriteLine("Update succcessfully");

                    }
                    connection.Close();
                    break;
                case 7:
                    Console.WriteLine("Enter State:");
                    string state = Console.ReadLine();
                    querry = $"UPDATE ContactPerson SET State='{state}' WHERE FirstName='{firstName}'";
                    connection.Open();
                    command = new SqlCommand(querry, connection);
                    try
                    {
                        rows = command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    if (rows != 0)
                    {
                        Console.WriteLine("Rows Effected:-" + rows);
                        Console.WriteLine("Update succcessfully");

                    }
                    connection.Close();
                    break;
                case 8:
                    Console.WriteLine("Enter Country:");
                    string country = Console.ReadLine();
                    querry = $"UPDATE ContactPerson SET Country='{country}' WHERE FirstName='{firstName}'";
                    connection.Open();
                    command = new SqlCommand(querry, connection);
                    try
                    {
                        rows = command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    if (rows != 0)
                    {
                        Console.WriteLine("Rows Effected:-" + rows);
                        Console.WriteLine("Update succcessfully");

                    }
                    connection.Close();
                    break;
                default:Console.WriteLine("Invalid choice"); 
                    break;

            }
        }

        //Delete a Contact person
        public void deleteContactPerson(string firstName)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string querry = $"DELETE FROM ContactPerson where FirstName='{firstName}'";
            SqlCommand command= new SqlCommand(querry, connection);
            connection.Open();
            int rows = 0;
            try
            {
                rows=command.ExecuteNonQuery();
            }
            catch(SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            if (rows > 0)
            {
                Console.WriteLine("Rows Effected :-" + rows);
                Console.WriteLine("Deleted Successfully");
            }
            connection.Close ();    
        }
        //Get All Contact Person
        public void getAllContactPerson()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string querry ="SELECT * FROM ContactPerson";
            SqlCommand command = new SqlCommand(querry, connection);
            connection.Open();
            try
            {
                SqlDataReader reader= command.ExecuteReader();
                if (reader.HasRows)
                {
                    while(reader.Read()) {
                        
                        string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                        string address = reader.GetString(reader.GetOrdinal("Address"));
                        long phoneNumber = Convert.ToInt64(reader.GetString(reader.GetOrdinal("PhoneNumber")));
                        string email = reader.GetString(reader.GetOrdinal("Email"));
                        int zipCode =reader.GetInt32(reader.GetOrdinal("ZipCode"));
                        string city = reader.GetString(reader.GetOrdinal("City"));
                        string state = reader.GetString(reader.GetOrdinal("State"));
                        string country = reader.GetString(reader.GetOrdinal("Country"));
                        person = new ContactPerson(firstName,lastName,address,city,state,zipCode,phoneNumber,email,country);
                        Console.WriteLine(person.ToString());
                        Console.WriteLine("-------------------------------------------------------------------------------------");
                    }
                }
                else
                {
                    throw new ContactNotFoundException("No Records Found!!!!!");
                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            connection.Close();
        }
        public void getAllContactPersonBasedOnCity(string city)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string querry = $"SELECT * FROM ContactPerson WHERE City='{city}'";
            SqlCommand command = new SqlCommand(querry, connection);
            connection.Open();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                        string address = reader.GetString(reader.GetOrdinal("Address"));
                        long phoneNumber = Convert.ToInt64(reader.GetString(reader.GetOrdinal("PhoneNumber")));
                        string email = reader.GetString(reader.GetOrdinal("Email"));
                        int zipCode = reader.GetInt32(reader.GetOrdinal("ZipCode"));
                        city = reader.GetString(reader.GetOrdinal("City"));
                        string state = reader.GetString(reader.GetOrdinal("State"));
                        string country = reader.GetString(reader.GetOrdinal("Country"));
                        person = new ContactPerson(firstName, lastName, address, city, state, zipCode, phoneNumber, email, country);
                        Console.WriteLine(person.ToString());
                        Console.WriteLine("-------------------------------------------------------------------------------------");
                    }
                }
                else
                {
                    throw new ContactNotFoundException("No Records Found!!!!!");
                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            connection.Close();
        }
        public void getAllContactPersonBasedOnState(string state)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string querry = $"SELECT * FROM ContactPerson WHERE State='{state}'";
            SqlCommand command = new SqlCommand(querry, connection);
            connection.Open();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                        string address = reader.GetString(reader.GetOrdinal("Address"));
                        long phoneNumber = Convert.ToInt64(reader.GetString(reader.GetOrdinal("PhoneNumber")));
                        string email = reader.GetString(reader.GetOrdinal("Email"));
                        int zipCode = reader.GetInt32(reader.GetOrdinal("ZipCode"));
                        string city = reader.GetString(reader.GetOrdinal("City"));
                        state = reader.GetString(reader.GetOrdinal("State"));
                        string country = reader.GetString(reader.GetOrdinal("Country"));
                        person = new ContactPerson(firstName, lastName, address, city, state, zipCode, phoneNumber, email, country);
                        Console.WriteLine(person.ToString());
                        Console.WriteLine("-------------------------------------------------------------------------------------");
                    }
                }
                else
                {
                    throw new ContactNotFoundException("No Records Found!!!!!");
                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            connection.Close();
        }
    }
}
