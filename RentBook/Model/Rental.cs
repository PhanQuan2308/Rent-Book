using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
public class Rental
{
    public int RentalID { get; set; }

    public int CustomerID { get; set; }

    public DateTime RentalDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    [StringLength(50)]
    public string Status { get; set; }

    [JsonIgnore]
    public Customer? Customer { get; set; }

    [JsonIgnore]
    public ICollection<RentalDetail> RentalDetails { get; set; } = new List<RentalDetail>();
}