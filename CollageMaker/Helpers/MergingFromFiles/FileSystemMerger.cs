using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace CollageMaker.Helpers.TemplateMethod.ImageMerger
{
    public class FileSystemMerger 
    {
        public void MergeImages(string path, int columnsNumber, int rowsNumber)
        {
            //Converting profiles images from directory to list of Bitmaps
            List<Bitmap> bitmapList = ConvertToBitmaps(path);

            //Merging
            Merge(bitmapList, columnsNumber, rowsNumber);

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


        public List<Bitmap> ConvertToBitmaps(string folderPath)
        {
            List<Bitmap> bitmapList = new List<Bitmap>();
            List<string> imagesFromFolder = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories).ToList();
            

            // Loop Files
            foreach (string imgPath in imagesFromFolder)
            {
                try
                {
                    var bmp = (Bitmap)Image.FromFile(imgPath);

                    bitmapList.Add(bmp);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            return bitmapList;
        }

       
    }
}