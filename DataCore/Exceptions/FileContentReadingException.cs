namespace DataCore.Exceptions
{
    public class FileContentReadingException : Exception
    {
        public FileContentReadingException():base("Невозможно получить содержимое файла") { }
    }
}
