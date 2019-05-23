using CodeShare.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeShare.WebApi.Controllers
{
    [Route("api/codes")]
    [ApiController]
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
            .Include(c => c.Files)
                .ThenInclude(f => f.CodeLanguage)
            .Include(e => e.Logs)
            .Include(c => c.Ratings)
            .Include(c => c.CommentSection)
                .ThenInclude(cs => cs.Replies);

        protected override IQueryable<Code> QueryableEntitiesMinimal => Entities
            .Include(e => e.Banners)
            .Include(e => e.User.ProfilePictures);

        [HttpGet]
        public new ActionResult<IEnumerable<Code>> Get() => base.Get();

        [HttpGet("{uid}")]
        public new ActionResult<Code> Get(Guid uid) => base.Get(uid);

        [HttpPut("{uid}")]
        public new ActionResult<Code> Put(Guid uid, [FromBody] Code entity) => base.Put(uid, entity);

        [HttpPost]
        public new ActionResult<Code> Post(Code entity) =>  base.Post(entity);

        [HttpDelete("{uid}")]
        public new IActionResult Delete(Guid uid) => base.Delete(uid);

        protected override void OnPut(Code entity, Code existingEntity)
        {
            //UpdateEntities(entity.Replies, existingEntity.Replies);
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