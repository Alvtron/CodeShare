using CodeShare.Model;
using CodeShare.Utilities;
using CodeShare.WebApi.Controllers;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace ReportShare.WebApi.Controllers
{
    public class ReportController : EntityController<Report>
    {
        protected override DbSet<Report> Entities => Context.Reports;

        protected override IQueryable<Report> QueryableEntities => Entities
            .Include(a => a.ImageAttachments);

        protected override IQueryable<Report> QueryableEntitiesMinimal => Entities;

        [Route("api/reports")]
        public new IQueryable<Report> Get() => base.Get();

        [ResponseType(typeof(Report)), Route("api/reports/{uid}")]
        public new IHttpActionResult Get(Guid uid) => base.Get(uid);

        [ResponseType(typeof(void)), Route("api/reports/{uid}")]
        public new IHttpActionResult Put(Guid uid, [FromBody] Report entity) => base.Put(uid, entity);

        [ResponseType(typeof(Report)), Route("api/reports")]
        public new IHttpActionResult Post(Report entity) => base.Post(entity);

        [ResponseType(typeof(Report)), Route("api/reports/{uid}")]
        public new IHttpActionResult Delete(Guid uid) => base.Delete(uid);

        protected override void OnPut(Report entity, Report existingEntity)
        {
            UpdateEntities(entity.ImageAttachments, existingEntity.ImageAttachments);
        }

        protected override void OnPost(Report entity)
        {

        }

        protected override void OnDelete(Report entity)
        {

        }
    }
}