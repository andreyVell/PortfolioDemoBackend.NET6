namespace DataCore.Exceptions
{
    public class FileIsNotImageException : Exception
    {
        public FileIsNotImageException() : base("Выбранный файл не является изображением") { }
    }
}
