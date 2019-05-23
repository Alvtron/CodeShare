using CodeShare.Model;
using CodeShare.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeShare.WebApi.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class CodeFileController : EntityController<CodeFile>
    {
        protected override DbSet<CodeFile> Entities => Context.CodeFiles;

        protected override IQueryable<CodeFile> QueryableEntities => Entities
            .Include(e => e.Code.User)
            .Include(e => e.CodeLanguage);

        protected override IQueryable<CodeFile> QueryableEntitiesMinimal => Entities;

        [HttpGet]
        public new ActionResult<IEnumerable<CodeFile>> Get() => base.Get();

        [HttpGet("{uid}")]
        public new ActionResult<CodeFile> Get(Guid uid) => base.Get(uid);

        [HttpPut("{uid}")]
        public new ActionResult<CodeFile> Put(Guid uid, [FromBody] CodeFile entity) => base.Put(uid, entity);

        [HttpPost]
        public new ActionResult<CodeFile> Post(CodeFile entity) => base.Post(entity);

        [HttpDelete("{uid}")]
        public new IActionResult Delete(Guid uid) => base.Delete(uid);

        protected override void OnPost(CodeFile entity)
        {
        }

        protected override void OnPut(CodeFile entity, CodeFile existingEntity)
        {
        }

        protected override void OnDelete(CodeFile entity)
        {
        }
    }
}