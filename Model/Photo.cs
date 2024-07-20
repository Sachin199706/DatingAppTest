using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.Model
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public required string URL { get; set; }
        public bool IsMain {  get; set; }
        public string ? PublicID { get; set; }
        //navigative
       
        public int UserDetailsId { get; set; } = 0;
        public UserDetails UserDetails { get; set; } = null!;

    }
}