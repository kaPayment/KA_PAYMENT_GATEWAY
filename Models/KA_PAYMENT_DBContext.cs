using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace KA_PAYMENT_GATEWAY.Models
{
    public partial class KA_PAYMENT_DBContext : DbContext
    {
       
        public KA_PAYMENT_DBContext()
        {
        }

        public KA_PAYMENT_DBContext(DbContextOptions<KA_PAYMENT_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Merchant> Merchants { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=SQL5070.site4now.net;Initial Catalog=db_a999a5_kapaymentdb;User Id=db_a999a5_kapaymentdb_admin;Password=As123456!");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Arabic_CI_AS");

            modelBuilder.Entity<Merchant>(entity =>
            {
                entity.ToTable("MERCHANTS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasColumnName("ACTIVE");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.Balance).HasColumnName("BALANCE");

                entity.Property(e => e.BusType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BUS_TYPE");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_AT");

                entity.Property(e => e.CreattedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATTED_BY");

                entity.Property(e => e.DeleteAt)
                    .HasColumnType("datetime")
                    .HasColumnName("DELETE_AT");

                entity.Property(e => e.DeleteBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DELETE_BY");

                entity.Property(e => e.Deleted).HasColumnName("DELETED");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PHONE");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Merchants)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MERCHANTS_USERS");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("PAYMENTS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CURRENCY");

                entity.Property(e => e.Describtion)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("DESCRIBTION");

                entity.Property(e => e.Done).HasColumnName("DONE");

                entity.Property(e => e.FeeAmount).HasColumnName("FEE_AMOUNT");

                entity.Property(e => e.MerchantId).HasColumnName("MERCHANT_ID");

                entity.Property(e => e.NetAmount).HasColumnName("NET_AMOUNT");

                entity.Property(e => e.OrderNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_NO");

                entity.Property(e => e.Provider)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PROVIDER");

                entity.Property(e => e.TaxAmount).HasColumnName("TAX_AMOUNT");

                entity.Property(e => e.TotalAmount).HasColumnName("TOTAL_AMOUNT");

                entity.Property(e => e.TransactionId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TRANSACTION_ID");

                entity.HasOne(d => d.Merchant)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.MerchantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAYMENTS_MERCHANTS");
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.ToTable("TOKENS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientToken)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("CLIENT_TOKEN");

                entity.Property(e => e.CurrentUrl)
                    .IsUnicode(false)
                    .HasColumnName("CURRENT_URL");

                entity.Property(e => e.ExpiredAt)
                    .HasColumnType("datetime")
                    .HasColumnName("EXPIRED_AT");

                entity.Property(e => e.MerchantId).HasColumnName("MERCHANT_ID");

                entity.Property(e => e.PaymentId).HasColumnName("PAYMENT_ID");

                entity.HasOne(d => d.Merchant)
                    .WithMany(p => p.Tokens)
                    .HasForeignKey(d => d.MerchantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TOKENS_MERCHANTS");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USERS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasColumnName("ACTIVE");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATED_AT");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.Deleted).HasColumnName("DELETED");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("DELETED_AT");

                entity.Property(e => e.DeletedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DELETED_BY");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.SecurityStamp)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("SECURITY_STAMP");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("USER_NAME");

                entity.Property(e => e.UserType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("USER_TYPE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
