using System;
using System.Collections.Generic;

#nullable disable

namespace KA_PAYMENT_GATEWAY.Models
{
    public partial class User
    {
        public User()
        {
            Merchants = new HashSet<Merchant>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public bool Active { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Deleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string SecurityStamp { get; set; }

        public virtual ICollection<Merchant> Merchants { get; set; }
    }
}
