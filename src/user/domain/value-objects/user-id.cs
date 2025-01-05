using UsersMicroservice.Core.Domain;

using UsersMicroservice.src.user.domain.exceptions;

namespace UsersMicroservice.src.user.domain.value_objects;
public class UserId : IValueObject<UserId>
{
    private readonly string _id;
    public UserId(string id)
    {
        if (UUIDValidator.IsValid(id))
        {
            _id = id;
        }
        else
        {
            throw new InvalidUserIdException();
        }
    }

    public string GetId()
    {
        return _id;
    }

    public bool Equals(UserId other)
    {
        return _id == other.GetId();
    }
}
