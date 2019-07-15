using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    interface IService
    {
        Task AddAsync(User user);

        Task<User> GetAsync(long id);

    }
}
