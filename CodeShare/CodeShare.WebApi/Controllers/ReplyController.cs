using CodeShare.Model;
using CodeShare.Utilities;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace CodeShare.WebApi.Controllers
{
    public class ReplyController : EntityController<Reply>
    {
        protected override DbSet<Reply> Entities => Context.Replies;

        protected override IQueryable<Reply> QueryableEntities => Entities
            .Include(c => c.User)
            .Include(c => c.User.ProfilePictures)
            .Include(c => c.User.Banners)
            .Include(c => c.User.Screenshots)
            .Include(e => e.Logs)
            .Include(c => c.Ratings)
            .Include(c => c.Replies.Select(comment => comment.User.ProfilePictures))
            .Include(c => c.Replies.Select(comment => comment.User.Banners))
            .Include(c => c.Replies.Select(comment => comment.Ratings));

        protected override IQueryable<Reply> QueryableEntitiesMinimal => Entities;

        [Route("api/replys")]
        public new IQueryable<Reply> Get() => base.Get();

        [ResponseType(typeof(Reply)), Route("api/replys/{uid}")]
        public new IHttpActionResult Get(Guid uid) => base.Get(uid);

        [ResponseType(typeof(void)), Route("api/replys/{uid}")]
        public new IHttpActionResult Put(Guid uid, [FromBody] Reply entity) => base.Put(uid, entity);

        [ResponseType(typeof(Reply)), Route("api/replys")]
        public new IHttpActionResult Post(Reply entity) => base.Post(entity);

        [ResponseType(typeof(Reply)), Route("api/replys/{uid}")]
        public new IHttpActionResult Delete(Guid uid) => base.Delete(uid);

        protected override void OnPut(Reply entity, Reply existingEntity)
        {
            UpdateEntities(entity.Replies, existingEntity.Replies);
            UpdateEntities(entity.Ratings, existingEntity.Ratings);
            UpdateEntities(entity.Logs, existingEntity.Logs);
        }

        protected override void OnPost(Reply entity)
        {

        }

        protected override void OnDelete(Reply entity)
        {

        }
    }
}