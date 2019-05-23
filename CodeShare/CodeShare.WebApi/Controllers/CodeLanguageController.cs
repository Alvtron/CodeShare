using CodeShare.Model;
using CodeShare.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeShare.WebApi.Controllers
{
    [Route("api/codelanguages")]
    [ApiController]
    public class CodeLanguageController : EntityController<CodeLanguage>
    {
        protected override DbSet<CodeLanguage> Entities => Context.CodeLanguages;

        protected override IQueryable<CodeLanguage> QueryableEntities => Entities;

        protected override IQueryable<CodeLanguage> QueryableEntitiesMinimal => Entities;

        [HttpGet]
        public new ActionResult<IEnumerable<CodeLanguage>> Get() => base.Get();

        [HttpGet("{uid}")]
        public new ActionResult<CodeLanguage> Get(Guid uid) => base.Get(uid);

        [HttpPut("{uid}")]
        public new ActionResult<CodeLanguage> Put(Guid uid, [FromBody] CodeLanguage entity) => base.Put(uid, entity);

        [HttpPost]
        public new ActionResult<CodeLanguage> Post(CodeLanguage entity) => base.Post(entity);

        [HttpDelete("{uid}")]
        public new IActionResult Delete(Guid uid) => base.Delete(uid);

        protected override void OnPost(CodeLanguage entity)
        {
            
        }

        protected override void OnPut(CodeLanguage entity, CodeLanguage existingEntity)
        {
            
        }

        protected override void OnDelete(CodeLanguage entity)
        {
            
        }
    }
}