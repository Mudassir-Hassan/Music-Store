using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore1.Models;
using MusicStore1.Context;
using MusicStore1.ViewModels;

namespace MusicStore1.Controllers
{
    public class ShoppingCartController : Controller
    {
        MusicStoredb storedb = new MusicStoredb(); 

        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            //Set up Our View model
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            //return View
            return View(viewModel);
        }
        
        //
        //Get:/Store/AddToCart/5

        public ActionResult AddToCart (int id)
        {
            //Retrieve the Album from the database
            var addedAlbum = storedb.Albums
                .Single(album => album.AlbumId == id);
            

            //Add it to shopping Cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddtoCart(addedAlbum);

            //Go back to main Store page for more shopping
            return RedirectToAction("Index");
        }

        //
        //Ajax:/Shoppingcart/RemovefromCart/5

        [HttpPost]
        public ActionResult RemovefromCart(int id)
        {
            //Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
                
           //Get the name of the album for display confirmation
            string albumName = storedb.Carts
                .Single(item => item.RecordId == id).Album.Title;

            // Remove from the cart
            int itemCount = cart.RemoveFromCart(id);
            
            //Display the Confirmation Message
            var results= new ShoppingCartRemoveViewModel
            {
                Message=Server.HtmlEncode(albumName)+"has been removed from your shopping cart.",
                CartTotal=cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount=itemCount,
                DeleteId= id
            };
            return Json(results);
        }

        //
        //Get;/ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
	}
}