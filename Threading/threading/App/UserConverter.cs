using System;

namespace App
{
    public class UserConverter
    {
        public byte[] UserToBytes(User user)
        {
            byte[] res = new byte[260];
            int pointer = 0;
            FillByteArray(res, BitConverter.GetBytes(user.Id), ref pointer);
            FillByteArray(res, StringToBytes(user.Login), ref pointer);
            FillByteArray(res, StringToBytes(user.FirstName), ref pointer);
            FillByteArray(res, StringToBytes(user.LastName), ref pointer);
            FillByteArray(res, BitConverter.GetBytes(user.LastEnterDate.Ticks), ref pointer);
            FillByteArray(res, StringToBytes(user.Email), ref pointer);
            FillByteArray(res, BitConverter.GetBytes(user.BirthDate.Ticks), ref pointer);
            return res;
        }

        public User BytesToUser(byte[] bytesUser)
        {
            if (bytesUser.Length != 260)
            {
                throw new ArgumentException("Argument can be 260 light");
            }

            int pointer = 0;
            var res = new User();
            res.Id = BitConverter.ToInt32(CopyFromBytes(bytesUser, ref pointer, 4));
            res.Login = BytesToString(CopyFromBytes(bytesUser, ref pointer, 60));
            res.FirstName = BytesToString(CopyFromBytes(bytesUser, ref pointer, 60));
            res.LastName = BytesToString(CopyFromBytes(bytesUser, ref pointer, 60));
            res.LastEnterDate = new DateTime(BitConverter.ToInt64(CopyFromBytes(bytesUser, ref pointer, 8)));
            res.Email = BytesToString(CopyFromBytes(bytesUser, ref pointer, 60));
            res.BirthDate = new DateTime(BitConverter.ToInt64(CopyFromBytes(bytesUser, ref pointer, 8)));
            return res;
        }


        #region private methods

        private byte[] CopyFromBytes(byte[] sourse, ref int pointer, int length)
        {
            var res = new byte[length];
            Array.Copy(sourse, pointer, res, 0, length);
            pointer += length;
            return res;
        }

        private string BytesToString(byte[] byteStr)
        {
            string res = null;
            if (byteStr == null)
            {
                throw new ArgumentNullException($"Exception from {nameof(BytesToString)}");
            }

            foreach (var t in byteStr)
            {
                if (t == 0)
                {
                    res += ' ';
                }
                else
                {
                    res += (char)t;
                }
            }

            return res?.Trim();
        }

        private byte[] StringToBytes(string str)
        {
            var res = new byte[60];
            var chStr = String.IsNullOrWhiteSpace(str) ? new char[0] : str.ToCharArray();
            for (var i = 0; i < chStr.Length; i++)
            {
                if (i >= 60)
                {
                    break;
                }
                res[i] = (byte)chStr[i];
            }
            return res;
        }

        private void FillByteArray(byte[] filling, byte[] filler, ref int currentIndex)
        {
            for (var i = 0; i < filler.Length; i++)
            {
                if (currentIndex + i >= filling.Length)
                {
                    throw new IndexOutOfRangeException("Filling index out of range!");
                }
                filling[currentIndex + i] = filler[i];
            }

            currentIndex += filler.Length;
        }

        #endregion

    }
}