using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Text.RegularExpressions;

namespace Services.Helpers
{
    public static class ImageFormatter
    {
        public static async Task<byte[]> CompressImageAsync(byte[] imageData, int newWidth, int newHeight)
        {
            using (var inputStream = new MemoryStream(imageData))
            {
                using (Image image = await Image.LoadAsync(inputStream))
                {           
                    if (image.Size.Height > newHeight || image.Size.Width > newWidth)
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(newWidth, newHeight),
                            //Crop(увеличивает изображение подгоняя под заданные рамки)
                            //BoxPad (по границам черные полосы)
                            //Max (максимальная граница становится в соответствии с заданой, а минимальная уменьшается в пропорции)
                            //Min (минимальная граница становится в соответствии с заданой, а максимальная увеличивается в пропорции)
                            //Pad (по границам черные полосы)
                            //Stretch (растягивает под заданные рамки)
                            Mode = ResizeMode.Max
                        }));
                    }
                    using (var outputStream = new MemoryStream())
                    {
                        await image.SaveAsync(outputStream, new JpegEncoder());
                        return outputStream.ToArray();
                    }
                }
            }
        }

        public static bool IsBase64StringIsImage(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }
            var mimeType = Regex.Match(input, @"data:(?<type>.+?),(?<data>.+)").Groups["type"].Value.Replace(";base64",string.Empty);
            return mimeType.Contains("image");            
        }
    }
}
