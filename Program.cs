using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace ImageListStripper
{
  public class Program
  {
    public static void Main(string[] args)
    {
      // stolen from:
      //    https://stackoverflow.com/questions/52644854/deserialize-windowsforms-imagelist

      //File containing only the value of the imagelist
      string source = args[0];

      BinaryFormatter formatter = new BinaryFormatter();

      string base64 = File.ReadAllText(source);
      byte[] bytes = Convert.FromBase64String(base64);

      using (Stream stream = new MemoryStream(bytes))
      {
        ImageListStreamer streamer = (ImageListStreamer)formatter.Deserialize(stream);

        //streamer.ImageList is actually null BUT that does not matter at all.

        ImageList list = new ImageList();
        list.ImageStream = streamer;

        var index = 0;

        //list is now filled with all the images !
        foreach (Bitmap image in list.Images)
        {
          //Got my Bitmap YAY !
          // NOTE:  could alternately be a png
          image.Save($"image{index}.bmp");

          index++;
        }
      }
    }
  }
}
