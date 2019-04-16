namespace Xutils.Extensions
{
    using System;
    using System.Text;

    public static class AggregateExceptionExtensions
    {
        public static string GetAllMessages(this AggregateException aggregateException)
        {
            StringBuilder errorMessages = new StringBuilder();
            errorMessages.AppendLine(aggregateException.Message);
            if(aggregateException.InnerException != null)
                errorMessages.AppendLine($"{aggregateException.InnerException.GetType().Name}: {aggregateException.InnerException.Message}");
            foreach (Exception exception in aggregateException.Flatten().InnerExceptions)
                errorMessages.AppendLine($"{exception.GetType().Name}: {exception.Message}");
            return errorMessages.ToString();
        }
    }
}
