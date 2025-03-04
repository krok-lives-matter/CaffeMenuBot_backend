using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CaffeMenuBot.Data.Models.Menu
{
    [Table("categories", Schema = CaffeMenuBotContext.SchemaName)]
    public class Category
    {
        [Key, Required, Column("category_id", TypeName = "integer")]
        public int Id { get; set; }

        [Required, Column("category_name", TypeName = "text")]
        public string CategoryName { get; set; } = null!;

        [JsonIgnore]
        [Column("cover_photo_filename", TypeName = "text")]
        public string? CoverPhotoFileName { get; set; }

        [NotMapped]
        public string? CoverPhotoUrl { get; set; }

        [Required, Column("is_visible", TypeName = "boolean")]
        public bool IsVisible { get; set; } = true;

        [JsonIgnore]
        public List<Dish> Dishes { get; set; } = new List<Dish>();
    }
}