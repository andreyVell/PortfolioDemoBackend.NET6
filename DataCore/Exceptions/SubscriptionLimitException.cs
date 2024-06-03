namespace DataCore.Exceptions
{
    public class SubscriptionLimitException : Exception
    {
        public SubscriptionLimitException(): base("Невозможно выполнить действие, достигнут предел по вашему тарифному плану") { }
    }
}
