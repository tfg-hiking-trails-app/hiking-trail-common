namespace Common.Domain.Exceptions;

public class EntityAlreadyExistsException : Exception
{
    public EntityAlreadyExistsException(string entity, string property, string value) 
        : base($"The {property}: '{value}' in the entity {entity} already exists in the database.")
    {
    }

    public EntityAlreadyExistsException(string message) : base(message)
    {
    }
}