namespace DataCore.Exceptions
{
    public class ActionNotAllowedException : Exception
    {
        public ActionNotAllowedException() : base("У вас нет прав на выполнение данной операции") { }
    }
}
