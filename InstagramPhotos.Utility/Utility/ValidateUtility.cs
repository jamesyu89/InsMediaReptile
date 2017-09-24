using System;
using System.Text.RegularExpressions;
using InstagramPhotos.Utility.Log;

namespace InstagramPhotos.Utility.Utility
{
    public class ValidateUtility
    {
        public static string ValidateKeyInfo(string value, string pattern, string paramName, Type type, string outMsg)
        {
            try
            {
                string error = string.Empty;
                if (value == null)
                {
                    error = string.Format("validate key info error on parameter '{0}' in class '{1}': null reference",
                        paramName, type.FullName);
                    throw new ArgumentException(error, paramName);
                }
                else
                {
                    if (!Regex.Match(value.Trim(), pattern.Trim(), RegexOptions.IgnoreCase).Success)
                    {
                        error = string.Format("validate key info error on parameter '{0}' in class '{1}': value '{2}' not match pattern '{3}'",
                            paramName, type.FullName, value, pattern);
                        throw new ArgumentException(error, paramName);
                    }
                }

                return value.Trim();
            }
            catch (ArgumentException e)
            {
                throw new Exception(outMsg, e);
            }
            catch (Exception ex)
            {
                //Logger.Exception(ex);
                throw new Exception(outMsg);
            }
        }
    }
}
