using Data.Interfaces;
using Mapster;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class MapperService : IMapperService
    {
        public TypeAdapterConfig Config<TSource, TDestination>()
        {
            var _config = new TypeAdapterConfig();
            _config.NewConfig<TSource, TDestination>()
                .AddDestinationTransform((string x) => x ?? "")
                .AddDestinationTransform((int? x) => IntToNullable(x))
                .AddDestinationTransform((DateTime? x) => DateToNullable(x));
            return _config;
        }
        public TDestination MapConfig<TSource, TDestination>(TSource _source)
        {
            var _destination = _source.Adapt<TDestination>(Config<TSource, TDestination>());
            return _destination;
        }

        public TDestination MapConfig<TSource, TDestination>(TSource _source, TDestination _destination)
        {
            var config = new TypeAdapterConfig();
            config.NewConfig<TSource, TDestination>()
                .AddDestinationTransform((string x) => x ?? "")
                .AddDestinationTransform((int? x) => IntToNullable(x))
                .AddDestinationTransform((bool? x) => x ?? false)
                .AddDestinationTransform((DateTime? x) => DateToNullable(x))
                .AddDestinationTransform((decimal? x) => DecimalToNullable(x));

            _destination = _source.Adapt(_destination, Config<TSource, TDestination>());
            return _destination;
        }

        public IQueryable<TDestination> MapConfig<TSource, TDestination>(IQueryable<TSource> _source)
        {
            var _destination = _source.ProjectToType<TDestination>(Config<TSource, TDestination>());
            return _destination;
        }
        public TDestination MapDefault<TSource, TDestination>(TSource _source)
        {
            var _config = new TypeAdapterConfig();
            _config.NewConfig<TSource, TDestination>();
            var _destination = _source.Adapt<TDestination>(_config);
            _destination = DefaultValue(_destination);
            return _destination;
        }

        private static int? IntToNullable(int? val)
        {
            var nullVal = val == default(int) ? null : val;
            return nullVal;
        }
        private static DateTime? DateToNullable(DateTime? val)
        {
            var nullVal = val < new DateTime(1900, 1, 1) ? null : val;
            return nullVal;
        }

        private static decimal? DecimalToNullable(decimal? val)
        {
            val ??= decimal.Zero;
            return val;
        }

        private static TDestination DefaultValue<TDestination>(TDestination dest)
        {
            var propertyInfos = dest.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var val = propertyInfo.GetValue(dest, null);
                var attributes = propertyInfo.GetCustomAttributes(typeof(UIHintAttribute), true).OfType<UIHintAttribute>()
                    .Where(m => m.UIHint.Contains("option"));
                if (attributes.Any() && (val == null || string.IsNullOrWhiteSpace(val.ToString())) && propertyInfo.PropertyType == typeof(string))
                {
                    propertyInfo.SetValue(dest, "0", null);
                }
                else if (val == null && propertyInfo.PropertyType == typeof(string))
                {
                    propertyInfo.SetValue(dest, "", null);
                }
                else if (propertyInfo.PropertyType == typeof(int?))
                {
                    val ??= 0;
                    propertyInfo.SetValue(dest, IntToNullable(Convert.ToInt32(val)), null);
                }
                else if (propertyInfo.PropertyType == typeof(DateTime?))
                {
                    val ??= default(DateTime);
                    propertyInfo.SetValue(dest, DateToNullable(Convert.ToDateTime(val)), null);
                }
                else if (propertyInfo.PropertyType == typeof(bool?) && val == null)
                {
                    val ??= default(DateTime);
                    propertyInfo.SetValue(dest, false, null);
                }
            }
            return dest;
        }
    }
}
