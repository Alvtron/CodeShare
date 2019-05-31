// ***********************************************************************
// Assembly         : CodeShare.DataAccess
// Author           : Thomas Angeland
// Created          : 05-15-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-31-2019
// ***********************************************************************
// <copyright file="DataContext.cs" company="CodeShare.DataAccess">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.Utilities;
using Microsoft.EntityFrameworkCore;

namespace CodeShare.DataAccess
{
    /// <summary>
    /// Class DataContext.
    /// Implements the <see cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class DataContext : DbContext
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; private set; }
        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>The users.</value>
        public virtual DbSet<User> Users { get; set; }
        /// <summary>
        /// Gets or sets the friendships.
        /// </summary>
        /// <value>The friendships.</value>
        public virtual DbSet<Friendship> Friendships { get; set; }
        /// <summary>
        /// Gets or sets the codes.
        /// </summary>
        /// <value>The codes.</value>
        public virtual DbSet<Code> Codes { get; set; }
        /// <summary>
        /// Gets or sets the code languages.
        /// </summary>
        /// <value>The code languages.</value>
        public virtual DbSet<CodeLanguage> CodeLanguages { get; set; }
        /// <summary>
        /// Gets or sets the questions.
        /// </summary>
        /// <value>The questions.</value>
        public virtual DbSet<Question> Questions { get; set; }
        /// <summary>
        /// Gets or sets the code files.
        /// </summary>
        /// <value>The code files.</value>
        public virtual DbSet<CodeFile> CodeFiles { get; set; }
        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        public virtual DbSet<Comment> Comments { get; set; }
        /// <summary>
        /// Gets or sets the code comment sections.
        /// </summary>
        /// <value>The code comment sections.</value>
        public virtual DbSet<CodeCommentSection> CodeCommentSections { get; set; }
        /// <summary>
        /// Gets or sets the question comment sections.
        /// </summary>
        /// <value>The question comment sections.</value>
        public virtual DbSet<QuestionCommentSection> QuestionCommentSections { get; set; }
        /// <summary>
        /// Gets or sets the code ratings.
        /// </summary>
        /// <value>The code ratings.</value>
        public virtual DbSet<CodeRatingCollection> CodeRatings { get; set; }
        /// <summary>
        /// Gets or sets the question ratings.
        /// </summary>
        /// <value>The question ratings.</value>
        public virtual DbSet<QuestionRatingCollection> QuestionRatings { get; set; }
        /// <summary>
        /// Gets or sets the ratings.
        /// </summary>
        /// <value>The ratings.</value>
        public virtual DbSet<Rating> Ratings { get; set; }
        /// <summary>
        /// Gets or sets the reports.
        /// </summary>
        /// <value>The reports.</value>
        public virtual DbSet<Report> Reports { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DataContext(string connectionString = @"Server=donau.hiof.no;Database=thomaang;Persist Security Info=True;User ID=thomaang;Password=St5hdA")
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// <para>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// This method is called for each instance of the context that is created.
        /// The base implementation does nothing.
        /// </para>
        /// <para>
        /// In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" /> may or may not have been passed
        /// to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" /> to determine if
        /// the options have already been set, and skip some or all of the logic in
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
        /// </para>
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or modify options for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you to configure the context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.</remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Logger.WriteLine("Creating model for data context...");

            #region Logs
            Logger.WriteLine($"Creating model for {typeof(Log).Name} class.");
            modelBuilder.Entity<Log>().ToTable($"{typeof(Log).Name}s");
            modelBuilder.Entity<Log>().HasKey(l => l.Uid);

            Logger.WriteLine($"Creating model for {typeof(UserLog).Name} class.");
            modelBuilder.Entity<UserLog>().ToTable($"{typeof(UserLog).Name}s");
            modelBuilder.Entity<UserLog>().HasBaseType<Log>();
            modelBuilder.Entity<UserLog>()
                .HasOne(c => c.User)
                .WithMany(u => u.Logs)
                .HasForeignKey(l => l.UserUid);

            Logger.WriteLine($"Creating model for {typeof(CodeLog).Name} class.");
            modelBuilder.Entity<CodeLog>().ToTable($"{typeof(CodeLog).Name}s");
            modelBuilder.Entity<CodeLog>().HasBaseType<Log>();
            modelBuilder.Entity<CodeLog>()
                .HasOne(c => c.Code)
                .WithMany(u => u.Logs)
                .HasForeignKey(l => l.CodeUid);

            Logger.WriteLine($"Creating model for {typeof(QuestionLog).Name} class.");
            modelBuilder.Entity<QuestionLog>().ToTable($"{typeof(QuestionLog).Name}s");
            modelBuilder.Entity<QuestionLog>().HasBaseType<Log>();
            modelBuilder.Entity<QuestionLog>()
                .HasOne(c => c.Question)
                .WithMany(u => u.Logs)
                .HasForeignKey(l => l.QuestionUid);

            Logger.WriteLine($"Creating model for {typeof(CommentLog).Name} class.");
            modelBuilder.Entity<CommentLog>().ToTable($"{typeof(CommentLog).Name}s");
            modelBuilder.Entity<CommentLog>().HasBaseType<Log>();
            modelBuilder.Entity<CommentLog>()
                .HasOne(c => c.Comment)
                .WithMany(u => u.Logs)
                .HasForeignKey(l => l.CommentUid);

            #endregion

            #region Videos

            Logger.WriteLine($"Creating model for {typeof(Video).Name} class.");
            modelBuilder.Entity<Video>().ToTable($"{typeof(Video).Name}s");
            modelBuilder.Entity<Video>().HasKey(e => e.Uid);

            Logger.WriteLine($"Creating model for {typeof(CodeVideo).Name} class.");
            modelBuilder.Entity<CodeVideo>().ToTable($"{typeof(CodeVideo).Name}s");
            modelBuilder.Entity<CodeVideo>().HasBaseType<Video>();
            modelBuilder.Entity<CodeVideo>()
                .HasOne(e => e.Code)
                .WithMany(e => e.Videos)
                .HasForeignKey(e => e.CodeUid);

            Logger.WriteLine($"Creating model for {typeof(QuestionVideo).Name} class.");
            modelBuilder.Entity<QuestionVideo>().ToTable($"{typeof(QuestionVideo).Name}s");
            modelBuilder.Entity<QuestionVideo>().HasBaseType<Video>();
            modelBuilder.Entity<QuestionVideo>()
                .HasOne(e => e.Question)
                .WithMany(e => e.Videos)
                .HasForeignKey(e => e.QuestionUid);

            #endregion

            #region Images

            Logger.WriteLine($"Creating model for {typeof(WebFile).Name} class.");
            modelBuilder.Entity<WebFile>().HasKey(e => e.Uid);

            Logger.WriteLine($"Creating model for {typeof(WebImage).Name} class.");
            modelBuilder.Entity<WebImage>().ToTable($"{typeof(WebImage).Name}s");
            modelBuilder.Entity<WebImage>().HasBaseType<WebFile>();
            modelBuilder.Entity<WebImage>().OwnsOne(wi => wi.Crop,
                crop =>
                {
                    crop.Property(c => c.AspectRatio).HasColumnName("Crop_AspectRatio");
                    crop.Property(c => c.Width).HasColumnName("Crop_Width");
                    crop.Property(c => c.Height).HasColumnName("Crop_Height");
                    crop.Property(c => c.X).HasColumnName("Crop_X");
                    crop.Property(c => c.Y).HasColumnName("Crop_Y");
                });

            Logger.WriteLine($"Creating model for {typeof(UserAvatar).Name} class.");
            modelBuilder.Entity<UserAvatar>().ToTable($"{typeof(UserAvatar).Name}s");
            modelBuilder.Entity<UserAvatar>().HasBaseType<WebImage>();
            modelBuilder.Entity<UserAvatar>()
                .HasOne(e => e.User)
                .WithMany(e => e.Avatars)
                .HasForeignKey(e => e.UserUid);

            Logger.WriteLine($"Creating model for {typeof(UserBanner).Name} class.");
            modelBuilder.Entity<UserBanner>().ToTable($"{typeof(UserBanner).Name}s");
            modelBuilder.Entity<UserBanner>().HasBaseType<WebImage>();
            modelBuilder.Entity<UserBanner>()
                .HasOne(e => e.User)
                .WithMany(e => e.Banners)
                .HasForeignKey(e => e.UserUid);

            Logger.WriteLine($"Creating model for {typeof(CodeBanner).Name} class.");
            modelBuilder.Entity<CodeBanner>().ToTable($"{typeof(CodeBanner).Name}s");
            modelBuilder.Entity<CodeBanner>().HasBaseType<WebImage>();
            modelBuilder.Entity<CodeBanner>()
                .HasOne(e => e.Code)
                .WithMany(e => e.Banners)
                .HasForeignKey(e => e.CodeUid);

            Logger.WriteLine($"Creating model for {typeof(CodeScreenshot).Name} class.");
            modelBuilder.Entity<CodeScreenshot>().ToTable($"{typeof(CodeScreenshot).Name}s");
            modelBuilder.Entity<CodeScreenshot>().HasBaseType<WebImage>();
            modelBuilder.Entity<CodeScreenshot>()
                .HasOne(e => e.Code)
                .WithMany(e => e.Screenshots)
                .HasForeignKey(e => e.CodeUid);

            Logger.WriteLine($"Creating model for {typeof(QuestionScreenshot).Name} class.");
            modelBuilder.Entity<QuestionScreenshot>().ToTable($"{typeof(QuestionScreenshot).Name}s");
            modelBuilder.Entity<QuestionScreenshot>().HasBaseType<WebImage>();
            modelBuilder.Entity<QuestionScreenshot>()
                .HasOne(e => e.Question)
                .WithMany(e => e.Screenshots)
                .HasForeignKey(e => e.QuestionUid);

            Logger.WriteLine($"Creating model for {typeof(ReportImage).Name} class.");
            modelBuilder.Entity<ReportImage>().ToTable($"{typeof(ReportImage).Name}s");
            modelBuilder.Entity<ReportImage>().HasBaseType<WebImage>();
            modelBuilder.Entity<ReportImage>()
                .HasOne(e => e.Report)
                .WithMany(e => e.ImageAttachments)
                .HasForeignKey(e => e.ReportUid);

            #endregion

            #region Report

            Logger.WriteLine($"Creating model for {typeof(Report).Name} class.");
            modelBuilder.Entity<Report>().ToTable($"{typeof(Report).Name}s");
            modelBuilder.Entity<Report>().HasKey(e => e.Uid);
            modelBuilder.Entity<Report>()
                .HasMany(e => e.ImageAttachments)
                .WithOne(e => e.Report)
                .HasForeignKey(e => e.ReportUid);

            #endregion

            #region CodeLanguage

            Logger.WriteLine($"Creating model for {typeof(CodeLanguage).Name} class.");
            modelBuilder.Entity<CodeLanguage>().ToTable($"{typeof(CodeLanguage).Name}s");
            modelBuilder.Entity<CodeLanguage>().HasKey(e => e.Uid);
            modelBuilder.Entity<CodeLanguage>().OwnsOne(x => x.Syntax,
                syntax =>
                {
                    syntax.Property(x => x.Comments).HasColumnName("Syntax_Comments");
                    syntax.Property(x => x.Delimiter).HasColumnName("Syntax_Delimiter");
                    syntax.Property(x => x.Keywords).HasColumnName("Syntax_Keywords");
                });
            #endregion

            #region User

            Logger.WriteLine($"Creating model for {typeof(User).Name} class.");
            modelBuilder.Entity<User>().ToTable($"{typeof(User).Name}s");
            modelBuilder.Entity<User>().HasKey(e => e.Uid);
            modelBuilder.Entity<User>()
                .HasMany(c => c.Logs)
                .WithOne(l => l.User)
                .HasForeignKey(c => c.UserUid);
            modelBuilder.Entity<User>()
                .HasMany(u => u.Codes)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserUid);
            modelBuilder.Entity<User>()
                .HasMany(p => p.Avatars)
                .WithOne(d => d.User)
                .HasForeignKey(d => d.UserUid);
            modelBuilder.Entity<User>()
                .HasOne(e => e.Avatar)
                .WithOne()
                .HasForeignKey<User>(e => e.AvatarUid);
            modelBuilder.Entity<User>()
                .HasMany(p => p.Banners)
                .WithOne(d => d.User)
                .HasForeignKey(d => d.UserUid);
            modelBuilder.Entity<User>()
                .HasOne(e => e.Banner)
                .WithOne()
                .HasForeignKey<User>(e => e.BannerUid);
            modelBuilder.Entity<User>().OwnsOne(x => x.Email,
                email =>
                {
                    email.Property(x => x.Address).HasColumnName("EmailAddress");
                });
            modelBuilder.Entity<User>().OwnsOne(x => x.Password,
                password =>
                {
                    password.Property(x => x.Iterations).HasColumnName("Password_Iterations");
                    password.Property(x => x.Salt).HasColumnName("Password_Salt");
                    password.Property(x => x.Hash).HasColumnName("Password_Hash");
                });
            modelBuilder.Entity<User>().OwnsOne(x => x.Color,
                color =>
                {
                    color.Property(x => x.R).HasColumnName("Color_R");
                    color.Property(x => x.G).HasColumnName("Color_G");
                    color.Property(x => x.B).HasColumnName("Color_B");
                    color.Property(x => x.A).HasColumnName("Color_A");
                });

            modelBuilder.Entity<Friendship>().ToTable($"{typeof(Friendship).Name}s");
            modelBuilder.Entity<Friendship>().HasKey(e => e.Uid);
            modelBuilder.Entity<Friendship>()
                .HasOne(a => a.Requester)
                .WithMany(b => b.SentFriendRequests)
                .HasForeignKey(c => c.RequesterUid)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
                .HasOne(a => a.Confirmer)
                .WithMany(b => b.ReceivedFriendRequests)
                .HasForeignKey(c => c.ConfirmerUid)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Code

            Logger.WriteLine($"Creating model for {typeof(Code).Name} class.");
            modelBuilder.Entity<Code>().ToTable("Codes");
            modelBuilder.Entity<Code>().HasKey(e => e.Uid);
            modelBuilder.Entity<Code>()
                .HasMany(c => c.Logs)
                .WithOne(l => l.Code)
                .HasForeignKey(c => c.CodeUid);
            modelBuilder.Entity<Code>()
                .HasOne(c => c.User)
                .WithMany(l => l.Codes)
                .HasForeignKey(c => c.UserUid);
            modelBuilder.Entity<Code>()
                .HasMany(c => c.Files)
                .WithOne(c => c.Code)
                .HasForeignKey(l => l.CodeUid);
            modelBuilder.Entity<Code>()
                .HasMany(p => p.Banners)
                .WithOne(d => d.Code)
                .HasForeignKey(d => d.CodeUid);
            modelBuilder.Entity<Code>()
                .HasOne(e => e.Banner)
                .WithOne()
                .HasForeignKey<Code>(e => e.BannerUid);
            modelBuilder.Entity<Code>()
                .HasMany(p => p.Screenshots)
                .WithOne(d => d.Code)
                .HasForeignKey(d => d.CodeUid);
            modelBuilder.Entity<Code>()
                .HasOne(c => c.CommentSection)
                .WithOne(l => l.Code)
                .HasForeignKey<CodeCommentSection>(c => c.CodeUid);
            modelBuilder.Entity<Code>()
                .HasMany(c => c.Videos)
                .WithOne(c => c.Code)
                .HasForeignKey(c => c.CodeUid);
            modelBuilder.Entity<Code>()
                .HasOne(e => e.RatingCollection)
                .WithOne(e => e.Code)
                .HasForeignKey<CodeRatingCollection>(e => e.CodeUid);

            Logger.WriteLine($"Creating model for {typeof(CodeFile).Name} class.");
            modelBuilder.Entity<CodeFile>().ToTable($"{typeof(CodeFile).Name}s");
            modelBuilder.Entity<CodeFile>().HasKey(e => e.Uid);
            modelBuilder.Entity<CodeFile>()
                .HasOne(c => c.Code)
                .WithMany(e => e.Files)
                .HasForeignKey(c => c.CodeUid);
            modelBuilder.Entity<CodeFile>()
                .HasOne(c => c.CodeLanguage)
                .WithMany()
                .HasForeignKey(c => c.CodeLanguageUid);

            #endregion

            #region Question

            Logger.WriteLine($"Creating model for {typeof(Question).Name} class.");
            modelBuilder.Entity<Question>().ToTable($"{typeof(Question).Name}s");
            modelBuilder.Entity<Question>().HasKey(e => e.Uid);
            modelBuilder.Entity<Question>()
                .HasMany(c => c.Logs)
                .WithOne(l => l.Question)
                .HasForeignKey(c => c.QuestionUid);
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Solution)
                .WithMany()
                .HasForeignKey(q => q.SolutionUid);
            modelBuilder.Entity<Question>()
                .HasOne(q => q.User)
                .WithMany(u => u.Questions)
                .HasForeignKey(q => q.UserUid);
            modelBuilder.Entity<Question>()
                .HasOne(q => q.CodeLanguage)
                .WithMany()
                .HasForeignKey(q => q.CodeLanguageUid);
            modelBuilder.Entity<Question>()
                .HasMany(p => p.Screenshots)
                .WithOne(d => d.Question)
                .HasForeignKey(d => d.QuestionUid);
            modelBuilder.Entity<Question>()
                .HasOne(c => c.CommentSection)
                .WithOne(l => l.Question)
                .HasForeignKey<QuestionCommentSection>(c => c.QuestionUid);
            modelBuilder.Entity<Question>()
                .HasMany(c => c.Videos)
                .WithOne(c => c.Question)
                .HasForeignKey(c => c.QuestionUid);
            modelBuilder.Entity<Question>()
                .HasOne(e => e.RatingCollection)
                .WithOne(e => e.Question)
                .HasForeignKey<QuestionRatingCollection>(e => e.QuestionUid);

            #endregion

            #region Comments

            Logger.WriteLine($"Creating model for {typeof(CommentSection).Name} class.");
            modelBuilder.Entity<CommentSection>().ToTable($"{typeof(CommentSection).Name}s");
            modelBuilder.Entity<CommentSection>().HasKey(e => e.Uid);
            modelBuilder.Entity<CommentSection>()
                .HasMany(c => c.Replies)
                .WithOne()
                .HasForeignKey(c => c.CommentSectionUid);

            Logger.WriteLine($"Creating model for {typeof(CodeCommentSection).Name} class.");
            modelBuilder.Entity<CodeCommentSection>().ToTable($"{typeof(CodeCommentSection).Name}s");
            modelBuilder.Entity<CodeCommentSection>().HasBaseType<CommentSection>();
            modelBuilder.Entity<CodeCommentSection>()
                .HasOne(c => c.Code)
                .WithOne(l => l.CommentSection)
                .HasForeignKey<Code>(c => c.CommentSectionUid);

            Logger.WriteLine($"Creating model for {typeof(QuestionCommentSection).Name} class.");
            modelBuilder.Entity<QuestionCommentSection>().ToTable($"{typeof(QuestionCommentSection).Name}s");
            modelBuilder.Entity<QuestionCommentSection>().HasBaseType<CommentSection>();
            modelBuilder.Entity<QuestionCommentSection>()
                .HasOne(c => c.Question)
                .WithOne(l => l.CommentSection)
                .HasForeignKey<Question>(c => c.CommentSectionUid);

            Logger.WriteLine($"Creating model for {typeof(Comment).Name} class.");
            modelBuilder.Entity<Comment>().ToTable($"{typeof(Comment).Name}s");
            modelBuilder.Entity<Comment>().HasKey(e => e.Uid);
            modelBuilder.Entity<Comment>()
                .HasMany(c => c.Logs)
                .WithOne(l => l.Comment)
                .HasForeignKey(c => c.CommentUid);
            modelBuilder.Entity<Comment>()
                .HasOne(e => e.Parent)
                .WithMany(e => e.Replies)
                .HasForeignKey(e => e.ParentUid);
            modelBuilder.Entity<Comment>()
                .HasOne(e => e.User)
                .WithMany(e => e.Comments)
                .HasForeignKey(e => e.UserUid);
            modelBuilder.Entity<Comment>()
                .HasMany(e => e.Logs)
                .WithOne(e => e.Comment)
                .HasForeignKey(e => e.CommentUid);
            modelBuilder.Entity<Comment>()
                .HasOne(e => e.RatingCollection)
                .WithOne(e => e.Comment)
                .HasForeignKey<CommentRatingCollection>(e => e.CommentUid);

            #endregion

            #region Rating

            Logger.WriteLine($"Creating model for {typeof(Rating).Name} class.");
            modelBuilder.Entity<Rating>().ToTable($"{typeof(Rating).Name}s");
            modelBuilder.Entity<Rating>().HasKey(e => e.Uid);
            modelBuilder.Entity<Rating>()
                .HasOne(c => c.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(c => c.UserUid);
            modelBuilder.Entity<Rating>()
                .Property(e => e.Value)
                .HasConversion<int>();

            modelBuilder.Entity<RatingCollection>().ToTable($"{typeof(RatingCollection).Name}s");
            modelBuilder.Entity<RatingCollection>().HasKey(e => e.Uid);
            modelBuilder.Entity<RatingCollection>()
                .HasMany(c => c.Ratings)
                .WithOne()
                .HasForeignKey(c => c.RatingsUid);

            Logger.WriteLine($"Creating model for {typeof(CodeRatingCollection).Name} class.");
            modelBuilder.Entity<CodeRatingCollection>().ToTable($"{typeof(CodeRatingCollection).Name}s");
            modelBuilder.Entity<CodeRatingCollection>().HasBaseType<RatingCollection>();
            modelBuilder.Entity<CodeRatingCollection>()
                .HasOne(c => c.Code)
                .WithOne(l => l.RatingCollection)
                .HasForeignKey<Code>(c => c.RatingCollectionUid);

            Logger.WriteLine($"Creating model for {typeof(QuestionRatingCollection).Name} class.");
            modelBuilder.Entity<QuestionRatingCollection>().ToTable($"{typeof(QuestionRatingCollection).Name}s");
            modelBuilder.Entity<QuestionRatingCollection>().HasBaseType<RatingCollection>();
            modelBuilder.Entity<QuestionRatingCollection>()
                .HasOne(c => c.Question)
                .WithOne(l => l.RatingCollection)
                .HasForeignKey<Question>(c => c.RatingCollectionUid);

            Logger.WriteLine($"Creating model for {typeof(CommentRatingCollection).Name} class.");
            modelBuilder.Entity<CommentRatingCollection>().ToTable($"{typeof(QuestionRatingCollection).Name}s");
            modelBuilder.Entity<CommentRatingCollection>().HasBaseType<RatingCollection>();
            modelBuilder.Entity<CommentRatingCollection>()
                .HasOne(c => c.Comment)
                .WithOne(l => l.RatingCollection)
                .HasForeignKey<Comment>(c => c.RatingsUid);

            #endregion

            Logger.WriteLine("Model creating completed.");
        }
    }
}
