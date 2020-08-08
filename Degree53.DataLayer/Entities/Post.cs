using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Degree53.DataLayer.Entities
{

    [Table("Post", Schema = "Degree53")]
    public class Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public PostDetail PostDetail { get; set; }
    }
}
