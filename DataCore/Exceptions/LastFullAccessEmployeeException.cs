namespace DataCore.Exceptions
{
    public class LastFullAccessEmployeeException : Exception
    {
        public LastFullAccessEmployeeException() : base("Невозможно удалить или изменить данного пользователя (Это последний пользователь с полным доступом, дальнейшее управление системой станет невозможным)") { }
    }
}
