using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Xutils.Extensions.ASPNETCore
{
    /// <summary>
    /// Provides extension methods for ASP.NET Core
    /// </summary>
    public static class AspNetCoreExtensions
    {
        private static TOptions OptionsFactory<TOptions>(IServiceProvider serviceProvider, string sectionName = null)
        {
            IConfiguration configuration = serviceProvider.GetService<IConfiguration>();
            Type optionsType = typeof(TOptions);
            // ReSharper disable once PossibleNullReferenceException
            string typeName = optionsType.FullName.Split('.').Last();
            if (typeName.EndsWith("options", true, CultureInfo.InvariantCulture))
                typeName = typeName.Substring(0, typeName.Length - 7);
            TOptions options = Activator.CreateInstance<TOptions>();
            configuration.GetSection(sectionName ?? typeName).Bind(options);
            return options;
        }

        /// <summary>
        /// Binds a configuration section to an instance of an option type.
        /// </summary>
        /// <typeparam name="TOptions">The options type.</typeparam>
        /// <param name="services">The extended <see cref="IServiceCollection"/>.</param>
        /// <param name="lifetime">Specifies the service lifetime.</param>
        /// <param name="sectionName">Optional. The configuration section to bind. If ommited, the name is inferred from the option type name.</param>
        public static void AddOptions<TOptions>(this IServiceCollection services, ServiceLifetime lifetime, string sectionName = null)
            where TOptions : class, new()
        {
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton(serviceProvider => OptionsFactory<TOptions>(serviceProvider, sectionName));
                    break;

                case ServiceLifetime.Scoped:
                    services.AddScoped(serviceProvider => OptionsFactory<TOptions>(serviceProvider, sectionName));
                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient(serviceProvider => OptionsFactory<TOptions>(serviceProvider, sectionName));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
            }
        }

        /// <summary>
        /// Provides methods and types for selecting the type from the resolved assembly to be added to the <see cref="IServiceCollection"/>
        /// </summary>
        public class TypesFromAssemblySelector
        {
            /// <summary>
            /// Provides methods for selecting the lifetimes for the reference type specified
            /// </summary>
            public class ReferenceTypeLifetimeSelector
            {
                private readonly TypesFromAssemblySelector _parent;
                private readonly Type _referenceInterfaceType;

                internal ReferenceTypeLifetimeSelector(TypesFromAssemblySelector parent, Type referenceInterfaceType)
                {
                    _parent = parent;
                    _referenceInterfaceType = referenceInterfaceType;
                }

                /// <summary>
                /// Specifies the <see cref="ServiceLifetime"/> and adds the selected type to the <see cref="IServiceCollection"/>
                /// </summary>
                /// <param name="lifetime">Specifies the service lifetime.</param>
                /// <returns>A continuation type for selecting the type from the resolved assembly.</returns>
                public TypesFromAssemblySelector As(ServiceLifetime lifetime)
                {
                    _parent._services.AddServicesFrom(_referenceInterfaceType, _parent._hintType, lifetime);
                    return _parent;
                }

                /// <summary>
                /// Adds the selected types to the <see cref="IServiceCollection"/> with Scoped lifetime
                /// </summary>
                /// <returns>A continuation type for selecting the type from the resolved assembly.</returns>
                public TypesFromAssemblySelector AsScoped() => As(ServiceLifetime.Scoped);

                /// <summary>
                /// Adds the selected types to the <see cref="IServiceCollection"/> with Singleton lifetime
                /// </summary>
                /// <returns>A continuation type for selecting the type from the resolved assembly.</returns>
                public TypesFromAssemblySelector AsSingleton() => As(ServiceLifetime.Singleton);

                /// <summary>
                /// Adds the selected types to the <see cref="IServiceCollection"/> with Transient lifetime
                /// </summary>
                /// <returns>A continuation type for selecting the type from the resolved assembly.</returns>
                public TypesFromAssemblySelector AsTransient() => As(ServiceLifetime.Transient);
            }

            /// <summary>
            /// Provides methods for selecting the lifetimes for multple reference types specified
            /// </summary>
            public class MultiReferenceTypeLifetimeSelector
            {
                private readonly TypesFromAssemblySelector _parent;
                private readonly Type[] _referenceInterfaceTypes;

                internal MultiReferenceTypeLifetimeSelector(TypesFromAssemblySelector parent, Type[] referenceInterfaceTypes)
                {
                    _parent = parent;
                    _referenceInterfaceTypes = referenceInterfaceTypes;
                }

                /// <summary>
                /// Specifies the <see cref="ServiceLifetime"/> and adds the selected types to the <see cref="IServiceCollection"/>
                /// </summary>
                /// <param name="lifetime">Specifies the service lifetime.</param>
                /// <returns>A continuation type for selecting the type from the resolved assembly.</returns>
                public TypesFromAssemblySelector As(ServiceLifetime lifetime)
                {
                    foreach (Type referenceInterfaceType in _referenceInterfaceTypes)
                        _parent._services.AddServicesFrom(referenceInterfaceType, _parent._hintType, lifetime);
                    return _parent;
                }

                /// <summary>
                /// Adds the selected types to the <see cref="IServiceCollection"/> with Scoped lifetime
                /// </summary>
                /// <returns>A continuation type for selecting the type from the resolved assembly.</returns>
                public TypesFromAssemblySelector AsScoped() => As(ServiceLifetime.Scoped);

                /// <summary>
                /// Adds the selected types to the <see cref="IServiceCollection"/> with Singleton lifetime
                /// </summary>
                /// <returns>A continuation type for selecting the type from the resolved assembly.</returns>
                public TypesFromAssemblySelector AsSingleton() => As(ServiceLifetime.Singleton);

                /// <summary>
                /// Adds the selected types to the <see cref="IServiceCollection"/> with Transient lifetime
                /// </summary>
                /// <returns>A continuation type for selecting the type from the resolved assembly.</returns>
                public TypesFromAssemblySelector AsTransient() => As(ServiceLifetime.Transient);
            }

            private readonly IServiceCollection _services;
            private readonly Type _hintType;

            internal TypesFromAssemblySelector(IServiceCollection services, Type hintType)
            {
                _services = services;
                _hintType = hintType;
            }

            /// <summary>
            /// Selects the types from the resolved assembly to be added to the <see cref="IServiceCollection"/>
            /// </summary>
            /// <param name="referenceInterfaceTypes">The interfaces from which the implementations will be injected into the <see cref="IServiceCollection"/>.</param>
            /// <returns>A <see cref="MultiReferenceTypeLifetimeSelector"/> which provides methods </returns>
            public MultiReferenceTypeLifetimeSelector AddServicesOf(params Type[] referenceInterfaceTypes) => new MultiReferenceTypeLifetimeSelector(this, referenceInterfaceTypes);

            /// <summary>
            /// Selects the types from the resolved assembly to be added to the <see cref="IServiceCollection"/>
            /// </summary>
            /// <param name="referenceInterfaceType">The interface from which the implementations will be injected into the <see cref="IServiceCollection"/>.</param>
            /// <returns>A <see cref="MultiReferenceTypeLifetimeSelector"/> which provides methods </returns>
            public ReferenceTypeLifetimeSelector AddServicesOf(Type referenceInterfaceType) => new ReferenceTypeLifetimeSelector(this, referenceInterfaceType);

            /// <summary>
            /// Selects the types from the resolved assembly to be added to the <see cref="IServiceCollection"/>
            /// </summary>
            /// <typeparam name="TReferenceInterfaceType">The interface from which the implementations will be injected into the <see cref="IServiceCollection"/>.</typeparam>
            /// <returns>A <see cref="MultiReferenceTypeLifetimeSelector"/> which provides methods </returns>
            public ReferenceTypeLifetimeSelector AddServicesOf<TReferenceInterfaceType>() => new ReferenceTypeLifetimeSelector(this, typeof(TReferenceInterfaceType));
        }

        /// <summary>
        /// Indicates a type whose assembly will be searched for types to be injected.
        /// </summary>
        /// <param name="services">The extended <see cref="IServiceCollection"/>.</param>
        /// <param name="hintType">The type whose assembly will be searched for types to be injected.</param>
        /// <returns>A continuation type for selecting the type from the resolved assembly.</returns>
        public static TypesFromAssemblySelector FromAssemblyOf(this IServiceCollection services, Type hintType) => new TypesFromAssemblySelector(services, hintType);

        /// <summary>
        /// Indicates a type whose assembly will be searched for types to be injected.
        /// </summary>
        /// <typeparam name="THintType">The type whose assembly will be searched for types to be injected.</typeparam>
        /// <param name="services">The extended <see cref="IServiceCollection"/>.</param>
        /// <returns>A continuation type for selecting the type from the resolved assembly.</returns>
        public static TypesFromAssemblySelector FromAssemblyOf<THintType>(this IServiceCollection services) => new TypesFromAssemblySelector(services, typeof(THintType));


        /// <summary>
        /// Adds to the provided <see cref="IServiceCollection"/> all types from the <c>hintType</c> assembly that implements the <c>referenceInterfaceType</c>.>
        /// </summary>
        /// <param name="services">The extended <see cref="IServiceCollection"/>.</param>
        /// <param name="referenceInterfaceType">The interface from which the implementations will be injected into the <see cref="IServiceCollection"/>.</param>
        /// <param name="hintType">The type whose assembly will be searched for types to be injected.</param>
        /// <param name="lifetime">Specifies the service lifetime.</param>
        public static void AddServicesFrom(this IServiceCollection services, Type referenceInterfaceType, Type hintType, ServiceLifetime lifetime)
        {
            if (!referenceInterfaceType.IsInterface)
                throw new ArgumentOutOfRangeException(
                    $"{referenceInterfaceType.Name} type parameter must be an interface");

            string methodName = null;

            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    methodName = "AddScoped";
                    break;
                case ServiceLifetime.Singleton:
                    methodName = "AddSingleton";
                    break;
                case ServiceLifetime.Transient:
                    methodName = "AddTransient";
                    break;
            }

            if (referenceInterfaceType.IsGenericTypeDefinition)
            {
                //Get the container method for adding generic type implementations of the reference interface (for IInterface<>, give ClassOne for IInterface<foo> and ClassTwo for IInterface<bar>)
                MethodInfo containerMethod =
                    typeof(ServiceCollectionServiceExtensions).GetMethods()
                        .Where(m => m.Name == methodName &&
                                    m.GetGenericArguments().Length == 0)
                        .Select(m => new { MethodInfo = m, ParameterInfos = m.GetParameters() })
                        .Where(m => m.ParameterInfos.Length == 3 &&
                                    m.ParameterInfos[0].ParameterType == typeof(IServiceCollection) &&
                                    m.ParameterInfos[1].ParameterType == typeof(Type) &&
                                    m.ParameterInfos[2].ParameterType == typeof(Type))
                        .Select(m => m.MethodInfo)
                        .First();

                foreach (Type type in hintType.Assembly.GetTypes().Where(t => t.IsClass && t.ContainsGenericParameters))
                {
                    foreach (Type interfaceType in
                        type.GetInterfaces().Where(i =>
                            i.IsGenericType && referenceInterfaceType.IsAssignableFrom(i.GetGenericTypeDefinition())))
                    {
                        containerMethod.Invoke(null, new object[] { services, interfaceType.GetGenericTypeDefinition(), type.GetGenericTypeDefinition() });
                    }
                }

            }
            else
            {
                //Get the container method for adding specifically the concrete type (for Class, give Class)
                MethodInfo containerMethod =
                    typeof(ServiceCollectionServiceExtensions).GetMethods()
                        .First(m => m.Name == methodName &&
                                    m.GetGenericArguments().Length == 1 &&
                                    m.GetParameters().FirstOrDefault()?.ParameterType == typeof(IServiceCollection));

                //Adds all the types that implement the reference interface
                foreach (Type type in hintType.Assembly.GetTypes().Where(t =>
                    ((TypeInfo)t).ImplementedInterfaces.Contains(referenceInterfaceType) && t.IsClass && !t.ContainsGenericParameters))
                    containerMethod.MakeGenericMethod(type).Invoke(null, new object[] { services });
            }
        }

        /// <summary>
        /// Adds to the provided <see cref="IServiceCollection"/> all types from the <c>hintType</c> assembly that implements the <c>referenceInterfaceType</c>.>
        /// </summary>
        /// <typeparam name="TReferenceInterface">The type whose assembly will be searched for types to be injected.</typeparam>
        /// <typeparam name="THintType">The type whose assembly will be searched for types to be injected.</typeparam>
        /// <param name="services">The extended <see cref="IServiceCollection"/>.</param>
        /// <param name="lifetime">Specifies the service lifetime.</param>
        public static void AddServicesFrom<TReferenceInterface, THintType>(this IServiceCollection services, ServiceLifetime lifetime)
            => AddServicesFrom(services, typeof(TReferenceInterface), typeof(THintType), lifetime);
    }
}
