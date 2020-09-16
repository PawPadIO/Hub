using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace PawPadIO
{
    public static class DriverConfigurationExtensions
    {
        public static object GetValue(this IDriverConfiguration configuration, string key)
        {
            return GetValue(configuration, key, null);
        }

        public static object GetValue(this IDriverConfiguration configuration, string key, object defaultValue)
        {
            return configuration.GetSection(key).Value ?? defaultValue;
        }

        public static TValue GetValue<TValue>(this IDriverConfiguration configuration, string key)
        {
            return GetValue<TValue>(configuration, key, default);
        }

        public static TValue GetValue<TValue>(this IDriverConfiguration configuration, string key, TValue defaultValue)
        {
            var value = configuration.GetSection(key).Value;
            if (value != null)
                return (TValue)value;
            return defaultValue;
        }
    }

    public interface IDriverConfiguration
    {
        object this[string key] { get;set; }

        IEnumerable<IDriverConfigurationSection> GetChildren();

        IDriverConfigurationSection GetSection(string sectionName);
    }

    public interface IDriverConfigurationSection : IDriverConfiguration
    {
        string Key { get; }

        string Path { get; }

        object Value { get; set; }
    }
}
