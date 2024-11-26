using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Customer
{
    public int CustomerID { get; set; }

    [StringLength(255)]
    public string FullName { get; set; }

    [StringLength(15)]
    public string PhoneNumber { get; set; }

    public DateTime RegistrationDate { get; set; }

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}