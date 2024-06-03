namespace DataCore.Exceptions
{
    public class StageManagerAccessError : Exception
    {
        public StageManagerAccessError():base("Вы не являетесь ответственным лицом и не можете добавить или изменить отчёт") { }
    }
}
