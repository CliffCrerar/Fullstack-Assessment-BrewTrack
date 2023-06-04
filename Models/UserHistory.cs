using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewTrack.Models
{
    [Table("user_history")]
    public class UserHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key, Column("UserHistoryId")]
        public int Id { get; set; }
        [ForeignKey("UserId")] public Guid UserId { get; set; }

        public int LastPage { get; set; }
    }
}
