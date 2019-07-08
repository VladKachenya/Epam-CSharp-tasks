using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace App
{
    class Service : IService
    {
        private object _lockObject = new object();
        private List<User> _users = new List<User>();
        private ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        public void Add(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            //lock (_lockObject)
            _rwLock.EnterWriteLock();
            {
                _users.Add(user); // !!!
            }
            _rwLock.ExitWriteLock();
        }

        public User Get(long id)
        {
            //lock (_lockObject)
            _rwLock.EnterReadLock();
            {
                foreach (var user in _users) // !!!
                {
                    if (user.Id == id)
                    {
                        return user;
                    }
                }
            }
            _rwLock.ExitReadLock();

            return null;
        }
    }
}
