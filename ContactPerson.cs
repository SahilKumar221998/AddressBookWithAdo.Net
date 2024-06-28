using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookByUsingAdo.Net
{
    public class ContactPerson
    {
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        private string address;
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        private string city;
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        private string state;
        public string State
        {
            get { return state; }
            set { state = value; }
        }
        private int zip;
        public int Zip
        {
            get { return zip; }
            set { zip = value; }
        }
        private long phone_Number;
        public long Phone_Number
        {
            get { return phone_Number; }
            set { phone_Number = value; }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        private string country;
        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        public ContactPerson(string firstName, string lastName, string address, string city, string state, int zip, long phone_Number, string email, string country)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.address = address;
            this.city = city;
            this.state = state;
            this.zip = zip;
            this.phone_Number = phone_Number;
            this.email = email;
            this.country = country;
        }
        public string ToString()
        {
            Console.WriteLine("-------------------------------------------------------------------------------------");
            return $"Name: {firstName} {lastName}\nAddress: {address}, {city}, {state}, {zip}, {country}\nPhone: {phone_Number}\nEmail: {email}";

        }
    }
}
