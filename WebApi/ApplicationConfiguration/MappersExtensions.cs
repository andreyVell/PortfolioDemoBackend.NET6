using AutoMapper;
using Mappings;

namespace WebApi.ApplicationConfiguration
{
    public static class MappersExtensions
    {
        public static void AddCustomAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperMappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
