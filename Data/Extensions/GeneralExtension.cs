using Data.Services;
using Newtonsoft.Json.Linq;
using Microsoft.OData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Data.Extensions
{
    public static class GeneralExtension
    {
        public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            var dict = source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );
            return dict;
        }

        public static IDictionary<string, object> NavKeyDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            var keyfields = source.GetType().GetCustomAttributesData().Where(m => m.AttributeType == typeof(KeyAttribute)).FirstOrDefault()
                .ConstructorArguments;
            if (keyfields.Count == 0)
                return new Dictionary<string, object>();
            var keyattr = keyfields[0];
            string[] keyarr = new string[] { "" };
            string valueType = keyattr.Value.GetType().Name;
            if (valueType.Equals("String", StringComparison.OrdinalIgnoreCase))
            {
                keyarr[0] = keyattr.Value.ToString();
            }
            else
            {
                IReadOnlyCollection<CustomAttributeTypedArgument> col = (IReadOnlyCollection<CustomAttributeTypedArgument>)keyattr.Value;
                keyarr = col.Select(s => s.Value.ToString()).ToArray();
            }
            var dict = source.GetType().GetProperties(bindingAttr).Where(m => keyarr.Contains(m.Name)).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );
            return dict;
        }

        public static bool TryParseJSON(this string json)
        {
            try
            {
                var obj = JToken.Parse(json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string GeneratePassword(int size = 6)
        {
            try
            {
                size = 6;

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

                //result = result.Substring(index, 1) + randomUpperCaseLetter + result.Substring(index + 1, result.Length-1);

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
            catch (CustomException ex)
            {

                throw new CustomException(ex.ToString()); // Add the original exception
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
            catch (CustomException ex)
            {

                throw new CustomException(ex.ToString()); // Add the original exception
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
            catch (CustomException ex)
            {

                throw new CustomException(ex.ToString()); // Add the original exception
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
            catch (CustomException ex)
            {

                throw new CustomException(ex.ToString()); // Add the original exception
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
            catch (CustomException ex)
            {

                throw new CustomException(ex.ToString()); // Add the original exception
            }
        }
    }
}
