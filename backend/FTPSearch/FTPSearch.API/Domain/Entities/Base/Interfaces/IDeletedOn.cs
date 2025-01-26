namespace FTPSearch.API.Domain.Entities.Base.Interfaces;

public interface IDeletedOn : ISoftDeleteEntity
{
    DateTimeOffset? DeletedDate { get; set; }
}