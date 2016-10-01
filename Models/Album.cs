using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStore1.Models
{
    [Bind(Exclude="AlbumId")]
    public class Album
    {
        [ScaffoldColumn(false)]
        public int AlbumId { get; set; }

        [DisplayName("Genre")]
        public int GenreId { get; set; }

        [DisplayName("Artist")]
        public int ArtistId { get; set; }

        [StringLength(60)]
        [Required(ErrorMessage="An Album Title is Required")]
        public string Title { get; set; }

        [Required(ErrorMessage="Price is Required")]
        [Range(0.01,100.00,ErrorMessage="Price must be between 0.01 and 100.0")]
        public decimal Price { get; set; }
        
        [StringLength(1024)]
        [DisplayName("Album Art Url")]
        public string AlbumArtUrl { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }

    }
}