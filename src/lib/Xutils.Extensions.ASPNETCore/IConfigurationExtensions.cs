using System;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Xutils.Extensions.ASPNETCore
{
    public static class IConfigurationExtensions
    {
        public static TOptions GetOptions<TOptions>(this IConfiguration configuration, string sectionName = null)
        {
            Type optionsType = typeof(TOptions);
            // ReSharper disable once PossibleNullReferenceException
            string typeName = optionsType.FullName.Split('.').Last();
            if (typeName.EndsWith("options", true, CultureInfo.InvariantCulture))
                typeName = typeName.Substring(0, typeName.Length - 7);
            TOptions options = Activator.CreateInstance<TOptions>();
            configuration.GetSection(sectionName ?? typeName).Bind(options);
            return options;
        }
        
    }
}