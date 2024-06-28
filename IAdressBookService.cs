using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookByUsingAdo.Net
{
    public interface IAdressBookService
    {
        void choices();
        void createDataBase(string dataBaseName);
        void contactPersonData();
        void insertContactPerson(ContactPerson person);
        void updateContactPerson(string firstName);
        void deleteContactPerson(string firstName);
        void getAllContactPerson();
        void getAllContactPersonBasedOnCity(string city);
        void getAllContactPersonBasedOnState( string state);
    }
}
