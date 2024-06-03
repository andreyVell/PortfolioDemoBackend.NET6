namespace DataCore.Exceptions.AvetonUser
{
    public class UserDoesNotExistException : Exception
    {
        public UserDoesNotExistException() : base("Пользователь не существует") { }
    }
}
