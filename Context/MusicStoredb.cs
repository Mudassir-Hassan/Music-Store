using MusicStore1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MusicStore1.Context
{
    public class MusicStoredb: DbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public System.Data.Entity.DbSet<MusicStore1.ViewModels.ShoppingCartViewModel> ShoppingCartViewModels { get; set; }


        //public DbSet<MusicStore1.Models.Artist> Artists { get; set; }
    }
}