using System;
using System.Collections.Generic;
using System.Text;

namespace App
{
    class User
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime LastEnterDate { get; set; }

        public string Email { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
