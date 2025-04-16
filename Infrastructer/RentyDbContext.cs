using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Renty.Server.Infrastructer.Model;

namespace Renty.Server.Infrastructer
{
    public class RentyDbContext : IdentityDbContext<Users>
    {
        //public DbSet<Users> Users { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<ItemImages> ItemImages { get; set; }
        public DbSet<Categorys> Categorys { get; set; }
        public DbSet<ChatRooms> ChatRooms { get; set; }
        public DbSet<ChatMessages> ChatMessages { get; set; }
        public DbSet<TradeOffers> TradeOffers { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<WishList> WishLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Users>()
                .Property(u => u.Provider)
                .HasConversion<string>();

            modelBuilder.Entity<Users>()
                .Property(u => u.State)
                .HasConversion<string>();

            modelBuilder.Entity<Items>()
                .Property(i => i.UnitOfTime)
                .HasConversion<string>();

            modelBuilder.Entity<Items>()
                .Property(i => i.State)
                .HasConversion<string>();

            modelBuilder.Entity<Items>()
                .HasIndex(i => i.SellerId);

            modelBuilder.Entity<Items>()
                .HasOne(i => i.Seller)
                .WithMany(u => u.Items)
                .HasForeignKey(i => i.SellerId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(true);

            modelBuilder.Entity<ItemImages>()
                .HasIndex(ii => ii.ItemId);

            modelBuilder.Entity<ItemImages>()
                .HasOne(ii => ii.Item)
                .WithMany(i => i.ItemImages)
                .HasForeignKey(ii => ii.ItemId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            modelBuilder.Entity<Categorys>()
                .HasIndex(c => c.Name);

            modelBuilder.Entity<Categorys>()
                .Property(c => c.Name)
                .HasConversion<string>();

            modelBuilder.Entity<Categorys>()
                .HasMany(c => c.Items)
                .WithMany(i => i.Categories)
                .UsingEntity(j => j.ToTable("item_categorys"));

            modelBuilder.Entity<Categorys>().HasData(
                Enum.GetValues(typeof(CategoryType)).Cast<CategoryType>().Select(c =>
                    new Categorys() { Id = (int)c, Name = c}
                )
            );

            modelBuilder.Entity<ChatRooms>()
                .HasIndex(c => c.ItemId);

            modelBuilder.Entity<ChatRooms>()
                .HasIndex(c => c.SellerId);

            modelBuilder.Entity<ChatRooms>()
                .HasIndex(c => c.BuyerId);

            modelBuilder.Entity<ChatRooms>()
                .HasOne(c => c.Item)
                .WithMany(i => i.Chats)
                .HasForeignKey(c => c.ItemId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            modelBuilder.Entity<ChatRooms>()
                .HasOne(c => c.Seller)
                .WithMany(u => u.SellerChats)
                .HasForeignKey(c => c.SellerId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(true);

            modelBuilder.Entity<ChatRooms>()
                .HasOne(c => c.Buyer)
                .WithMany(u => u.BuyerChats)
                .HasForeignKey(c => c.BuyerId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(true);

            modelBuilder.Entity<ChatRooms>()
                .HasOne(c => c.LastMessage)
                .WithOne()
                .HasForeignKey<ChatRooms>(c => c.LastMessageId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            modelBuilder.Entity<ChatMessages>()
                .HasIndex(cm => cm.ChatRoomId);

            modelBuilder.Entity<ChatMessages>()
                .HasIndex(cm => cm.SenderId);

            modelBuilder.Entity<ChatMessages>()
                .HasIndex(cm => cm.ReceiverId);

            modelBuilder.Entity<ChatMessages>()
                .HasOne(cm => cm.ChatRoom)
                .WithMany(c => c.Messages)
                .HasForeignKey(cm => cm.ChatRoomId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(true);

            modelBuilder.Entity<ChatMessages>()
                .HasOne(cm => cm.Sender)
                .WithMany()
                .HasForeignKey(cm => cm.SenderId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(true);

            modelBuilder.Entity<ChatMessages>()
                .HasOne(cm => cm.Receiver)
                .WithMany()
                .HasForeignKey(cm => cm.ReceiverId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(true);

            modelBuilder.Entity<ChatMessages>()
                .Property(cm => cm.Type)
                .HasConversion<string>();

            modelBuilder.Entity<TradeOffers>()
                .HasIndex(to => to.ItemId);

            modelBuilder.Entity<TradeOffers>()
                .HasIndex(to => to.BuyerId);

            modelBuilder.Entity<TradeOffers>()
                .HasOne(to => to.Item)
                .WithMany(i => i.TradeOffers)
                .HasForeignKey(to => to.ItemId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            modelBuilder.Entity<TradeOffers>()
                .HasOne(to => to.Buyer)
                .WithMany(u => u.ProspectiveRentalList)
                .HasForeignKey(to => to.BuyerId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(true);

            modelBuilder.Entity<TradeOffers>()
                .Property(to => to.State)
                .HasConversion<string>();

            modelBuilder.Entity<Transactions>()
                .HasIndex(t => t.ItemId);

            modelBuilder.Entity<Transactions>()
                .HasIndex(t => t.BuyerId);

            modelBuilder.Entity<Transactions>()
                .Property(t => t.State)
                .HasConversion<string>();

            modelBuilder.Entity<Transactions>()
                .HasOne(t => t.Item)
                .WithMany(i => i.Transactions)
                .HasForeignKey(t => t.ItemId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            modelBuilder.Entity<Transactions>()
                .HasOne(t => t.Buyer)
                .WithMany(u => u.RentalHistory)
                .HasForeignKey(t => t.BuyerId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(true);

            modelBuilder.Entity<WishList>()
                .HasIndex(w => w.UserId);

            modelBuilder.Entity<WishList>()
                .HasOne(w => w.User)
                .WithMany(u => u.WishLists)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            modelBuilder.Entity<WishList>()
                .HasOne(w => w.Item)
                .WithMany()
                .HasForeignKey(w => w.ItemId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // databse.db에는 db파일을 저장할 경로를 적는다.
            optionsBuilder.UseSqlite("Data Source=/app/database/database.db");
        }
    }
}
