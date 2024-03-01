using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminar_5.Models
{
    public class ChatContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public ChatContext(){}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.
            UseNpgsql("Host = localhost;Username=postgres;" +
                "Password=example;Database = Seminar5").
            LogTo(Console.WriteLine);
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(user => user.Id);
                entity.ToTable("users");
                entity.Property(user => user.Id)
                        .HasColumnName("id");
                entity.Property(user => user.UserName)
                        .HasColumnName("name")
                        .HasMaxLength(50);

                entity.HasMany(user => user.RecievedMessages)
                        .WithOne(message => message.Consumer)
                        .HasForeignKey(message => message.ConsumerId)
                        .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(message => message.Id);
                entity.Property(message => message.Content)
                        .HasColumnName("text");

                entity.HasOne(message => message.Author)
                          .WithMany(user => user.SendedMessages)
                          .HasForeignKey(message => message.AuthorId)
                          .HasConstraintName("messages_from_user_id_fkey_author_id");
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
