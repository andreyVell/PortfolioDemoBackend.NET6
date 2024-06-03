namespace DataCore.Exceptions
{
    public class ClientAlreadyExistsException : Exception
    {
        public ClientAlreadyExistsException() : base("Клиент с таким логином уже существует") { }
    }
}
