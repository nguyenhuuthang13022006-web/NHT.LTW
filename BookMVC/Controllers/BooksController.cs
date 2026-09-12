using BookMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookMVC.Controllers;

public class BooksController : Controller
{
    private static readonly List<Book> Books = new()
    {
        new Book { Id = 1, Title = "Chí Phèo", AuthorId = 1, AuthorName = "Nam Cao", CategoryId = 1, CategoryName = "Văn học", Price = 500000, Image = "/images/chipheo.svg" },
        new Book { Id = 2, Title = "Lão Hạc", AuthorId = 1, AuthorName = "Nam Cao", CategoryId = 1, CategoryName = "Văn học", Price = 700000, Image = "/images/laohac.svg" },
        new Book { Id = 4, Title = "Conan Phiêu lưu ký", AuthorId = 2, AuthorName = "Aoyama", CategoryId = 2, CategoryName = "Truyện tranh", Price = 550000, Image = "/images/conan.svg" },
        new Book { Id = 6, Title = "Dương Xưa Mây Trắng", AuthorId = 3, AuthorName = "Nguyễn Hiến Lê", CategoryId = 3, CategoryName = "Kỹ năng", Price = 850000, Image = "/images/duongxua.svg" }
    };

    public IActionResult Index(int? authorId, int? categoryId, string? keyword)
    {
        var result = Books.AsEnumerable();

        if (authorId.HasValue)
            result = result.Where(x => x.AuthorId == authorId.Value);

        if (categoryId.HasValue)
            result = result.Where(x => x.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(keyword))
            result = result.Where(x => x.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase));

        var vm = new BookViewModel
        {
            Books = result.ToList(),
            AuthorId = authorId,
            CategoryId = categoryId,
            Keyword = keyword
        };

        ViewBag.Authors = Books.GroupBy(x => new { x.AuthorId, x.AuthorName })
            .Select(x => x.Key).OrderBy(x => x.AuthorName).ToList();

        ViewBag.Categories = Books.GroupBy(x => new { x.CategoryId, x.CategoryName })
            .Select(x => x.Key).OrderBy(x => x.CategoryName).ToList();

        return View(vm);
    }

    [HttpGet]
    public IActionResult Create() => View(new Book());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Book book)
    {
        if (!ModelState.IsValid)
            return View(book);

        book.Id = Books.Count == 0 ? 1 : Books.Max(x => x.Id) + 1;
        book.AuthorName = book.AuthorId switch
        {
            1 => "Nam Cao",
            2 => "Aoyama",
            3 => "Nguyễn Hiến Lê",
            _ => "Chưa xác định"
        };
        book.CategoryName = book.CategoryId switch
        {
            1 => "Văn học",
            2 => "Truyện tranh",
            3 => "Kỹ năng",
            _ => "Khác"
        };
        book.Image = "/images/default.svg";
        Books.Add(book);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var book = Books.FirstOrDefault(x => x.Id == id);
        return book == null ? NotFound() : View(book);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Book book)
    {
        var old = Books.FirstOrDefault(x => x.Id == book.Id);
        if (old == null) return NotFound();

        old.Title = book.Title;
        old.Price = book.Price;
        old.AuthorId = book.AuthorId;
        old.CategoryId = book.CategoryId;
        old.AuthorName = book.AuthorId switch
        {
            1 => "Nam Cao",
            2 => "Aoyama",
            3 => "Nguyễn Hiến Lê",
            _ => "Chưa xác định"
        };
        old.CategoryName = book.CategoryId switch
        {
            1 => "Văn học",
            2 => "Truyện tranh",
            3 => "Kỹ năng",
            _ => "Khác"
        };

        return RedirectToAction(nameof(Index));
    }
}
