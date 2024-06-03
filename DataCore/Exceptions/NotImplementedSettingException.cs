namespace DataCore.Exceptions
{
    public class NotImplementedSettingException: Exception
    {
        public NotImplementedSettingException(string settingName) : base(settingName + " setting is not implemented") { }
    }
}
