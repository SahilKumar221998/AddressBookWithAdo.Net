﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookByUsingAdo.Net
{
    public class InsertionUnsuccesfullException:Exception
    {
        public InsertionUnsuccesfullException(string Message):base(Message)
        {
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
