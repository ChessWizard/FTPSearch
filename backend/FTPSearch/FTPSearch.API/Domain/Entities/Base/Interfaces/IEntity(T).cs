namespace FTPSearch.API.Domain.Entities.Base.Interfaces;

public interface IEntity<T> : IEntity
{
    T Id { get; set; }
}