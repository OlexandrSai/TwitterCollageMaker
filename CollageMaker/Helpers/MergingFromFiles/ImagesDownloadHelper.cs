using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Tweetinvi.Models;

namespace CollageMaker.Helpers
{
    public class ImagesDownloadHelper
    {
        private static readonly Regex _illegalInFileName = new Regex(@"[\\/:*?""<>|]");
        public static void GetFilesFromTwitter(IUser[] friends)
        {
            //Deleting previous files in folder
            DirectoryInfo di = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/"));
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            //Downloading images of friends
            using (var client = new WebClient())
            {
                foreach (var get in friends)
                {
                    var mystring = _illegalInFileName.Replace(get.ProfileImageUrl, "");
                    client.DownloadFile(new Uri(get.ProfileImageUrl),
                        System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/") + mystring);
                }
            }
        }
    }
}