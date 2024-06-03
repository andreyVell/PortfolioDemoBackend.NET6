namespace DataCore.Exceptions.AvetonUser
{
    public class UserLoginIncorrectDataException : Exception
    {
        public UserLoginIncorrectDataException() : base("Логин или пароль неверные") { }
    }
}
