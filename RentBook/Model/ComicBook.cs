using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ComicBook
{
    public int ComicBookID { get; set; }

    [StringLength(255)]
    public string Title { get; set; }

    [StringLength(255)]
    public string Author { get; set; }

    public decimal PricePerDay { get; set; }

    [JsonIgnore]
    public ICollection<RentalDetail> RentalDetails { get; set; } = new List<RentalDetail>();
}