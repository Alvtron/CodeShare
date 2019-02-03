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
    public class CodeFileController : EntityController<CodeFile>
    {
        protected override DbSet<CodeFile> Entities => Context.CodeFiles;

        protected override IQueryable<CodeFile> QueryableEntities => Entities
            .Include(e => e.Code.User)
            .Include(e => e.CodeLanguage);

        protected override IQueryable<CodeFile> QueryableEntitiesMinimal => Entities;

        [Route("api/codefiles")]
        public new IQueryable<CodeFile> Get() => base.Get();

        [ResponseType(typeof(CodeFile)), Route("api/codefiles/{uid}")]
        public new IHttpActionResult Get(Guid uid) => base.Get(uid);

        [ResponseType(typeof(void)), Route("api/codefiles/{uid}")]
        public new IHttpActionResult Put(Guid uid, [FromBody] CodeFile entity) => base.Put(uid, entity);
        
        [ResponseType(typeof(File)), Route("api/codefiles")]
        public new IHttpActionResult Post(CodeFile entity) => base.Post(entity);
        
        [ResponseType(typeof(File)), Route("api/codefiles/{uid}")]
        public new IHttpActionResult Delete(Guid uid) => base.Delete(uid);

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