using AutoMapper;

namespace crm_api.Mappings
{
    // Aggregator profile kept to anchor AddAutoMapper(typeof(MappingProfile)) registration.
    // All mappings have been moved into domain-specific profiles under this folder.
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Intentionally left blank.
        }
    }
}
