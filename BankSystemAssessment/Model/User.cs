using System.ComponentModel.DataAnnotations.Schema;

namespace BankSystemAssessment.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
    }
}
