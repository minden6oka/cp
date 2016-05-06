using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Drawing;

namespace CinemaProject
{
    class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int pic = new DirectoryInfo("movies").GetFiles().Count();
            string path = "movies/" + (pic + 1) + ".jpg";
            byte[] image = (byte[]) value;
            Bitmap bmp = null;
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(image,0,image.Length);
                ms.Position = 0;
                bmp = (Bitmap)Bitmap.FromStream(ms);
                bmp.Save(path);
            }
            return Environment.CurrentDirectory + "/" + path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
