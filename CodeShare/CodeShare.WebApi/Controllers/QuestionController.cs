using CodeShare.Model;
using CodeShare.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeShare.WebApi.Controllers
{
    [Route("api/questions")]
    [ApiController]
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
            .Include(c => c.CommentSection)
                .ThenInclude(cs => cs.Replies);

        protected override IQueryable<Question> QueryableEntitiesMinimal => Entities
            .Include(q => q.User.ProfilePictures)
            .Include(q => q.User.Banners)
            .Include(q => q.CodeLanguage);

        [HttpGet]
        public new ActionResult<IEnumerable<Question>> Get() => base.Get();

        [HttpGet("{uid}")]
        public new ActionResult<Question> Get(Guid uid) => base.Get(uid);

        [HttpPut("{uid}")]
        public new ActionResult<Question> Put(Guid uid, [FromBody] Question entity) => base.Put(uid, entity);

        [HttpPost]
        public new ActionResult<Question> Post(Question entity) => base.Post(entity);

        [HttpDelete("{uid}")]
        public new IActionResult Delete(Guid uid) => base.Delete(uid);

        protected override void OnPut(Question entity, Question existingEntity)
        {
            //UpdateEntities(entity.Replies, existingEntity.Replies);
            UpdateEntities(entity.Ratings, existingEntity.Ratings);
            UpdateEntities(entity.Logs, existingEntity.Logs);
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