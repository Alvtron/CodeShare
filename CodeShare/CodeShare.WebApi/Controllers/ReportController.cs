using CodeShare.Model;
using CodeShare.Utilities;
using CodeShare.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReportShare.WebApi.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportController : EntityController<Report>
    {
        protected override DbSet<Report> Entities => Context.Reports;

        protected override IQueryable<Report> QueryableEntities => Entities
            .Include(a => a.ImageAttachments);

        protected override IQueryable<Report> QueryableEntitiesMinimal => Entities;

        [HttpGet]
        public new ActionResult<IEnumerable<Report>> Get() => base.Get();

        [HttpGet("{uid}")]
        public new ActionResult<Report> Get(Guid uid) => base.Get(uid);

        [HttpPut("{uid}")]
        public new ActionResult<Report> Put(Guid uid, [FromBody] Report entity) => base.Put(uid, entity);

        [HttpPost]
        public new ActionResult<Report> Post(Report entity) => base.Post(entity);

        [HttpDelete("{uid}")]
        public new IActionResult Delete(Guid uid) => base.Delete(uid);

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