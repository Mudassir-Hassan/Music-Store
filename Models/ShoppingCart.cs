using MusicStore1.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStore1.Models
{
    public partial class ShoppingCart
    {
        MusicStoredb storeDB = new MusicStoredb();

        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }



        //Helper Method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        //Add to Cart
        public void AddtoCart(Album album)
        {
            //Get the Matching Cart and album Instances
            var cartItem = storeDB.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId && c.AlbumId == album.AlbumId);

            if (cartItem == null)
            {
                //create a new cart item if no cart exists
                cartItem = new Cart
                {
                    AlbumId = album.AlbumId,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                storeDB.Carts.Add(cartItem);
            }
            else
            {
                //if item does exist in the cart then add one to the quantity
                cartItem.Count++;
            }
            storeDB.SaveChanges();
        }

        // Delete Item from the cart
        public int RemoveFromCart(int id)
        {
            //Get Cart
            var cartItem = storeDB.Carts.Single(
                cart => cart.CartId == ShoppingCartId && cart.RecordId == id
                );

            int itemCount = 0;
            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    storeDB.Carts.Remove(cartItem);
                }
                //save changes
                storeDB.SaveChanges();
            }
            return itemCount;
        }
        public void EmptyCart()
        {
            var cartItems = storeDB.Carts.Where(cart => cart.CartId == ShoppingCartId);
            foreach (var cartItem in cartItems)
            {
                storeDB.Carts.Remove(cartItem);
            }
            //save changes
            storeDB.SaveChanges();
        }
        public List<Cart> GetCartItems()
        {
            return storeDB.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            //Get Count of Each item in the cart And sum them up
            int? count = (from cartItems in storeDB.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            //Return 0 iff all enrtries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {
            //Multiply album price by count of that album to get
            //the current price for each of those albums in the cart
            //sum all album price totals to get the cart total

            decimal? total = (from cartItems in storeDB.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count * cartItems.Album.Price).Sum();
            return total ?? decimal.Zero;
        }
        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();
            foreach (var item in cartItems)
            {
                var OrderDetail = new OrderDetail
                {
                    AlbumId = item.AlbumId,
                    OrderId = order.OrderID,
                    UnitPrice = Convert.ToInt32(item.Album.Price),
                    Quantity = item.Count
                };
                //set the order total of the shopping cart
                orderTotal += (item.Count * item.Album.Price);

                storeDB.OrderDetails.Add(OrderDetail);
            }
            //set the order total to the order total count
            order.Total = orderTotal;

            //save the order
            storeDB.SaveChanges();

            //Empty the shopping cart
            EmptyCart();

            //Return the orderId as the confirmation number
            return order.OrderID;
        }

        //we are using httpcontextbase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    //Generate a new random GUID using system,GUID class
                    Guid tempCartId = Guid.NewGuid();

                    //send the tempCartId back to the client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();

                }
            }
            return context.Session[CartSessionKey].ToString();
        }
        //When a user has logged in migrate their shopping cart 
        //to be associated their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = storeDB.Carts.Where(c => c.CartId == ShoppingCartId);
            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            storeDB.SaveChanges();
            
        }
    }
}