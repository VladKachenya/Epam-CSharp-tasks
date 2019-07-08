using System;
using System.Collections.Generic;
using System.Text;

namespace App
{
    interface IService
    {
        void Add(User user);

        User Get(long id);

    }
}
