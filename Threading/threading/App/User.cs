using System;
using System.Collections.Generic;
using System.Text;

namespace App
{
    public class User
    {
        // 4 bytes
        public int Id { get; set; }

        // 60 bytes
        public string Login { get; set; }

        // 60 bytes
        public string FirstName { get; set; }
        // 60 bytes
        public string LastName { get; set; }

        // 8 bytes
        public DateTime LastEnterDate { get; set; }

        // 60 bytes
        public string Email { get; set; }

        // 8 bytes
        public DateTime BirthDate { get; set; }

        // total 260
    }
}
