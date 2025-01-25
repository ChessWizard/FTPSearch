namespace FTPSearch.API.Domain.Entities.Base.Interfaces;

public interface IDeletedOn
{
    DateTimeOffset? DeletedDate { get; set; }
}