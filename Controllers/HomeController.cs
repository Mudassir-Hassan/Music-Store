using MusicStore1.Context;
using MusicStore1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStore1.Controllers
{
    public class HomeController : Controller
    {
        MusicStoredb storeDB = new MusicStoredb();
        //
        // GET: /Home/
        public ActionResult Index()
        {
            //Get Most popular Albums
            var albums = GetTopSellingAlbums(5);
            return View(albums);
        }

        private List<Album> GetTopSellingAlbums(int Count)
        {
            //Group the order detail by album and return
            //the albums with the highest count
            return storeDB.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(Count)
                .ToList();
        }
	}
}