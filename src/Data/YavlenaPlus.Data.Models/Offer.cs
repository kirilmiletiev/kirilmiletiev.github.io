using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using YavlenaPlus.Data.Models.Common;

namespace YavlenaPlus.Data.Models
{
    public class Offer : BaseModel<int>
    {
        public Offer()
        {
            this.IsNew = true;
            this.Favorites = new HashSet<Favorite>();
            this.Comments = new HashSet<Comment>();
            // this.Users = new HashSet<YavlenaPlusUser>();

        }
        public Offer(int price, string type, string location, int size, string phoneNumber)
            : this()
        {
            this.Price = price;
            this.Type = type;
            this.Location = location;
            this.Size = size;
            this.PhoneOfOwner = phoneNumber;
           // this.Picture = pictureUrl;
            this.TimeOfCreation = DateTime.Now;
            this.IsActual = true;
        }

        public Offer(int price, string type, string location, int size, string phoneNumber, string pictureUrl, string description, string link)
            : this(price, type, location, size, phoneNumber)
        {
            this.ShortDescription = description;
            this.Link = link;
            this.Picture = pictureUrl;
        }

        [Required]
        [Range(0, 10000000)]
        [Column(TypeName = "decimal(18,0)")]
        public decimal Price { get; set; }

        [Required]
        [MinLength(3)]
        public string Type { get; set; }

        [Required]
        [MinLength(3)]
        public string Location { get; set; }

        [Required]
        [Range(1, 10000000)]
        public int Size { get; set; }

        [Range(-5, 100)]
        public int? Floor { get; set; }

        [Range(0, 200)]
        public int? TotalFloors { get; set; }

        public int? YearOfBuild { get; set; }

        public string ShortDescription { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneOfOwner { get; set; }

        public string Picture { get; set; }

        public bool IsNew { get; set; } //TODO:  <НОВА ОФЕРТА>   *тага в нова обява* 

        public bool IsActual { get; set; }

        public string Link { get; set; }

        public DateTime TimeOfCreation { get; set; }

        public YavlenaPlusUser YavlenaPlusUser { get; set; }

        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Comment> Comments{ get; set; }
    }
}