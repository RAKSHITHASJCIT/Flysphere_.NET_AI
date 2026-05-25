using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlysphereBackendDotnet.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("userid")]
        public string UserId { get; set; }

        [Required]
        [Column("firstname")]
        public string FirstName { get; set; }

        [Required]
        [Column("lastname")]
        public string LastName { get; set; }

        [Required]
        [Column("phone")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("password")]
        public string Password { get; set; }

        [Required]
        [Column("role")]
        public string Role { get; set; } // ADMIN or USER
    }
}
