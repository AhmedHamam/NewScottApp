namespace Base.Application.Interfaces;

public interface IDateTime
{
    DateTimeOffset Now { get; }
}