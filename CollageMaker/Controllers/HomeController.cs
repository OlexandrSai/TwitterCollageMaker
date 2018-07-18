using CollageMaker.Helpers;
using CollageMaker.Helpers.TemplateMethod.ImageMerger;
using CollageMaker.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Tweetinvi;
using Tweetinvi.Models;

namespace CollageMaker.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            Auth.SetUserCredentials("consumerKey", "consumerSecret","userAccessToken", "userAccessSecret");

            DataModel model = new DataModel();
            return View(model);
        }


        [HttpPost]
        public ActionResult Index(DataModel data)
        {
            if (ModelState.IsValid)
            {
                IUser user = Tweetinvi.User.GetUserFromScreenName(data.UserName);
                ViewBag.UserName = user.Name;
                //getting  list of user friends
                IUser[] friends = user.GetFriends(data.NumberOfColumns * data.NumberOfRows).ToArray();

                //getting urls of friends profile images
                string[] urls = new string[friends.Length];
                for (int i = 0; i < friends.Length; i++)
                {
                    urls[i] = friends[i].ProfileImageUrl;
                }

                CollageModel collageModel = new CollageModel() { Urls = urls, BrPointer = data.NumberOfColumns };

                UrlMerger urlsMerger = new UrlMerger();
                urlsMerger.MergeImages(urls, data.NumberOfColumns, data.NumberOfRows);
                return View("Result", collageModel);


                #region MergingFromFiles

                //Getting images of friends
                //ImagesDownloadHelper.GetFilesFromTwitter(friends);
                //DirectoryInfo di = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/"));
                //FileSystemMerger filesMerger = new FileSystemMerger();
                //filesMerger.MergeImages(di.ToString(), data.NumberOfColumns, data.NumberOfRows);
                //return View("Result", collageModel);

                #endregion
            }
            return View();
        }

        public ActionResult Download()
        {
            var dir = Server.MapPath("/Content/");
            var path = Path.Combine(dir, "result.jpg");
            return File(path, "image/jpeg", "result.jpg");
        }


    }
}