namespace Base.Application.Interfaces;

/// <summary>
/// Defines functionality for building CSV files
/// </summary>
public interface ICsvFileBuilder
{
    /// <summary>
    /// Builds a CSV file from a collection of records
    /// </summary>
    /// <typeparam name="T">The type of records to convert to CSV</typeparam>
    /// <param name="records">The collection of records to convert</param>
    /// <returns>The CSV file content as a byte array</returns>
    byte[] BuildFile<T>(IEnumerable<T> records) where T : class;

    /// <summary>
    /// Builds a CSV file from a DataTable
    /// </summary>
    /// <param name="dataTable">The DataTable to convert to CSV</param>
    /// <returns>The CSV file content as a byte array</returns>
    byte[] BuildFile(DataTable dataTable);

    /// <summary>
    /// Asynchronously builds a CSV file from a collection of records
    /// </summary>
    /// <typeparam name="T">The type of records to convert to CSV</typeparam>
    /// <param name="records">The collection of records to convert</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the CSV file content as a byte array</returns>
    Task<byte[]> BuildFileAsync<T>(IEnumerable<T> records, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Asynchronously builds a CSV file from a DataTable
    /// </summary>
    /// <param name="dataTable">The DataTable to convert to CSV</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the CSV file content as a byte array</returns>
    Task<byte[]> BuildFileAsync(DataTable dataTable, CancellationToken cancellationToken = default);
}