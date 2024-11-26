using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ComicBook
{
    public int ComicBookID { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public decimal PricePerDay { get; set; }
    public ICollection<RentalDetail> RentalDetails { get; set; } = new List<RentalDetail>();
}