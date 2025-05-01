using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Renty.Server.Global;
using Renty.Server.Infrastructer.Model;

namespace Renty.Server.Infrastructer
{
    public class RentyDbContext(IOptions<Settings> settings) : IdentityDbContext<Users>
    {
        //public DbSet<Users> Users { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<ItemImages> ItemImages { get; set; }
        public DbSet<Categorys> Categorys { get; set; }
        public DbSet<ChatRooms> ChatRooms { get; set; }
        public DbSet<ChatPlayers> ChatPlayers { get; set; }
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
                .Property(i => i.PriceUnit)
                .HasConversion<string>();

            modelBuilder.Entity<Items>()
                .Property(i => i.State)
                .HasConversion<string>();

            modelBuilder.Entity<Items>()
                .HasIndex(i => i.SellerId);

            modelBuilder.Entity<Items>()
                .HasIndex(i => new { i.CreatedAt, i.Id })
                .IsDescending(true, true);

            modelBuilder.Entity<Items>()
                .HasQueryFilter(item => item.DeletedAt == null);

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

            modelBuilder.Entity<Categorys>().HasData
            (
                Enum.GetValues(typeof(CategoryType)).Cast<CategoryType>().Select
                (
                    c => new Categorys() { Id = (int)c, Name = c}
                )
            );

            modelBuilder.Entity<ChatRooms>()
                .HasIndex(c => c.ItemId);

            modelBuilder.Entity<ChatRooms>()
                .HasQueryFilter(room => room.DeletedAt == null);

            modelBuilder.Entity<ChatRooms>()
                .HasOne(c => c.Item)
                .WithMany(i => i.Chats)
                .HasForeignKey(c => c.ItemId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            modelBuilder.Entity<ChatRooms>()
                .HasOne(c => c.LastMessage)
                .WithOne()
                .HasForeignKey<ChatRooms>(c => c.LastMessageId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            modelBuilder.Entity<ChatPlayers>()
                .HasIndex(cp => new { cp.ChatRoomId, cp.UserId });

            modelBuilder.Entity<ChatPlayers>()
                .HasIndex(cp => cp.UserId);

            modelBuilder.Entity<ChatPlayers>()
                .HasOne(cp => cp.ChatRoom)
                .WithMany(c => c.Players)
                .HasForeignKey(cp => cp.ChatRoomId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            modelBuilder.Entity<ChatPlayers>()
                .HasOne(cp => cp.User)
                .WithMany(u => u.ChatPlayers)
                .HasForeignKey(cp => cp.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            modelBuilder.Entity<ChatMessages>()
                .HasIndex(cm => cm.ChatRoomId);

            modelBuilder.Entity<ChatMessages>()
                .HasIndex(cm => cm.SenderId);

            modelBuilder.Entity<ChatMessages>()
                .HasOne(cm => cm.ChatRoom)
                .WithMany(c => c.Messages)
                .HasForeignKey(cm => cm.ChatRoomId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            modelBuilder.Entity<ChatMessages>()
                .HasOne(cm => cm.Sender)
                .WithMany(cp => cp.Messages)
                .HasForeignKey(cm => cm.SenderId)
                .OnDelete(DeleteBehavior.Cascade)
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
            var dataStorage = settings.Value.DataStorage;
            optionsBuilder.UseSqlite($"Data Source={dataStorage}/database.db");
        }
    }
}
