using CodeShare.Model;
using CodeShare.Utilities;
using System;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CodeShare.WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : EntityController<User>
    {
        protected override DbSet<User> Entities => Context.Users;

        protected override IQueryable<User> QueryableEntities => Entities
            .Include(e => e.SentFriendRequests)
                .ThenInclude(e => e.Confirmer)
            .Include(e => e.ReceievedFriendRequests)
                .ThenInclude(e => e.Requester)
            .Include(e => e.Codes)
                .ThenInclude(f => f.Banners)
            .Include(e => e.Logs)
            .Include(e => e.ProfilePictures)
            .Include(e => e.Banners);

        protected override IQueryable<User> QueryableEntitiesMinimal => Entities
            .Include(c => c.ProfilePictures)
            .Include(a => a.Banners);

        [HttpGet]
        public new ActionResult<IEnumerable<User>> Get() => base.Get();

        [HttpGet("{uid}")]
        public new ActionResult<User> Get(Guid uid) => base.Get(uid);

        [HttpPut("{uid}")]
        public new ActionResult<User> Put(Guid uid, [FromBody] User entity) => base.Put(uid, entity);

        [HttpPost]
        public new ActionResult<User> Post(User entity) => base.Post(entity);

        [HttpDelete("{uid}")]
        public new IActionResult Delete(Guid uid) => base.Delete(uid);

        protected override void OnPost(User entity)
        {
            
        }

        protected override void OnPut(User entity, User existingEntity)
        {
            UpdateEntities(entity.SentFriendRequests, existingEntity.SentFriendRequests);
            UpdateEntities(entity.ReceievedFriendRequests, existingEntity.ReceievedFriendRequests);
            UpdateEntities(entity.Logs, existingEntity.Logs);
            UpdateEntities(entity.ProfilePictures, existingEntity.ProfilePictures);
            UpdateEntities(entity.Banners, existingEntity.Banners);
        }

        protected override void OnDelete(User entity)
        {
            Context.Database.ExecuteSqlCommand("DELETE FROM dbo.Friendships WHERE Friend_A = @uid OR Friend_B = @uid", new SqlParameter("uid", entity.Uid));
        }
    }
}