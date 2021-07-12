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
            BigSchoolContext1 context = new BigSchoolContext1();
            if(context.Attendances.Any(p =>p.Attendee == userID && p.CourseId == attendancesDto.Id))
            {
                return BadRequest("The attendance already exists!");
            }
            var attendace = new Attendance() { CourseId = attendancesDto.Id, Attendee = User.Identity.GetUserId() };
            context.Attendances.Add(attendace);
            context.SaveChanges();
            return Ok();

        }
    }
}
