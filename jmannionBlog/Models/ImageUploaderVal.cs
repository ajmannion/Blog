using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;

namespace jmannionBlog.Models
{
    public class ImageUploaderVal
    {

        public bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            //Check for actual object
            if (file == null)
                return false;

            // Check source file must be less than 2 mb and greater than 1 Kb
            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024)
                return false;

            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    return ImageFormat.Jpeg.Equals(img.RawFormat) ||
                           ImageFormat.Png.Equals(img.RawFormat) ||
                           ImageFormat.Gif.Equals(img.RawFormat);
                }
            }

            catch
            {
                return false;
            }
        }
    }
}
