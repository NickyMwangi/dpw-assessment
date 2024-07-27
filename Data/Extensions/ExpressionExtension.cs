using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;

namespace Service.Utility.Extensions
{
    public static class ExpressionExtension
    {
        public static Expression<Func<T, bool>> WhereExpression<T>(this string stringExpression, params string[] paramExpression)
        {
            var paramObjects = GetParamType(paramExpression);
            Expression<Func<T, bool>> exp =
                (Expression<Func<T, bool>>)DynamicExpressionParser.ParseLambda(typeof(T), typeof(bool), stringExpression, paramObjects);
            return exp;
        }

        public static object[] GetParamType(string[] expParams)
        {
            try
            {

                var result = new ArrayList();
                for (var i = 0; i < expParams.Length; i++)
                {
                    object vl = null;
                    if (expParams[i].Contains("(string[])"))
                    {
                        vl = expParams[i].Replace("(string[])", "").Split(';');
                    }
                    else
                    if (expParams[i].Contains("(int)"))
                    {
                        if (int.TryParse(expParams[i].Replace("(int)", ""), out int intvl))
                            vl = intvl;

                    }
                    else if (expParams[i].Contains("(decimal)"))
                    {
                        if (decimal.TryParse(expParams[i].Replace("(decimal)", ""), out decimal decvl))
                            vl = decvl;
                    }
                    else if (expParams[i].Contains("(bool)"))
                    {
                        if (bool.TryParse(expParams[i].Replace("(bool)", ""), out bool boolvl))
                            vl = boolvl;
                    }
                    else if (expParams[i].Contains("(date)"))
                    {
                        if (DateTime.TryParse(expParams[i].Replace("(date)", ""), out DateTime datevl))
                            vl = datevl;
                    }
                    else
                    {
                        vl = expParams[i];
                    }
                    result.Add(vl);
                }
                return result.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Dictionary<string, string> SearchExpression(this string[] searchfields, int paramCount, string fieldValue)
        {
            try
            {
                var result = new Dictionary<string, string>();
                if (fieldValue == null)
                    return result;
                foreach (var field in searchfields)
                {
                    string[] opArray = { ">", "<", "<=", "=>", "!=" };
                    var opval = CheckOperator(fieldValue);
                    if (field.Contains("(int)"))
                    {                        
                        if (int.TryParse(opval[1], out int vl))
                            result.Add(  $"{field.Replace("(int)", "")} {opval[0] } @{paramCount}",$"(int){fieldValue}");
                    }
                    else if (field.Contains("(decimal)"))
                    {
                        if (decimal.TryParse(opval[1], out decimal vl))
                            result.Add($"{field.Replace("(decimal)", "")} {opval[0] } @{paramCount}", $"(decimal){fieldValue}");
                    }
                    else if (field.Contains("(bool)"))
                    {
                        if (bool.TryParse(opval[1], out bool vl))
                            result.Add($"{field.Replace("(bool)", "")} {opval[0] } @{paramCount}", $"(bool){fieldValue}");
                    }
                    else if (field.Contains("(date)"))
                    {
                        if (DateTime.TryParse(opval[1], out DateTime vl))
                            result.Add($"{field.Replace("(date)", "")} {opval[0] } @{paramCount}", $"(date){fieldValue}");
                    }
                    else
                    {
                        result.Add($"{field}.Contains(@{paramCount})", fieldValue);
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string[] CheckOperator(string searchValue)
        {
            string[] result = { "=", searchValue };
            string[] opArray = { ">", "<", "<=", "=>" };
            foreach (string x in opArray)
            {
                if (searchValue.StartsWith(x))
                {
                    result[0] = x;
                    result[1] = searchValue.Replace(x, "");
                }
            }
            return result;
        }
    }
}
