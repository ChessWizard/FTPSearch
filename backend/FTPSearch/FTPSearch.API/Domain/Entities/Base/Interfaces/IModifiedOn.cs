namespace FTPSearch.API.Domain.Entities.Base.Interfaces;

public interface IModifiedOn
{
    DateTimeOffset? ModifiedDate { get; set; }
}