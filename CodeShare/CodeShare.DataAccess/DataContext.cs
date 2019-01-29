using CodeShare.Model;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;

namespace CodeShare.DataAccess
{
    public class DataContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Code> Codes { get; set; }
        public virtual DbSet<CodeLanguage> CodeLanguages { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<CodeFile> CodeFiles { get; set; }
        public virtual DbSet<Reply> Replies { get; set; }

        public DataContext() : base("CodeShare")
        {
            Debug.WriteLine($"Initializing context...");

            Configuration.ProxyCreationEnabled = false;

            Database.SetInitializer(new DataInitializer());

            Debug.WriteLine($"Connection configured with connection string: {Database.Connection.ConnectionString}.");
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Debug.WriteLine($"Creating model for Context...");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            #region Logs

            Debug.WriteLine($"Creating model for {typeof(UserLog).Name} class.");
            modelBuilder.Entity<UserLog>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(UserLog).Name}s");
            });
            modelBuilder.Entity<UserLog>().HasKey(e => e.Uid);
            modelBuilder.Entity<UserLog>()
                .HasRequired(c => c.User)
                .WithMany(u => u.Logs)
                .HasForeignKey(l => l.UserUid);

            Debug.WriteLine($"Creating model for {typeof(CodeLog).Name} class.");
            modelBuilder.Entity<CodeLog>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(CodeLog).Name}s");
            });
            modelBuilder.Entity<CodeLog>().HasKey(e => e.Uid);
            modelBuilder.Entity<CodeLog>()
                .HasRequired(c => c.Code)
                .WithMany(u => u.Logs)
                .HasForeignKey(l => l.CodeUid);

            Debug.WriteLine($"Creating model for {typeof(QuestionLog).Name} class.");
            modelBuilder.Entity<QuestionLog>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(QuestionLog).Name}s");
            });
            modelBuilder.Entity<QuestionLog>().HasKey(e => e.Uid);
            modelBuilder.Entity<QuestionLog>()
                .HasRequired(c => c.Question)
                .WithMany(u => u.Logs)
                .HasForeignKey(l => l.QuestionUid);

            Debug.WriteLine($"Creating model for {typeof(CommentLog).Name} class.");
            modelBuilder.Entity<CommentLog>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(CommentLog).Name}s");
            });
            modelBuilder.Entity<CommentLog>().HasKey(e => e.Uid);
            modelBuilder.Entity<CommentLog>()
                .HasRequired(c => c.Comment)
                .WithMany(u => u.Logs)
                .HasForeignKey(l => l.CommentUid);

            #endregion

            #region Videos

            Debug.WriteLine($"Creating model for {typeof(Video).Name} class.");
            modelBuilder.Entity<Video>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(Video).Name}s");
            });
            modelBuilder.Entity<Video>().HasKey(e => e.Uid);

            #endregion

            #region Images

            Debug.WriteLine($"Creating model for {typeof(ProfilePicture).Name} class.");
            modelBuilder.Entity<ProfilePicture>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(ProfilePicture).Name}s");
            });
            modelBuilder.Entity<ProfilePicture>().HasKey(e => e.Uid);
            modelBuilder.Types<ProfilePicture>()
                .Configure(ctc => ctc.Property(p => p.Crop.X).HasColumnName("Crop_X"));
            modelBuilder.Types<ProfilePicture>()
                .Configure(ctc => ctc.Property(p => p.Crop.Y).HasColumnName("Crop_Y"));
            modelBuilder.Types<ProfilePicture>()
                .Configure(ctc => ctc.Property(p => p.Crop.Width).HasColumnName("Crop_Width"));
            modelBuilder.Types<ProfilePicture>()
                .Configure(ctc => ctc.Property(p => p.Crop.Height).HasColumnName("Crop_Height"));
            modelBuilder.Types<ProfilePicture>()
                .Configure(ctc => ctc.Property(p => p.Crop.AspectRatio).HasColumnName("Crop_AspectRatio"));

            Debug.WriteLine($"Creating model for {typeof(Banner).Name} class.");
            modelBuilder.Entity<Banner>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(Banner).Name}s");
            });
            modelBuilder.Entity<Banner>().HasKey(e => e.Uid);
            modelBuilder.Types<Banner>()
                .Configure(ctc => ctc.Property(p => p.Crop.X).HasColumnName("Crop_X"));
            modelBuilder.Types<Banner>()
                .Configure(ctc => ctc.Property(p => p.Crop.Y).HasColumnName("Crop_Y"));
            modelBuilder.Types<Banner>()
                .Configure(ctc => ctc.Property(p => p.Crop.Width).HasColumnName("Crop_Width"));
            modelBuilder.Types<Banner>()
                .Configure(ctc => ctc.Property(p => p.Crop.Height).HasColumnName("Crop_Height"));
            modelBuilder.Types<Banner>()
                .Configure(ctc => ctc.Property(p => p.Crop.AspectRatio).HasColumnName("Crop_AspectRatio"));

            Debug.WriteLine($"Creating model for {typeof(Screenshot).Name} class.");
            modelBuilder.Entity<Screenshot>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(Screenshot).Name}s");
            });
            modelBuilder.Entity<Screenshot>().HasKey(e => e.Uid);

            #endregion

            #region Report

            Debug.WriteLine($"Creating model for {typeof(Report).Name} class.");
            modelBuilder.Entity<Report>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(Report).Name}s");
            });
            modelBuilder.Entity<Report>().HasKey(e => e.Uid);

            #endregion

            #region CodeLanguage

            Debug.WriteLine($"Creating model for {typeof(CodeLanguage).Name} class.");
            modelBuilder.Entity<CodeLanguage>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(CodeLanguage).Name}s");
            });
            modelBuilder.Entity<CodeLanguage>().HasKey(e => e.Uid);
            modelBuilder.Types<CodeLanguage>()
                .Configure(ctc => ctc.Property(c => c.Syntax.Delimiter).HasColumnName("Syntax_Delimiter"));
            modelBuilder.Types<CodeLanguage>()
                .Configure(ctc => ctc.Property(c => c.Syntax.Keywords).HasColumnName("Syntax_Keywords"));
            modelBuilder.Types<CodeLanguage>()
                .Configure(ctc => ctc.Property(c => c.Syntax.Comments).HasColumnName("Syntax_Comments"));

            #endregion

            #region Content

            Debug.WriteLine($"Creating model for {typeof(Content).Name} class.");
            modelBuilder.Entity<Content>().HasKey(e => e.Uid);
            modelBuilder.Entity<Content>()
                .HasMany(c => c.Banners)
                .WithRequired(c => c.Content)
                .HasForeignKey(c => c.ContentUid);
            modelBuilder.Entity<Content>()
                .HasMany(c => c.Screenshots)
                .WithRequired(c => c.Content)
                .HasForeignKey(c => c.ContentUid);
            modelBuilder.Entity<Content>()
                .HasMany(c => c.Replies)
                .WithOptional(c => c.Content)
                .HasForeignKey(c => c.ContentUid);
            modelBuilder.Entity<Content>()
                .HasMany(c => c.Videos)
                .WithRequired(c => c.Content)
                .HasForeignKey(c => c.ContentUid);
            modelBuilder.Entity<Content>()
                .HasMany(c => c.Ratings)
                .WithRequired(c => c.Content)
                .HasForeignKey(c => c.ContentUid);

            #endregion

            #region User

            Debug.WriteLine($"Creating model for {typeof(User).Name} class.");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>()
                .HasMany(u => u.Friends)
                .WithMany()
                .Map(mc =>
                {
                    mc.ToTable("Friendships");
                    mc.MapLeftKey("Friend_A");
                    mc.MapRightKey("Friend_B");
                });
            modelBuilder.Types<User>()
                .Configure(ctc => ctc.Property(u => u.Email.Address).HasColumnName("Email"));
            modelBuilder.Entity<User>()
                .HasMany(u => u.Codes)
                .WithRequired(c => c.User)
                .HasForeignKey(c => c.UserUid);
            modelBuilder.Entity<User>()
                .HasMany(c => c.ProfilePictures)
                .WithRequired(c => c.User)
                .HasForeignKey(c => c.UserUid);
            modelBuilder.Types<User>()
                .Configure(ctc => ctc.Property(u => u.Password.Iterations).HasColumnName("Iterations"));
            modelBuilder.Types<User>()
                .Configure(ctc => ctc.Property(u => u.Password.Salt).HasColumnName("Salt"));
            modelBuilder.Types<User>()
                .Configure(ctc => ctc.Property(u => u.Password.Hash).HasColumnName("Hash"));
            modelBuilder.Types<User>()
                .Configure(ctc => ctc.Property(u => u.Color.R).HasColumnName("Color_R"));
            modelBuilder.Types<User>()
                .Configure(ctc => ctc.Property(u => u.Color.G).HasColumnName("Color_G"));
            modelBuilder.Types<User>()
                .Configure(ctc => ctc.Property(u => u.Color.B).HasColumnName("Color_B"));
            modelBuilder.Types<User>()
                .Configure(ctc => ctc.Property(u => u.Color.A).HasColumnName("Color_A"));

            #endregion

            #region Code

            Debug.WriteLine($"Creating model for {typeof(Code).Name} class.");
            modelBuilder.Entity<Code>().ToTable("Codes");
            modelBuilder.Entity<Code>()
                .HasRequired(c => c.User)
                .WithMany(l => l.Codes)
                .HasForeignKey(c => c.UserUid);
            modelBuilder.Entity<Code>()
                .HasMany(c => c.Files)
                .WithRequired(c => c.Code)
                .HasForeignKey(l => l.CodeUid);

            Debug.WriteLine($"Creating model for {typeof(CodeFile).Name} class.");
            modelBuilder.Entity<CodeFile>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(CodeFile).Name}s");
            });
            modelBuilder.Entity<CodeFile>().HasKey(e => e.Uid);
            modelBuilder.Entity<CodeFile>()
                .HasRequired(c => c.CodeLanguage)
                .WithMany()
                .HasForeignKey(c => c.CodeLanguageUid);

            #endregion

            #region Comments

            Debug.WriteLine($"Creating model for {typeof(Comment).Name} class.");
            modelBuilder.Entity<Comment>().HasKey(e => e.Uid);
            modelBuilder.Entity<Comment>()
                .HasMany(e => e.Logs)
                .WithRequired(e => e.Comment)
                .HasForeignKey(e => e.CommentUid);
            modelBuilder.Entity<Comment>()
                .HasMany(e => e.Ratings)
                .WithRequired(e => e.Comment)
                .HasForeignKey(e => e.CommentUid);

            Debug.WriteLine($"Creating model for {typeof(Reply).Name} class.");
            modelBuilder.Entity<Reply>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"Replies");
            });
            modelBuilder.Entity<Reply>().HasKey(e => e.Uid);
            modelBuilder.Entity<Reply>()
                .HasMany(u => u.Replies)
                .WithMany()
                .Map(mc =>
                {
                    mc.ToTable("RepliesToReply");
                    mc.MapLeftKey("Comment");
                    mc.MapRightKey("Reply");
                });
            modelBuilder.Entity<Reply>()
                .HasOptional(e => e.Content)
                .WithMany(e => e.Replies)
                .HasForeignKey(e => e.ContentUid);

            #endregion

            #region Question

            Debug.WriteLine($"Creating model for {typeof(Question).Name} class.");
            modelBuilder.Entity<Question>().ToTable($"{typeof(Question).Name}s");
            modelBuilder.Entity<Question>().HasKey(e => e.Uid);
            modelBuilder.Entity<Question>()
                .HasOptional(q => q.Solution)
                .WithMany()
                .HasForeignKey(q => q.SolutionUid);
            modelBuilder.Entity<Question>()
                .HasRequired(q => q.User)
                .WithMany(u => u.Questions)
                .HasForeignKey(q => q.UserUid);
            modelBuilder.Entity<Question>()
                .HasOptional(q => q.CodeLanguage)
                .WithMany()
                .HasForeignKey(q => q.CodeLanguageUid);

            #endregion

            #region Rating

            Debug.WriteLine($"Creating model for {typeof(Rating).Name} class.");
            modelBuilder.Entity<Rating>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(Rating).Name}s");
            });
            modelBuilder.Entity<Rating>().HasKey(e => e.Uid);

            Debug.WriteLine($"Creating model for {typeof(ContentRating).Name} class.");
            modelBuilder.Entity<ContentRating>().ToTable($"{typeof(ContentRating).Name}s");
            modelBuilder.Entity<ContentRating>().HasKey(e => e.Uid);
            modelBuilder.Entity<ContentRating>()
                .HasRequired(c => c.Content)
                .WithMany(c => c.Ratings)
                .HasForeignKey(c => c.ContentUid);

            Debug.WriteLine($"Creating model for {typeof(CommentRating).Name} class.");
            modelBuilder.Entity<CommentRating>().ToTable($"{typeof(CommentRating).Name}s");
            modelBuilder.Entity<CommentRating>().HasKey(e => e.Uid);
            modelBuilder.Entity<CommentRating>()
                .HasRequired(c => c.Comment)
                .WithMany(c => c.Ratings)
                .HasForeignKey(c => c.CommentUid);

            #endregion

            Debug.WriteLine("Model creating completed.");
        }
    }
}
 