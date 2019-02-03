using CodeShare.Model;
using CodeShare.Utilities;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace CodeShare.WebApi.Controllers
{
    public class CodeController : EntityController<Code>
    {
        protected override DbSet<Code> Entities => Context.Codes;

        protected override IQueryable<Code> QueryableEntities => Entities
            .Include(a => a.Banners)
            .Include(c => c.Screenshots)
            .Include(c => c.Videos)
            .Include(c => c.User)
            .Include(c => c.User.ProfilePictures)
            .Include(c => c.User.Banners)
            .Include(c => c.User.Screenshots)
            .Include(c => c.Files.Select(f => f.CodeLanguage))
            .Include(e => e.Logs)
            .Include(c => c.Ratings)
            .Include(c => c.Replies.Select(comment => comment.User.ProfilePictures))
            .Include(c => c.Replies.Select(comment => comment.User.Banners))
            .Include(c => c.Replies.Select(comment => comment.Ratings));

        protected override IQueryable<Code> QueryableEntitiesMinimal => Entities
            .Include(e => e.Banners)
            .Include(e => e.User.ProfilePictures);

        [Route("api/codes")]
        public new IQueryable<Code> Get() => base.Get();

        [ResponseType(typeof(Code)), Route("api/codes/{uid}")]
        public new IHttpActionResult Get(Guid uid) => base.Get(uid);

        [ResponseType(typeof(void)), Route("api/codes/{uid}")]
        public new IHttpActionResult Put(Guid uid, [FromBody] Code entity) => base.Put(uid, entity);

        [ResponseType(typeof(Code)), Route("api/codes")]
        public new IHttpActionResult Post(Code entity) =>  base.Post(entity);

        [ResponseType(typeof(Code)), Route("api/codes/{uid}")]
        public new IHttpActionResult Delete(Guid uid) => base.Delete(uid);

        protected override void OnPut(Code entity, Code existingEntity)
        {
            UpdateEntities(entity.Replies, existingEntity.Replies);
            UpdateEntities(entity.Ratings, existingEntity.Ratings);
            UpdateEntities(entity.Logs, existingEntity.Logs);
            UpdateEntities(entity.Files, existingEntity.Files);
            UpdateEntities(entity.Banners, existingEntity.Banners);
            UpdateEntities(entity.Screenshots, existingEntity.Screenshots);
            UpdateEntities(entity.Videos, existingEntity.Videos);
        }

        protected override void OnPost(Code entity)
        {

        }

        protected override void OnDelete(Code entity)
        {

        }
    }
}