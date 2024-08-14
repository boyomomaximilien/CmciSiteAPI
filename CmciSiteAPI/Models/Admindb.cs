using System.ComponentModel.DataAnnotations;

namespace CmciSiteAPI.Models
{
    public class Admindb
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Pwd { get; set; }
    }
}
