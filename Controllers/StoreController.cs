using MusicStore1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore1.Context;

namespace MusicStore1.Controllers
{
    public class StoreController : Controller
    {
        MusicStoredb storeDB = new MusicStoredb();
        //
        // GET: /Store/
        public ActionResult Index()
        {
            var genres = storeDB.Genres.ToList();
            return View(genres);
        }

        //GET:/Store/Browse/
        public ActionResult Browse(string Genre)
        {
            var genreModel = storeDB.Genres.Include("Albums").Single(g => g.Name == Genre);
            return View(genreModel);
        }

        //GET:/Store/Details/
        public ActionResult Details(int id)
        {
            var album = storeDB.Albums.Find(id);
            return View(album);
        }
        
        //Get: /Store/GenreMenu
        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            var genres = storeDB.Genres.ToList();
            return PartialView(genres);
        }
	}
}