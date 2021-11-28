namespace DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RiscoContext : DbContext
    {
        public RiscoContext()
            : base("name=RiscoContextQA")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<RiscoContext, DAL.Migrations.Configuration>());
            Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<PaymentCard> PaymentCards { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        public virtual DbSet<Favourite> Favourites { get; set; }
        public virtual DbSet<ForgotPasswordToken> ForgotPasswordTokens { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRatings> UserRatings { get; set; }
        public virtual DbSet<AppRatings> AppRatings { get; set; }
        public virtual DbSet<UserAddress> UserAddresses { get; set; }
        public virtual DbSet<ContactUs> ContactUs { get; set; }
        public virtual DbSet<UserDevice> UserDevices { get; set; }
        public virtual DbSet<RefreshTokens> RefreshTokens { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<UserSubscriptions> UserSubscriptions { get; set; }
        public virtual DbSet<AdminNotifications> AdminNotifications { get; set; }

        public virtual DbSet<Banner_Images> BannerImages { get; set; }
        public virtual DbSet<AdminTokens> AdminTokens { get; set; }        
        public virtual DbSet<VerifyNumberCodes> VerifyNumberCodes { get; set; }
        public virtual DbSet<Post> Posts{ get; set; }
        public virtual DbSet<Media> Medias { get; set; }
        public virtual DbSet<Interest> Interests { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Share> Shares { get; set; }
        public virtual DbSet<TrendLog> TrendLogs { get; set; }
        public virtual DbSet<HidePost> HidePosts { get; set; }
        public virtual DbSet<HideAllPost> HideAllPosts { get; set; }
        public virtual DbSet<FollowFollower> FollowFollowers { get; set; }
        public virtual DbSet<TurnOffNotification> TurnOffNotifications { get; set; }
        public virtual DbSet<ReportPost> ReportPosts { get; set; }
        public virtual DbSet<TopFollowerLog> TopFollowerLogs { get; set; }
        public virtual DbSet<ResetPasswordCode> ResetPasswordCode  { get; set; }
        public virtual DbSet<FeelingActivities> FeelingActivity { get; set; }
        public virtual DbSet<CheckIn> CheckIn { get; set; }
        public virtual DbSet<Friends> Friends { get; set; }

        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
              .HasMany(x => x.Requester)
              .WithRequired(x => x.Requester)
              .HasForeignKey(x => x.Requester_Id)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
              .HasMany(x => x.Addressee)
              .WithRequired(x => x.Addressee)
              .HasForeignKey(x => x.Addressee_Id)
              .WillCascadeOnDelete(false);


            modelBuilder.Entity<Post>()
              .HasMany(x => x.CheckIn)
              .WithOptional(x => x.Post)
              .HasForeignKey(x => x.Post_Id)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
              .HasMany(x => x.FeelingActivity)
              .WithOptional(x => x.Post)
              .HasForeignKey(x => x.Post_Id)
              .WillCascadeOnDelete(false);



            modelBuilder.Entity<User>()
              .HasMany(x => x.ResetPasswordCode)
              .WithRequired(x => x.User)
              .HasForeignKey(x => x.User_Id)
              .WillCascadeOnDelete(false);


            modelBuilder.Entity<Post>()
              .HasMany(x => x.FriendTagInPost)
              .WithRequired(x => x.Post)
              .HasForeignKey(x => x.Post_Id)
              .WillCascadeOnDelete(false);


            modelBuilder.Entity<User>()
              .HasMany(x => x.FriendTagInPost)
              .WithRequired(x => x.User)
              .HasForeignKey(x => x.User_Id)
              .WillCascadeOnDelete(false);


            modelBuilder.Entity<User>()
              .HasMany(x => x.VerifyNumberCodes)
              .WithRequired(x => x.User)
              .HasForeignKey(x => x.User_Id)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Admin>()
                .HasMany(x => x.AdminTokens)
                .WithRequired(x => x.Admin)
                .HasForeignKey(x => x.Admin_Id)
                .WillCascadeOnDelete(false);



            modelBuilder.Entity<AdminNotifications>()
               .HasMany(e => e.Notifications)
               .WithOptional(e => e.AdminNotification)
               .HasForeignKey(e => e.AdminNotification_Id)
               .WillCascadeOnDelete(false);




            modelBuilder.Entity<User>()
                .HasMany(x => x.UserSubscriptions)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.User_Id)
                .WillCascadeOnDelete(false);

  
                
            //modelBuilder.Entity<Order_Items>

            //modelBuilder.Entity<Store>()
            //    .HasMany(e => e.StoreDeliveryHours)
            //    .WithRequired(e => e.Store)
            //    .HasForeignKey(e => e.Store_Id)
            //    .WillCascadeOnDelete(false);


            //modelBuilder.Entity<StoreDeliveryHours>()
            //    .HasRequired(s => s.Store)
            //    .WithOptional(ad => ad.StoreDeliveryHours);



            modelBuilder.Entity<User>()
                .HasMany(e => e.UserDevices)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserRatings)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserAddresses)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
              .HasMany(e => e.AppRatings)
              .WithRequired(e => e.User)
              .HasForeignKey(e => e.User_ID)
              .WillCascadeOnDelete(false);


            //modelBuilder.Entity<Category>()
            //    .HasMany(e => e.SubCategories)
            //    .WithRequired(e => e.Category)
            //    .HasForeignKey(e => e.Category_Id)
            //    .WillCascadeOnDelete(false);


            
            //modelBuilder.Entity<Package>()
            //    .HasMany(e => e.Offer_Packages)
            //    .WithRequired(e => e.Package)
            //    .HasForeignKey(e => e.Package_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Package>()
            //   .HasMany(e => e.Order_Items)
            //   .WithOptional(e => e.Package)
            //   .HasForeignKey(e => e.Package_Id)
            //   .WillCascadeOnDelete(false);


            //modelBuilder.Entity<OrderPayment>()
            //   .HasMany(e => e.Orders)
            //   .WithRequired(e => e.OrderPayment)
            //   .HasForeignKey(e => e.OrderPayments_Id)


            //modelBuilder.Entity<Package>()
            //    .HasMany(e => e.Offer_Products)
            //    .WithRequired(e => e.Package)
            //    .HasForeignKey(e => e.Package_Id)
            //    .WillCascadeOnDelete(false);

           
            //modelBuilder.Entity<Product>()
            //    .HasMany(e => e.Order_Items)
            //    .WithOptional(e => e.Product)
            //    .HasForeignKey(e => e.Product_Id)
            //    .WillCascadeOnDelete(false);

             //modelBuilder.Entity<Store>()
            //    .HasMany(e => e.DeliveryMen)
            //    .WithRequired(e => e.Store)
            //    .HasForeignKey(e => e.Store_Id)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Notifications)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.User_ID)
                .WillCascadeOnDelete(false);



            //modelBuilder.Entity<Store>()
            //    .HasMany(e => e.SubCategories)
            //    .WithRequired(e => e.Store)
            //    .HasForeignKey(e => e.Store_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<SubCategory>()
            //    .HasMany(e => e.Products)
            //    .WithRequired(e => e.SubCategory)
            //    .HasForeignKey(e => e.SubCategory_Id)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<User>()
            //    .Property(e => e.FirstName)
            //    .IsUnicode(false);

            //modelBuilder.Entity<User>()
            //    .Property(e => e.LastName)
            //    .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.PaymentCards)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Favourites)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ForgotPasswordTokens)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_ID)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<User>()
                .HasMany(e => e.Posts)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserGroups)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Likes)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Shares)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.TrendLogs)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.FirstUserHidePosts)
                .WithRequired(e => e.FirstUser)
                .HasForeignKey(e => e.FirstUser_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.SecondUserHidePosts)
                .WithRequired(e => e.SecondUser)
                .HasForeignKey(e => e.SecondUser_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.FirstUserHideAllPosts)
                .WithRequired(e => e.FirstUserAll)
                .HasForeignKey(e => e.FirstUserAll_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.SecondUserHideAllPosts)
                .WithRequired(e => e.SecondUserAll)
                .HasForeignKey(e => e.SecondUserAll_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.FirstUserFollowFollower)
                .WithRequired(e => e.FirstUser)
                .HasForeignKey(e => e.FirstUser_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.SecondUserFollowFollower)
                .WithRequired(e => e.SecondUser)
                .HasForeignKey(e => e.SecondUser_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.TurnOffNotifications)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ReportPosts)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.FirstUserTopFollowerLog)
                .WithRequired(e => e.FirstUser)
                .HasForeignKey(e => e.FirstUser_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.SecondUserTopFollowerLog)
                .WithRequired(e => e.SecondUser)
                .HasForeignKey(e => e.SecondUser_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Medias)
                .WithRequired(e => e.Post)
                .HasForeignKey(e => e.Post_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Likes)
                .WithOptional(e => e.Post)
                .HasForeignKey(e => e.Post_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.Post)
                .HasForeignKey(e => e.Post_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Shares)
                .WithRequired(e => e.Post)
                .HasForeignKey(e => e.Post_Id)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Post>()
            //    .HasMany(e => e.TrendLogs)
            //    .WithOptional(e => e.Post)
            //    .HasForeignKey(e => e.Post_Id)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.HidePosts)
                .WithRequired(e => e.Post)
                .HasForeignKey(e => e.Post_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.TurnOffNotifications)
                .WithRequired(e => e.Post)
                .HasForeignKey(e => e.Post_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.ReportPosts)
                .WithRequired(e => e.Post)
                .HasForeignKey(e => e.Post_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Comment>()
                .HasMany(e => e.Likes)
                .WithOptional(e => e.Comment)
                .HasForeignKey(e => e.Comment_Id)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Comment>()
            //    .HasMany(e => e.TrendLogs)
            //    .WithOptional(e => e.Comment)
            //    .HasForeignKey(e => e.Comment_Id)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserGroup>()
                .HasMany(e => e.Posts)
                .WithOptional(e => e.UserGroup)
                .HasForeignKey(e => e.UserGroup_Id)
                .WillCascadeOnDelete(false);
        }
    }
}
