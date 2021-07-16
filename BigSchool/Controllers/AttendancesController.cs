using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace BigSchool.Controllers
{
    public class AttendancesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Attend(Course attendancesDto)
        {
            var userID = User.Identity.GetUserId();
            BigSchoolContext context = new BigSchoolContext();
            if(context.Attendances.Any(p =>p.Attendee == userID && p.CourseId == attendancesDto.Id))
            {
                context.Attendances.Remove(context.Attendances.SingleOrDefault(p => p.Attendee == userID && p.CourseId == attendancesDto.Id));
                context.SaveChanges();
                return Ok("cancel");
            }
            var attendace = new Attendance() { CourseId = attendancesDto.Id, Attendee = User.Identity.GetUserId() };
            context.Attendances.Add(attendace);
            context.SaveChanges();
            return Ok();

        }
    }
}
