using Microsoft.EntityFrameworkCore;

namespace Base.Application.Models;

/// <summary>
/// Represents a paginated list of items with pagination information
/// </summary>
/// <typeparam name="T">The type of elements in the list</typeparam>
public class PaginatedList<T>
{
    /// <summary>
    /// Gets the list of items in the current page
    /// </summary>
    public List<T> Items { get; }

    /// <summary>
    /// Gets the current page number (1-based)
    /// </summary>
    public int PageNumber { get; }

    /// <summary>
    /// Gets the total number of pages
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// Gets the total count of items across all pages
    /// </summary>
    public int TotalCount { get; }

    /// <summary>
    /// Gets the size of each page
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Initializes a new instance of the PaginatedList class
    /// </summary>
    /// <param name="items">The items in the current page</param>
    /// <param name="count">The total count of items across all pages</param>
    /// <param name="pageNumber">The current page number (1-based)</param>
    /// <param name="pageSize">The size of each page</param>
    public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
    }

    /// <summary>
    /// Gets a value indicating whether there is a previous page
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Gets a value indicating whether there is a next page
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>
    /// Creates a new instance of PaginatedList asynchronously from an IQueryable source
    /// </summary>
    /// <param name="source">The IQueryable source</param>
    /// <param name="pageNumber">The page number to get (1-based)</param>
    /// <param name="pageSize">The size of each page</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paginated list</returns>
    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }

    /// <summary>
    /// Creates a new instance of PaginatedList from an IEnumerable source
    /// </summary>
    /// <param name="source">The IEnumerable source</param>
    /// <param name="pageNumber">The page number to get (1-based)</param>
    /// <param name="pageSize">The size of each page</param>
    /// <returns>A new paginated list</returns>
    public static PaginatedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}