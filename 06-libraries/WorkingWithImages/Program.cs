using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System.Diagnostics;

string imagesFolder = Path.Combine(Environment.CurrentDirectory, "images");

WriteLine($"Looking for images in the following directory {imagesFolder}");

if (!Directory.Exists(imagesFolder))
{
    WriteLine("");
    WriteLine("Folder does not exist!");
    return;
}

IEnumerable<string> images = Directory.EnumerateFiles(imagesFolder);

foreach (string imagePath in images)
{
    if (Path.GetFileNameWithoutExtension(imagePath).EndsWith("-thumbnail"))
    {
        WriteLine("Skipping thumbnail {0}", Path.GetFileName(imagePath));
        WriteLine();
        continue;
    }

    string thumbnailPath = Path.Combine(
        Environment.CurrentDirectory,
        "images",
        Path.GetFileNameWithoutExtension(imagePath) + "-thumbnail" + Path.GetExtension(imagePath)
        );

    using (Image image = Image.Load(imagePath))
    {
        WriteLine($"Converting: {imagePath}");
        WriteLine($"To: {thumbnailPath}");

        image.Mutate(x => x.Resize(image.Width / 10, image.Height / 10));
        image.Mutate(x => x.Grayscale());
        image.Save(thumbnailPath);
        WriteLine("Done: {0}", thumbnailPath);
        WriteLine();
    }
}

WriteLine("Image processing completed");

if (OperatingSystem.IsWindows())
{
    Process.Start("explorer.exe", imagesFolder);
}
else
{
    WriteLine("View images in folder: {0}", imagesFolder);
}
