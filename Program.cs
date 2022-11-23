using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace ImageListStripper
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      // stolen from:
      //    https://stackoverflow.com/questions/52644854/deserialize-windowsforms-imagelist

      //File containing only the value of the imagelist
      var source = args[0];

      var formatter = new BinaryFormatter();

      var base64 = File.ReadAllText(source);
      var bytes = Convert.FromBase64String(base64);

      using (var stream = new MemoryStream(bytes))
      {
        var streamer = (ImageListStreamer)formatter.Deserialize(stream);

        //streamer.ImageList is actually null BUT that does not matter at all.

        var list = new ImageList();
        list.ImageStream = streamer;

        var index = 0;
        var sourceFileName = Path.GetFileNameWithoutExtension(source);

        Console.WriteLine($"Stripping images from: {source}");

        //list is now filled with all the images !
        foreach (Bitmap image in list.Images)
        {
          var fileName = $"{sourceFileName}-image-{index}.bmp";

          //Got my Bitmap YAY !
          // NOTE:  could alternately be a png or a MemoryBmp
          // save to disk as a presumptive bmp
          image.Save(fileName);

          // probe file on disk to find real image type and rename
          var data = File.ReadAllBytes(fileName);
          var imgFmt = GetImageFormat(data);
          var imgExtn = GetImageExtension(imgFmt);
          var newFileName = Path.ChangeExtension(fileName, imgExtn);
          File.Move(fileName, newFileName);

          Console.WriteLine($"  {newFileName}");

          index++;
        }
      }
    }

    private static string GetImageExtension(ImageFormat fmt)
    {
      switch (fmt)
      {
        case ImageFormat.Bmp:
          return "bmp";

        case ImageFormat.Jpeg:
          return "jpg";

        case ImageFormat.Gif:
          return "gif";

        case ImageFormat.Tiff:
          return "tif";

        case ImageFormat.Png:
          return "png";

        case ImageFormat.Unknown:
        default:
          throw new ArgumentOutOfRangeException($"Unknown image format: {fmt}");
      }
    }

    // stolen from:
    //    https://stackoverflow.com/questions/1397512/find-image-format-using-bitmap-object-in-c-sharp
    public enum ImageFormat
    {
      Bmp,
      Jpeg,
      Gif,
      Tiff,
      Png,
      Unknown
    }

    public static ImageFormat GetImageFormat(byte[] bytes)
    {
      // see http://www.mikekunz.com/image_file_header.html  
      var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
      var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
      var png = new byte[] { 137, 80, 78, 71 };    // PNG
      var tiff = new byte[] { 73, 73, 42 };         // TIFF
      var tiff2 = new byte[] { 77, 77, 42 };         // TIFF
      var jpeg = new byte[] { 255, 216, 255 };      // jpeg

      if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
      {
        return ImageFormat.Bmp;
      }

      if (gif.SequenceEqual(bytes.Take(gif.Length)))
      {
        return ImageFormat.Gif;
      }

      if (png.SequenceEqual(bytes.Take(png.Length)))
      {
        return ImageFormat.Png;
      }

      if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
      {
        return ImageFormat.Tiff;
      }

      if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
      {
        return ImageFormat.Tiff;
      }

      if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
      {
        return ImageFormat.Jpeg;
      }

      return ImageFormat.Unknown;
    }
  }
}
