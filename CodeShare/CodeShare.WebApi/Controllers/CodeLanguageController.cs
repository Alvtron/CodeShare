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
    public class CodeLanguageController : EntityController<CodeLanguage>
    {
        protected override DbSet<CodeLanguage> Entities => Context.CodeLanguages;

        protected override IQueryable<CodeLanguage> QueryableEntities => Entities;

        protected override IQueryable<CodeLanguage> QueryableEntitiesMinimal => Entities;

        [Route("api/codelanguages")]
        public new IQueryable<CodeLanguage> Get() => base.Get();

        [ResponseType(typeof(CodeLanguage)), Route("api/codelanguages/{uid}")]
        public new IHttpActionResult Get(Guid uid) => base.Get(uid);

        [ResponseType(typeof(void)), Route("api/codelanguages/{uid}")]
        public new IHttpActionResult Put(Guid uid, [FromBody] CodeLanguage entity) => base.Put(uid, entity);

        [ResponseType(typeof(CodeLanguage)), Route("api/codelanguages")]
        public new IHttpActionResult Post(CodeLanguage entity) => base.Post(entity);

        [ResponseType(typeof(CodeLanguage)), Route("api/codelanguages/{uid}")]
        public new IHttpActionResult Delete(Guid uid) => base.Delete(uid);

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