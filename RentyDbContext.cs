using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Renty.Server.Auth.Domain;
using Renty.Server.Chat.Domain;
using Renty.Server.Global;
using Renty.Server.My.Domain;
using Renty.Server.Post.Domain;
using Renty.Server.Product.Domain;
using Renty.Server.Review.Domain;
using Renty.Server.Transaction.Domain;

namespace Renty.Server
{
    public class RentyDbContext(IOptions<Settings> settings) : IdentityDbContext<Users>
    {
        public DbSet<Items> Items { get; set; }
        public DbSet<ItemImages> ItemImages { get; set; }
        public DbSet<Categorys> Categorys { get; set; }
        public DbSet<ChatRooms> ChatRooms { get; set; }
        public DbSet<ChatUsers> ChatUsers { get; set; }
        public DbSet<ChatMessages> ChatMessages { get; set; }
        public DbSet<TradeOffers> TradeOffers { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<BuyerPosts> BuyerPosts { get; set; }
        public DbSet<BuyerPostComments> BuyerComments { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<ReviewImages> ReviewImages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Users>()
                .Property(u => u.Provider)
                .HasConversion<string>();

            modelBuilder.Entity<Users>()
                .Property(u => u.State)
                .HasConversion<string>();

            modelBuilder.Entity<Users>()
                .HasQueryFilter(user => user.DeletedAt == null);

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
                .HasIndex(c => c.UpdatedAt);

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

            modelBuilder.Entity<ChatUsers>()
                .HasIndex(u => new { u.ChatRoomId, u.UserId });

            modelBuilder.Entity<ChatUsers>()
                .HasIndex(u => u.UserId);

            modelBuilder.Entity<ChatUsers>()
                .HasOne(c => c.ChatRoom)
                .WithMany(cr => cr.ChatUsers)
                .HasForeignKey(c => c.ChatRoomId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            modelBuilder.Entity<ChatUsers>()
                .HasOne(c => c.User)
                .WithMany(u => u.ChatUsers)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            modelBuilder.Entity<ChatMessages>()
                .HasIndex(cm => new { cm.ChatRoomId, cm.CreatedAt })
                .IsDescending(false, true);

            modelBuilder.Entity<ChatMessages>()
                .HasIndex(cm => cm.SenderId);

            modelBuilder.Entity<ChatMessages>()
                .Property(cm => cm.Type)
                .HasConversion<string>();

            modelBuilder.Entity<ChatMessages>()
                .HasOne(cm => cm.ChatRoom)
                .WithMany(c => c.Messages)
                .HasForeignKey(cm => cm.ChatRoomId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            modelBuilder.Entity<ChatMessages>()
                .HasOne(cm => cm.Sender)
                .WithMany(u => u.Messages)
                .HasForeignKey(cm => cm.SenderId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            modelBuilder.Entity<TradeOffers>()
                .HasIndex(to => new { to.ItemId, to.BuyerId });

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

            modelBuilder.Entity<TradeOffers>()
                .Property(i => i.PriceUnit)
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
                .HasIndex(w => new { w.UserId, w.ItemId });

            modelBuilder.Entity<WishList>()
                .HasIndex(w => w.ItemId);

            modelBuilder.Entity<WishList>()
                .HasOne(w => w.User)
                .WithMany(u => u.WishLists)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            modelBuilder.Entity<WishList>()
                .HasOne(w => w.Item)
                .WithMany(i => i.WishLists)
                .HasForeignKey(w => w.ItemId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(true);

            // BuyerPost 엔터티 설정
            modelBuilder.Entity<BuyerPosts>(entity =>
            {
                // 속성 설정
                entity.Property(bp => bp.Title)
                      .HasMaxLength(255);

                entity.Property(bp => bp.Description)
                      .HasMaxLength(1000);

                // 쿼리 필터 설정
                entity.HasQueryFilter(bp => bp.DeletedAt == null);

                // 인덱스 설정
                entity.HasIndex(bp => new { bp.CreatedAt, bp.CategoryId });

                // 관계 설정: BuyerPost -> User (다대일)
                // BuyerPost는 하나의 User(BuyerUser)를 가짐
                // User는 여러 BuyerPost를 가질 수 있음
                entity.HasOne(bp => bp.BuyerUser)          // BuyerPost 엔터티의 BuyerUser 네비게이션 속성
                      .WithMany(u => u.BuyerPosts)        // User 엔터티의 BuyerPosts 컬렉션 네비게이션 속성
                      .HasForeignKey(bp => bp.BuyerUserId) // BuyerPost 엔터티의 외래 키
                      .OnDelete(DeleteBehavior.Cascade) // 사용자가 삭제될 때 관련된 게시글이 있으면 삭제 방지 (요구사항에 따라 Cascade, SetNull 등으로 변경 가능)
                      .IsRequired(true);

                entity.HasOne(bp => bp.Category)
                        .WithMany(c => c.BuyerPosts)
                        .HasForeignKey(bp => bp.CategoryId)
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired(false);
            });

            // BuyerPostComment 엔터티 설정
            modelBuilder.Entity<BuyerPostComments>(entity =>
            {
                entity.HasOne(bc => bc.Item)
                      .WithMany(i => i.BuyerPostComments)
                      .HasForeignKey(bc => bc.ItemId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .IsRequired(false);

                entity.HasOne(bc => bc.BuyerPost)
                      .WithMany(bp => bp.Comments)
                      .HasForeignKey(bc => bc.BuyerPostId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired(true);

                entity.HasOne(bc => bc.User)
                      .WithMany(u => u.BuyerPostComments)
                      .HasForeignKey(bc => bc.UserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired(true);
            });

            modelBuilder.Entity<Reviews>(entity =>
            {
                entity.Property(r => r.Content)
                      .HasMaxLength(1000);
                
                entity.Property(r => r.SellerEvaluation)
                      .HasConversion<string>();

                entity.HasOne(r => r.Reviewer)
                      .WithMany(u => u.Reviews)
                      .HasForeignKey(r => r.ReviewerId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired(true);

                entity.HasOne(r => r.Reviewee)
                      .WithMany(u => u.Reviewees)
                      .HasForeignKey(r => r.RevieweeId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired(true);

                entity.HasOne(r => r.Item)
                      .WithMany(i => i.Reviews)
                      .HasForeignKey(r => r.ItemId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired(true);
            });

            modelBuilder.Entity<ReviewImages>(entity =>
            {
                entity.HasOne(ri => ri.Review)
                      .WithMany(r => r.Images)
                      .HasForeignKey(ri => ri.ReviewId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired(true);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dataStorage = settings.Value.DataStorage;
            optionsBuilder.UseSqlite($"Data Source={dataStorage}/database.db");
        }
    }
}
