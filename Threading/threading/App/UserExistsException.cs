using System;

namespace App
{
    public class UserExistsException : Exception
    {
        public UserExistsException(string message = "User already exists")
            : base(message)
        { }
    }
}