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
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<CodeLanguage> CodeLanguages { get; set; }
        public virtual DbSet<Question> Questions { get; set; }

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

            Debug.WriteLine($"Creating model for {typeof(Log).Name} class.");
            // Log relations
            modelBuilder.Entity<Log>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(Log).Name}s");
            });
            modelBuilder.Entity<Log>().HasKey(e => e.Uid);
            modelBuilder.Entity<Log>()
                .HasOptional(c => c.Actor)
                .WithMany()
                .HasForeignKey(l => l.ActorUid)
                .WillCascadeOnDelete(false);

            Debug.WriteLine($"Creating model for {typeof(CommentLog).Name} class.");
            modelBuilder.Entity<CommentLog>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(CommentLog).Name}s");
            });
            modelBuilder.Entity<CommentLog>().HasKey(e => e.Uid);

            Debug.WriteLine($"Creating model for {typeof(ContentLog).Name} class.");
            modelBuilder.Entity<ContentLog>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(ContentLog).Name}s");
            });
            modelBuilder.Entity<ContentLog>().HasKey(e => e.Uid);
            modelBuilder.Entity<ContentLog>()
                .HasRequired(c => c.Content)
                .WithMany()
                .HasForeignKey(l => l.ContentUid)
                .WillCascadeOnDelete(false);

            Debug.WriteLine($"Creating model for {typeof(WebFile).Name} class.");
            modelBuilder.Entity<WebFile>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(WebFile).Name}s");
            });
            modelBuilder.Entity<WebFile>().HasKey(e => e.Uid);

            Debug.WriteLine($"Creating model for {typeof(Video).Name} class.");
            modelBuilder.Entity<Video>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(Video).Name}s");
            });
            modelBuilder.Entity<Video>().HasKey(e => e.Uid);

            Debug.WriteLine($"Creating model for {typeof(WebImage).Name} class.");
            modelBuilder.Entity<WebImage>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(WebImage).Name}s");
            });
            modelBuilder.Entity<WebImage>().HasKey(e => e.Uid);

            Debug.WriteLine($"Creating model for {typeof(ProfilePicture).Name} class.");
            modelBuilder.Entity<ProfilePicture>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(ProfilePicture).Name}s");
            });
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

            Debug.WriteLine($"Creating model for {typeof(Report).Name} class.");
            modelBuilder.Entity<Report>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(Report).Name}s");
            });
            modelBuilder.Entity<Report>().HasKey(e => e.Uid);

            Debug.WriteLine($"Creating model for {typeof(Rating).Name} class.");
            modelBuilder.Entity<Rating>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(Rating).Name}s");
            });
            modelBuilder.Entity<Rating>().HasKey(e => e.Uid);

            Debug.WriteLine($"Creating model for {typeof(CodeLanguage).Name} class.");
            modelBuilder.Entity<CodeLanguage>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(CodeLanguage).Name}s");
            });
            modelBuilder.Entity<CodeLanguage>().HasKey(e => e.Uid);

            Debug.WriteLine($"Creating model for {typeof(File).Name} class.");
            modelBuilder.Entity<File>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(File).Name}s");
            });
            modelBuilder.Entity<File>().HasKey(e => e.Uid);

            Debug.WriteLine($"Creating model for {typeof(Answer).Name} class.");
            modelBuilder.Entity<Answer>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(Answer).Name}s");
            });
            modelBuilder.Entity<Answer>().HasKey(e => e.Uid);

            Debug.WriteLine($"Creating model for {typeof(Content).Name} class.");
            modelBuilder.Entity<Content>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(Content).Name}s");
            });
            modelBuilder.Entity<Content>().HasKey(e => e.Uid);
            modelBuilder.Entity<Content>()
                .HasMany(e => e.Logs)
                .WithRequired(e => e.Content)
                .HasForeignKey(e => e.ContentUid);
            modelBuilder.Entity<Content>()
                .HasMany(c => c.Banners)
                .WithRequired(c => c.Content)
                .HasForeignKey(c => c.ContentUid);
            modelBuilder.Entity<Content>()
                .HasMany(c => c.Screenshots)
                .WithRequired(c => c.Content)
                .HasForeignKey(c => c.ContentUid);
            modelBuilder.Entity<Content>()
                .HasMany(c => c.Comments)
                .WithRequired(c => c.Content)
                .HasForeignKey(c => c.ContentUid);
            modelBuilder.Entity<Content>()
                .HasMany(c => c.Videos)
                .WithRequired(c => c.Content)
                .HasForeignKey(c => c.ContentUid);
            modelBuilder.Entity<Content>()
                .HasMany(c => c.Ratings)
                .WithRequired(c => c.Content)
                .HasForeignKey(c => c.ContentUid);
            modelBuilder.Entity<Content>()
                .HasMany(c => c.Files)
                .WithRequired(c => c.Content)
                .HasForeignKey(l => l.ContentUid);

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

            Debug.WriteLine($"Creating model for {typeof(Code).Name} class.");
            modelBuilder.Entity<Code>().ToTable("Codes");
            modelBuilder.Entity<Code>()
                .HasRequired(c => c.User)
                .WithMany(l => l.Codes)
                .HasForeignKey(c => c.UserUid);

            Debug.WriteLine($"Creating model for {typeof(Comment).Name} class.");
            modelBuilder.Entity<Comment>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable($"{typeof(Comment).Name}s");
            });
            modelBuilder.Entity<Comment>().HasKey(e => e.Uid);
            modelBuilder.Entity<Comment>()
                .HasMany(e => e.Logs)
                .WithRequired(e => e.Comment)
                .HasForeignKey(e => e.CommentUid);
            modelBuilder.Entity<Comment>()
                .HasMany(u => u.Replies)
                .WithMany()
                .Map(mc =>
                {
                    mc.ToTable("Replies");
                    mc.MapLeftKey("Parent");
                    mc.MapRightKey("Reply");
                });

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

            Debug.WriteLine("Model creating completed.");
        }
    }
}
 