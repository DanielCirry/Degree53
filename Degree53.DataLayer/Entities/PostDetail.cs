using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Degree53.DataLayer.Entities
{
    [Table("PostDetail", Schema = "Degree53")]
    public class PostDetail
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTimeOffset CreationDate { get; set; }
        [Required]
        public int NumbersOfViews { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }

    }
}
