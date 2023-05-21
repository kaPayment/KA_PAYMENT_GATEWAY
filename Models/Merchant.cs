using System;
using System.Collections.Generic;

#nullable disable

namespace KA_PAYMENT_GATEWAY.Models
{
    public partial class Merchant
    {
        public Merchant()
        {
            Payments = new HashSet<Payment>();
            Tokens = new HashSet<Token>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string BusType { get; set; }
        public bool Active { get; set; }
        public string CreattedBy { get; set; }
        public string CreatedAt { get; set; }
        public bool Deleted { get; set; }
        public string DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }
        public int UserId { get; set; }
        public float Balance { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
    }
}
