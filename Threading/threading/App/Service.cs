using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace App
{
    class Service : IService
    {
        private readonly object _lockObject = new object();
        private readonly Dictionary<int, int> _offsetTable;
        private readonly ReaderWriterLockSlim _rwLock;
        private readonly string _dirPath = Directory.GetCurrentDirectory() + "/data.dat";
        private readonly Func<UserConverter> _userConverterFun;

        public Service()
        {
            _userConverterFun = () => new UserConverter();
            _offsetTable = new Dictionary<int, int>();
            _rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            Initialization();
        }

        public void Add(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (_offsetTable.Any(ur => ur.Key == user.Id))
            {
                throw new UserExistsException();
            }

            _rwLock.EnterWriteLock();
            try
            {
                using (FileStream fstream = new FileStream(_dirPath, FileMode.Append, FileAccess.Write))
                {
                    var byteUser = _userConverterFun().ToBytes(user);
                    fstream.Write(byteUser, 0, byteUser.Length);
                    int offset = _offsetTable.Count * 260;
                    _offsetTable.Add(user.Id, offset);
                }
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public User Get(long id)
        {
            _rwLock.EnterReadLock();
            try
            {
                var offset = _offsetTable.FirstOrDefault(ur => ur.Key == id).Value;
                using (FileStream fstream = File.OpenRead(_dirPath))
                {
                    byte[] byteUser = new byte[260];
                    fstream.Seek(offset, SeekOrigin.Begin);
                    fstream.Read(byteUser, 0, 260);
                    return _userConverterFun().ToUser(byteUser);
                }
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }

        #region Initialization

        public void Initialization()
        {
            using (FileStream fstream = new FileStream(_dirPath, FileMode.OpenOrCreate))
            {
                var fsLength = fstream.Length;
                if (fsLength == 0)
                {
                    return;
                }
                if (fsLength % 260 != 0)
                {
                    throw new Exception("Data is corrupted!");
                }

                int count = (int)(fsLength / 260);
                for (int j = 0; j < count; j++)
                {
                    var offset = j * 260;
                    var byteId = new byte[4];
                    fstream.Seek(offset, SeekOrigin.Begin);
                    fstream.Read(byteId, 0, 4);

                    var id = BitConverter.ToInt32(byteId);
                    _offsetTable.Add(id, offset);
                }

            }
        }

        #endregion
    }
}
