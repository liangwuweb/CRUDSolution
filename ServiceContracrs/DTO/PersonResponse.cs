using ServiceContracrs.Enums;
using System;
using System.Runtime.CompilerServices;
using Entities;
using System.Net;
using System.Data;

namespace ServiceContracrs.DTO
{   
    public  class PersonResponse
    {
        public Guid PersonId { get; set; }

        public string? PersonName { get; set; }

        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public Guid? CountryID { get; set; }

        public string? CountryName { get; set; }

        public string? Address { get; set; }

        public bool ReceiveNewsLetters { get; set; }

        public double? Age { get; set; }

        public override bool Equals(object? obj)
        {
            if (!(obj is PersonResponse) || obj == null) return false;
            PersonResponse other = obj as PersonResponse;
            return PersonId == other.PersonId &&
                PersonName == other.PersonName &&
                Email == other.Email &&
                DateOfBirth == other.DateOfBirth &&
                Gender == other.Gender &&
                CountryID == other.CountryID &&
                CountryName == other.CountryName &&
                Address == other.Address &&
                ReceiveNewsLetters == other.ReceiveNewsLetters;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Person ID: {PersonId}, Person Name: {PersonName}, Email: {Email}, Gender: {Gender}, DOB: {DateOfBirth?.ToString("dd MMM yyyy")}, CountryID: {CountryID}, Country Name: {CountryName}, Address: {Address}";
        }
    }

    public static class PersonExtensions {
        public static PersonResponse? ToPersonResponse(this Person person) {
            return new PersonResponse()
            {
                PersonId = person.PersonId,
                PersonName = person.PersonName,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryID = person.CountryID,
                Address = person.Address,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null
            };
        }
    
    }
}
