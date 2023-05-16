using System.ComponentModel.DataAnnotations;

namespace Entities
{
    /// <summary>
    /// Domain Mode lfor Country
    /// </summary>
    public class Country
    {
        [Key]
        public Guid CountryID { get; set; }

        public string? CountryName { get; set; }

        public virtual ICollection<Person>? persons { get; set; }

    }
}