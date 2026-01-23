using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface ICountryService
    {
        Task<ApiResponse<PagedResponse<CountryGetDto>>> GetAllCountriesAsync(PagedRequest request);
        Task<ApiResponse<CountryGetDto>> GetCountryByIdAsync(long id);
        Task<ApiResponse<CountryGetDto>> CreateCountryAsync(CountryCreateDto countryCreateDto);
        Task<ApiResponse<CountryGetDto>> UpdateCountryAsync(long id, CountryUpdateDto countryUpdateDto);
        Task<ApiResponse<object>> DeleteCountryAsync(long id);
    }
}
