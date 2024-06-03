namespace DataCore.Entities
{
    public abstract class AttachedFileBase : EntityBase
    {
        public string? FileName { get; set; }
        public string? FilePath { get; set; } = null!;
        public string? ImageMediumSizeFilePath { get; set; }
    }
}
