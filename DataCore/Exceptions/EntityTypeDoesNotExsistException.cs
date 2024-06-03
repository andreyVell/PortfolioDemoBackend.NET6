namespace DataCore.Exceptions
{
    public class EntityTypeDoesNotExsistException : Exception
    {
        public EntityTypeDoesNotExsistException() : base("This entity type does not exist")
        {
        }
    }
}
