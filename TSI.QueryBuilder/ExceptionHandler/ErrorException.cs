using System;
using System.Collections.Generic;
using System.Text;

namespace TSI.QueryBuilder.ExceptionHandler
{
    internal class ErrorException
    {
        public static string ThrowException(Exception exp)
        {
            string msg = "Error!...";

            if (exp.InnerException != null)
            {
                Exception innerException = exp;
                var innerMsg = "";
                do
                {
                    if (innerException.InnerException == null)
                        innerMsg = innerMsg + System.Environment.NewLine + (string.IsNullOrEmpty(innerException.Message) ? string.Empty : innerException.Message);
                    innerException = innerException.InnerException;
                }
                while (innerException != null);

                msg += System.Environment.NewLine + "Inner Exceptions: " + innerMsg;
            }
            else
            {
                msg += exp.Message;
            }

            return msg;
        }
    }
}
