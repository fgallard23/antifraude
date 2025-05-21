using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities
{
    [Table("Journal")]
    public class JournalEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string sourceAccountId { get; set; }
        public string targetAccountId { get; set; }
        public int transferTypeId { get; set; }
        public double value { get; set; }
        public string status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
