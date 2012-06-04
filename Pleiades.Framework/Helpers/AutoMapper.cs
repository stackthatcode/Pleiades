using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace Pleiades.Framework.Helpers
{
    public static class AutoMapperExtensions
    {
        public static T2 AutoMap<T1, T2>(this T1 source, T2 destination)
        {
            Mapper.CreateMap<T1, T2>();
            Mapper.Map(source, destination);
            return destination;
        }

        public static T2 AutoMap<T1, T2>(this T1 source) where T2 : new()
        {
            Mapper.CreateMap<T1, T2>();
            var destination = new T2();
            Mapper.Map(source, destination);
            return destination;
        }
    }
}
