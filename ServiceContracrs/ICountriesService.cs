using ServiceContracrs.DTO;
namespace ServiceContracrs

{
    // Represents business logic for manipulating Country entity
    public interface  ICountriesService
    {
        /// <summary>
        /// Adds a Country object to the list of countires
        /// </summary>
        /// <param name="countryAddRequest">country object to add</param>
        /// <returns>Return  the country object after adding it</returns>
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Returns all countries from the list
        /// </summary>
        /// <returns>All countries from the list as List of CountruResponse</returns>
        List<CountryResponse> GetAllCountries();

        /// <summary>
        /// Return a country object base on the given country id
        /// </summary>
        /// <param name="id">id to serach for</param>
        /// <returns>country object match the id</returns>
        CountryResponse? GetCountryByID(Guid? id);
    }
}