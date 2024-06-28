using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookByUsingAdo.Net
{
    public class AddressBookMain
    {
        static void Main(string[] args)
        {
            IAdressBookService addressBookService = new AddressBookServiceImpl();
            addressBookService.choices();
        }
    }
}
