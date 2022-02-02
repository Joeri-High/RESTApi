using System.ComponentModel.DataAnnotations;

namespace RESTApi.Models
{
    public class Artikel
    {
        [Key]
        [MaxLength(13)]
        public string Code { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Uw kunt maximaal 50 tekens invoeren als naam!")]
        public string Naam { get; set; }
        [Required]
        [Range(1,999999, ErrorMessage = "Voer een potmaat in die groter is dan 0, en kleiner dan 999999!")]
        public int PotMaat { get; set; }
        [Required]
        [Range(1,999999, ErrorMessage = "Voer een planthoogte in die groter is dan 0, en kleiner dan 999999!")]
        public int PlantHoogte { get; set; }
        [EnumDataType(typeof(Kleur), ErrorMessage = "Deze kleur bestaat niet!")]
        public Kleur kleur { get; set; }
        [Required]
        [EnumDataType(typeof(ProductGroep), ErrorMessage = "Deze productgroep bestaat niet!")]
        public ProductGroep productGroep { get; set; }
    }
}