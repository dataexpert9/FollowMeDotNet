namespace DAL
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Post
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Post()
        {
            Medias = new HashSet<Media>();
            Likes = new HashSet<Like>();
            Comments = new HashSet<Comment>();
            Shares = new HashSet<Share>();
            //TrendLogs = new HashSet<TrendLog>();
            HidePosts = new HashSet<HidePost>();
            TurnOffNotifications = new HashSet<TurnOffNotification>();
            ReportPosts = new HashSet<ReportPost>();
            FriendTagInPost = new HashSet<FriendTagInPost>();
            FeelingActivity = new HashSet<FeelingActivities>();
        }

        public int Id { get; set; }

        public string Text { get; set; }

        public int Visibility { get; set; }

        public int RiskLevel { get; set; }

        public string Location { get; set; }

        public int User_Id { get; set; }

        public virtual User User { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public int? UserGroup_Id { get; set; }

        public virtual UserGroup UserGroup { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Media> Medias { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Like> Likes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Share> Shares { get; set; }

        //[JsonIgnore]
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<TrendLog> TrendLogs { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HidePost> HidePosts { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TurnOffNotification> TurnOffNotifications { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReportPost> ReportPosts { get; set; }


        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FriendTagInPost> FriendTagInPost { get; set; }

        public virtual ICollection<FeelingActivities> FeelingActivity { get; set; }
        public virtual ICollection<CheckIn> CheckIn { get; set; }


        [NotMapped]
        public bool IsLiked { get; set; }

        [NotMapped]
        public int LikesCount { get; set; }

        [NotMapped]
        public int CommentsCount { get; set; }

        [NotMapped]
        public int ShareCount { get; set; }
        [NotMapped]
        public bool IsUserFollow { get; set; }
    }
}
