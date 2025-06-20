namespace EstiloMestre.Domain.Repositories.Owner;

public interface IOwnerRepository
{
    Task Add(Entities.Owner owner);
    Task<Entities.Owner?> GetByUserId(long userId);
}
