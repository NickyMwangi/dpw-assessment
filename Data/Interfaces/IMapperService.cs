using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IMapperService
    {
        TDestination MapConfig<TSource, TDestination>(TSource _source);

        TDestination MapConfig<TSource, TDestination>(TSource _source, TDestination _destination);

        IQueryable<TDestination> MapConfig<TSource, TDestination>(IQueryable<TSource> _source);
        TDestination MapDefault<TSource, TDestination>(TSource _source);
    }
}
