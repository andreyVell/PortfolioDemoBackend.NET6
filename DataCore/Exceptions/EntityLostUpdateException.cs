namespace DataCore.Exceptions
{
    public class EntityLostUpdateException : Exception
    {
        public EntityLostUpdateException(string entityName) : base($"Объект {entityName} не может быть обновлён, потому что он уже был обновлён другим пользователем") { }
    }
}
