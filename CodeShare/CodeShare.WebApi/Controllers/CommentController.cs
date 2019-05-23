using CodeShare.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeShare.WebApi.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : EntityController<Comment>
    {
        protected override DbSet<Comment> Entities => Context.Comments;

        protected override IQueryable<Comment> QueryableEntities => Entities
            .Include(c => c.User)
            .Include(c => c.User.ProfilePictures)
            .Include(c => c.User.Banners)
            .Include(e => e.Logs)
            .Include(c => c.Ratings)
            .Include(c => c.Replies)
                .ThenInclude(comment => comment.User.ProfilePictures)
            .Include(c => c.Replies)
                .ThenInclude(comment => comment.User.Banners)
            .Include(c => c.Replies)
                .ThenInclude(comment => comment.Ratings);

        protected override IQueryable<Comment> QueryableEntitiesMinimal => Entities;

        [HttpGet]
        public new ActionResult<IEnumerable<Comment>> Get() => base.Get();

        [HttpGet("{uid}")]
        public new ActionResult<Comment> Get(Guid uid) => base.Get(uid);

        [HttpPut("{uid}")]
        public new ActionResult<Comment> Put(Guid uid, [FromBody] Comment entity) => base.Put(uid, entity);

        [HttpPost]
        public new ActionResult<Comment> Post(Comment entity) => base.Post(entity);

        [HttpDelete("{uid}")]
        public new IActionResult Delete(Guid uid) => base.Delete(uid);

        protected override void OnPut(Comment entity, Comment existingEntity)
        {
            UpdateEntities(entity.Replies, existingEntity.Replies);
            UpdateEntities(entity.Ratings, existingEntity.Ratings);
            UpdateEntities(entity.Logs, existingEntity.Logs);
        }

        protected override void OnPost(Comment entity)
        {

        }

        protected override void OnDelete(Comment entity)
        {

        }
    }
}