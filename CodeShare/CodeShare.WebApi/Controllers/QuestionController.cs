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
    public class QuestionController : EntityController<Question>
    {
        protected override DbSet<Question> Entities => Context.Questions;

        protected override IQueryable<Question> QueryableEntities => Entities
            .Include(q => q.User.ProfilePictures)
            .Include(q => q.User.Banners)
            .Include(e => e.Logs)
            .Include(q => q.Solution.User.ProfilePictures)
            .Include(q => q.Solution.User.Banners)
            .Include(q => q.CodeLanguage)
            .Include(q => q.Ratings)
            .Include(q => q.Replies)
            .Include(q => q.Replies.Select(comment => comment.User.ProfilePictures))
            .Include(q => q.Replies.Select(comment => comment.User.Banners));

        protected override IQueryable<Question> QueryableEntitiesMinimal => Entities
            .Include(a => a.Banners)
            .Include(q => q.User.ProfilePictures)
            .Include(q => q.User.Banners)
            .Include(q => q.CodeLanguage);

        [Route("api/questions")]
        public new IQueryable<Question> Get() => base.Get();

        [ResponseType(typeof(Question)), Route("api/questions/{uid}")]
        public new IHttpActionResult Get(Guid uid) => base.Get(uid);

        [ResponseType(typeof(void)), Route("api/questions/{uid}")]
        public new IHttpActionResult Put(Guid uid, [FromBody] Question entity) => base.Put(uid, entity);

        [ResponseType(typeof(Question)), Route("api/questions")]
        public new IHttpActionResult Post(Question entity) => base.Post(entity);

        [ResponseType(typeof(Question)), Route("api/questions/{uid}")]
        public new IHttpActionResult Delete(Guid uid) => base.Delete(uid);

        protected override void OnPut(Question entity, Question existingEntity)
        {
            UpdateEntities(entity.Replies, existingEntity.Replies);
            UpdateEntities(entity.Ratings, existingEntity.Ratings);
            UpdateEntities(entity.Logs, existingEntity.Logs);
            UpdateEntities(entity.Banners, existingEntity.Banners);
            UpdateEntities(entity.Screenshots, existingEntity.Screenshots);
            UpdateEntities(entity.Videos, existingEntity.Videos);
        }

        protected override void OnPost(Question entity)
        {

        }

        protected override void OnDelete(Question entity)
        {

        }
    }
}