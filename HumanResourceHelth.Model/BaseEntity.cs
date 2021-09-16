using System.ComponentModel.DataAnnotations;
namespace HumanResourceHelth.Model
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
