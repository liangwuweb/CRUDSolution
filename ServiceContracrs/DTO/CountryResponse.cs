using System;
using Entities;

namespace ServiceContracrs.DTO
{
    // DTO class that is used as return type for nist of CountiesService methos
    public class CountryResponse
    {
        public Guid CountryID { get; set; }

        public string CountryName { get; set; }

        public override bool Equals(object? obj)
        {
            if (!(obj is CountryResponse) || obj == null) return false;
           CountryResponse other = obj as CountryResponse;
            return  CountryID == other.CountryID && 
                CountryName == other.CountryName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }

    public static class CountryExtensions {
        public static CountryResponse ToCountryResponse(this Country country) {
            return new CountryResponse()
            {
                CountryID = country.CountryID,
                CountryName = country.CountryName
            };
        }

    }
}
