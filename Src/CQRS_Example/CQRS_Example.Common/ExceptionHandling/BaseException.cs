using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Example.Common.ExceptionHandling
{
    public class BaseException : Exception
    {
        public string ErrorIdentifier { get; }
        public ExceptionType Type { get; }

        public BaseException(string? message, string errorIdentifier, ExceptionType type = ExceptionType.General)
            : base(message)
        {
            ErrorIdentifier = errorIdentifier;
            Type = type;
        }

        public BaseException(string? message, Exception? innerException, string errorIdentifier, ExceptionType type = ExceptionType.General)
            : base(message, innerException)
        {
            ErrorIdentifier = errorIdentifier;
            Type = type;
        }

        public BaseException(string? message, string errorIdentifier, ExceptionType type, params object[] parameters)
            : base(message == null ? message : FormatLoggingString(message, parameters))
        {
            ErrorIdentifier = errorIdentifier;
            Type = type;
        }

        public BaseException(string? message, Exception? innerException, string errorIdentifier, ExceptionType type, params object[] parameters)
            : base(message == null ? message : FormatLoggingString(message, parameters), innerException)
        {
            ErrorIdentifier = errorIdentifier;
            Type = type;
        }

        public BaseException(string? message, string errorIdentifier, params object[] parameters)
            : this(message, errorIdentifier, ExceptionType.General, parameters)
        {
        }


        public BaseException(string? message, Exception? innerException, string errorIdentifier, params object[] parameters)
            : this(message, innerException, errorIdentifier, ExceptionType.General, parameters)
        {
        }

        private static string FormatLoggingString(string formattedString, params object[] args)
        {
            int currentIndex = 0;
            int argIndex = 0;
            string result = "";
            while(currentIndex < formattedString.Length)
            {
                if (formattedString[currentIndex] == '{')
                {
                    //Check if there are enough arguments
                    if (argIndex >= args.Length)
                    {
                        throw new ArgumentException("Insufficient arguments for formatted string");
                    }

                    //Find the closing curly brace
                    int endIndex = formattedString.IndexOf('}', currentIndex + 1);
                    if (endIndex == -1)
                    {
                        throw new ArgumentException("Invalid format string: missing closing curly braces");
                    }

                    //append the argument and increase the argument index
                    result += args[argIndex++].ToString() ?? "";
                    currentIndex = endIndex + 1;
                }
                else
                {
                    //Append the current character
                    result += formattedString[currentIndex];
                    currentIndex++;
                }
            }

            if(argIndex< args.Length)
            {
                throw new ArgumentException("Too many arguments for the formatted string");
            }

            return result;
        }
    }
}
