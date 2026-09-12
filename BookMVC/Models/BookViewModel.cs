namespace BookMVC.Models;

public class BookViewModel
{
    public List<Book> Books { get; set; } = new();
    public int? AuthorId { get; set; }
    public int? CategoryId { get; set; }
    public string? Keyword { get; set; }
}
