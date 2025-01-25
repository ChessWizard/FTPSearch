using FTPSearch.API.Domain.Entities.Base.Interfaces;

namespace FTPSearch.API.Domain.Entities.Base;

public abstract class BaseEntity<T> : IEntity<T>
{
    public virtual T Id { get; set; }
}