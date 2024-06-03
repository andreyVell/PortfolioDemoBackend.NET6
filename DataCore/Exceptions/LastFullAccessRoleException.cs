namespace DataCore.Exceptions
{
    public class LastFullAccessRoleException : Exception
    {
        public LastFullAccessRoleException(): base("Невозможно удалить или изменить данную роль (Это последняя роль с полным доступом, которая присвоена пользователю(ям), дальнейшее управление системой станет невозможным)") { }
    }
}
