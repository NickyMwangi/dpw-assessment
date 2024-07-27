using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Library.Utility
{
    public class WebUtils
    {

        public static string GeneratePassword()
        {
            try
            {
                int size = 4;

                //HACK: temporary
                var rng = new RNGCryptoServiceProvider();
                var seed = new byte[size];
                rng.GetBytes(seed);

                var rand = new Random(BitConverter.ToInt32(seed, 0));
                const string alllowedCharacters = "23456789abcdefghjkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ@#$%&*";
                var result = new string(
                    Enumerable.Repeat(alllowedCharacters, size)
                              .Select(s => s[rand.Next(s.Length)])
                              .ToArray());

                var r = new Random();

                var index = r.Next(0, size);//get a randon number
                var randomUpperCaseLetter = GetRandomUpperCaseLetter();
                result = result.Insert(index, randomUpperCaseLetter);//insert in a random position


                index = r.Next(0, size);//get a randon number
                var randomLowerCaseLetter = GetRandomLowerCaseLetter();
                result = result.Insert(index, randomLowerCaseLetter);//insert in a random position

                index = r.Next(0, size);//get a randon number
                var randomSpecialCharacter = GetRandomSpecialCharacter();
                result = result.Insert(index, randomSpecialCharacter);//insert in a random position

                index = r.Next(0, size);//get a randon number
                var randomNumericDigit = GetRandomNumericDigit();
                result = result.Insert(index, randomNumericDigit);//insert in a random position






                return result;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString()); // Add the original exception
            }
        }

        private static string GetRandomUpperCaseLetter()
        {

            try
            {
                int size = 4;

                //HACK: temporary
                var rng = new RNGCryptoServiceProvider();
                var seed = new byte[size];
                rng.GetBytes(seed);

                var rand = new Random(BitConverter.ToInt32(seed, 0));
                const string alllowedCharacters = "ABCDEFGHJKLMNPQRSTUVWXYZ";
                var result = new string(
                    Enumerable.Repeat(alllowedCharacters, size)
                              .Select(s => s[rand.Next(s.Length)])
                              .ToArray());

                var r = new Random();
                var index = r.Next(0, size);//get a random number
                return result.Substring(index, 1);//return random character

            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString()); // Add the original exception
            }




        }

        private static string GetRandomLowerCaseLetter()
        {

            try
            {

                int size = 4;
                //HACK: temporary
                var rng = new RNGCryptoServiceProvider();
                var seed = new byte[size];
                rng.GetBytes(seed);

                var rand = new Random(BitConverter.ToInt32(seed, 0));
                const string alllowedCharacters = "abcdefghjkmnpqrstuvwxyz";
                var result = new string(
                    Enumerable.Repeat(alllowedCharacters, size)
                              .Select(s => s[rand.Next(s.Length)])
                              .ToArray());


                var r = new Random();
                var index = r.Next(0, size);//get a random number
                return result.Substring(index, 1);//return random character

            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString()); // Add the original exception
            }




        }

        private static string GetRandomSpecialCharacter()
        {

            try
            {
                int size = 4;

                //HACK: temporary
                var rng = new RNGCryptoServiceProvider();
                var seed = new byte[size];
                rng.GetBytes(seed);

                var rand = new Random(BitConverter.ToInt32(seed, 0));
                const string alllowedCharacters = "@#$%&*";
                var result = new string(
                    Enumerable.Repeat(alllowedCharacters, size)
                              .Select(s => s[rand.Next(s.Length)])
                              .ToArray());


                var r = new Random();
                var index = r.Next(0, size);//get a random number
                return result.Substring(index, 1);//return random character

            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString()); // Add the original exception
            }




        }

        private static string GetRandomNumericDigit()
        {

            try
            {
                int size = 4;

                //HACK: temporary
                var rng = new RNGCryptoServiceProvider();
                var seed = new byte[size];
                rng.GetBytes(seed);

                var rand = new Random(BitConverter.ToInt32(seed, 0));
                const string alllowedCharacters = "23456789";
                var result = new string(
                    Enumerable.Repeat(alllowedCharacters, size)
                              .Select(s => s[rand.Next(s.Length)])
                              .ToArray());


                var r = new Random();
                var index = r.Next(0, size);//get a random number
                return result.Substring(index, 1);//return random character

            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString()); // Add the original exception
            }




        }

        public static bool IsBase64String(string str)
        {

            if ((str.Length % 4) != 0)
            {
                return false;
            }

            // Check that the string matches the base64 layout
            Regex regex = new Regex(@"[^-A-Za-z0-9+/=]|=[^=]|={3,}$");
            if (regex.Match(str).Success)
            {
                return false;
            }

            try
            {
                // If no exception is caught, then it is possibly a base64 encoded string
                MemoryStream stream = new MemoryStream(Convert.FromBase64String(str));
                return true;
            }
            catch
            {
                // If exception is caught, then it is not a base64 encoded string
                return false;
            }
        }

        public static string ImageToString(Stream fileStream)
        {
            var mStreamer = new MemoryStream();
            mStreamer.SetLength(fileStream.Length);
            fileStream.Read(mStreamer.GetBuffer(), 0, (int)fileStream.Length);
            mStreamer.Seek(0, SeekOrigin.Begin);
            byte[] fileBytes = mStreamer.GetBuffer();
            return Convert.ToBase64String(fileBytes);
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
