using AutoMapper;
using System;
using System.Linq;
using System.Collections.Generic;

public static class AutoMap
{
    //public static List<string> Profiles = new List<string>();
    
    public class CustomProfile<T, TU> : Profile
    {
        public CustomProfile()
        {
            base.CreateMap<T, TU>();
        }
    }

    public static TU Convert<T, TU>(this T input) where TU : class, new()
    {
        //1. Create the mapping; this is required
        //AutoMapper.Mapper.CreateMap<T, TU>().MaxDepth(1); //.ForAllMembers(options);   
        //2. Do the actual mapping of data.
        try
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CustomProfile<T, TU>>();
            });
            var mapper = configuration.CreateMapper();
            TU output = mapper.Map<T, TU>(input);
            return output;
        }
        catch (Exception ex)
        {
            return new TU();
        }
    }

    public static TU Convert<T, TU>(this T input, Action<T, TU> customMapper) where TU : class, new()
    {
        TU output = Convert<T, TU>(input);
        //do some custom property assignment here if there are properties of the DTO that does not come from the Entity.
        if (customMapper != null)
        {
            customMapper(input, output);
        }
        return output;
    }
}
