namespace Base.Application.Interfaces;

public interface ICsvFileBuilder<T> where T : class
{
    byte[] BuildTodoItemsFile(IEnumerable<T> records);
}