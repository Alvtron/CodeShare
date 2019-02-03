using CodeShare.Model;
using CodeShare.Utilities;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace CodeShare.WebApi.Controllers
{
    public class UsersController : EntityController<User>
    {
        protected override DbSet<User> Entities => Context.Users;

        protected override IQueryable<User> QueryableEntities => Entities
            .Include(e => e.Friends.Select(f => f.ProfilePictures))
            .Include(e => e.Codes.Select(f => f.Banners))
            .Include(e => e.Ratings)
            .Include(e => e.Logs)
            .Include(e => e.Replies.Select(comment => comment.User.ProfilePictures))
            .Include(e => e.Replies.Select(comment => comment.User.Banners))
            .Include(e => e.Replies.Select(comment => comment.Ratings))
            .Include(e => e.ProfilePictures)
            .Include(e => e.Banners)
            .Include(e => e.Videos);

        protected override IQueryable<User> QueryableEntitiesMinimal => Entities
            .Include(c => c.ProfilePictures)
            .Include(a => a.Banners);

        [Route("api/users")]
        public new IQueryable<User> Get() => base.Get();

        [ResponseType(typeof(User)), Route("api/users/{uid}")]
        public new IHttpActionResult Get(Guid uid) => base.Get(uid);

        [ResponseType(typeof(void)), Route("api/users/{uid}")]
        public new IHttpActionResult Put(Guid uid, [FromBody] User entity) => base.Put(uid, entity);

        [ResponseType(typeof(User)), Route("api/users")]
        public new IHttpActionResult Post(User entity) => base.Post(entity);

        [ResponseType(typeof(User)), Route("api/users/{uid}")]
        public new IHttpActionResult Delete(Guid uid) => base.Delete(uid);

        protected override void OnPost(User entity)
        {
            
        }

        protected override void OnPut(User entity, User existingEntity)
        {
            UpdateEntities(entity.Friends, existingEntity.Friends);
            UpdateEntities(entity.Replies, existingEntity.Replies);
            UpdateEntities(entity.Ratings, existingEntity.Ratings);
            UpdateEntities(entity.Logs, existingEntity.Logs);
            UpdateEntities(entity.ProfilePictures, existingEntity.ProfilePictures);
            UpdateEntities(entity.Banners, existingEntity.Banners);
            UpdateEntities(entity.Screenshots, existingEntity.Screenshots);
            UpdateEntities(entity.Videos, existingEntity.Videos);
        }

        protected override void OnDelete(User entity)
        {
            Context.Database.ExecuteSqlCommand("DELETE FROM dbo.Friendships WHERE Friend_A = @uid OR Friend_B = @uid", new SqlParameter("uid", entity.Uid));
        }
    }
}