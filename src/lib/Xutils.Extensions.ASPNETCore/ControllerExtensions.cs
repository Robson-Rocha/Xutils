using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Xutils.Extensions.ASPNETCore
{
    /// <summary>
    /// Provides extension methods for ASP.NET Core Controllers.
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Creates a CSV file response for an action method.
        /// </summary>
        /// <typeparam name="TData">The type of the data to be converted into an CSV.</typeparam>
        /// <param name="controller">The extended controller.</param>
        /// <param name="data">The data to be converted to a CSV</param>
        /// <param name="fileName">The name of the file response to be created</param>
        /// <param name="mimeType">Optional. The MIME type of the file response to be created. The default is "text/csv"</param>
        /// <returns></returns>
        public static FileResult Csv<TData>(this Controller controller, IEnumerable<TData> data, string fileName, string mimeType = "text/csv")
        {
            PropertyInfo[] dataProperties = typeof(TData).GetProperties();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(";", dataProperties.Select(p => p.Name).ToArray()));
            foreach (var item in data)
                sb.AppendLine(String.Join(";", dataProperties.Select(p => p.GetValue(item)?.ToString() ?? "").ToArray()));
            return controller.File(Encoding.Default.GetBytes(sb.ToString()), mimeType, fileName);
        }
    }
}

