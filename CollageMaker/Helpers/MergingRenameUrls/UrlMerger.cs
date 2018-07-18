using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;

namespace CollageMaker.Helpers.TemplateMethod.ImageMerger
{
    public class UrlMerger
    {
        public void MergeImages(string[] urls, int columnsNumber, int rowsNumber)
        {
            //Converting profiles images from urls to list of Bitmaps
            List<Bitmap> bitmapList = ConvertToBitmaps(urls);

            //Merging
            Merge(bitmapList, columnsNumber, rowsNumber);

        }

        public List<Bitmap> ConvertToBitmaps(string[] urls)
        {
            List<Bitmap> bitmapList = new List<Bitmap>();

            // Loop URLs
            foreach (string imgUrl in urls)
            {
                try
                {
                    WebClient wc = new WebClient();

                    // If proxy setting then set
                    //if (proxy != null)
                    wc.Proxy = new WebProxy();

                    // Download image
                    byte[] bytes = wc.DownloadData(imgUrl);
                    MemoryStream ms = new MemoryStream(bytes);
                    Image img = Image.FromStream(ms);

                    bitmapList.Add((Bitmap)img);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            return bitmapList;
        }

        public void Merge(IEnumerable<Bitmap> images, int columnsNumber, int rowsNumber)
        {
            var enumerable = images as IList<Bitmap> ?? images.ToList();

            var width = 0;
            var height = 0;

            // Get max width and height of the image
            width = columnsNumber * 48;
            height = rowsNumber * 48;


            // merge images
            var bitmap = new Bitmap(width, height);
            int xCount = 0;
            int yCount = 0;
            int count = 0;
            using (var g = Graphics.FromImage(bitmap))
            {
                foreach (var image in enumerable)
                {
                    if (count % columnsNumber == 0 && count != 0)
                    {
                        yCount += image.Height;
                        xCount = 0;
                    }
                    g.DrawImage(image, xCount, yCount);
                    xCount += image.Width;
                    count++;
                }

            }

            DirectoryInfo directoryName = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath("~/Content/"));
            bitmap.Save(directoryName + "result.jpg");
        }

        
    }
}