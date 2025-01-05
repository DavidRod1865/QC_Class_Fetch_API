using Microsoft.AspNetCore.Mvc;
using QC_ClassFetch.Services;
using QC_FetchAPI.Services;

namespace QC_FetchAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        [HttpGet]
        [Route("GetCourses")]
        public IActionResult GetCourses([FromQuery] string year, [FromQuery] string semester, [FromQuery] string department)
        {
            string semesterCode = CheckInput.CheckSemester(ref semester);
            string departmentCode = CheckInput.CheckDepartment(ref department);
            string yearCode = CheckInput.CheckYear(ref year);

            if (string.IsNullOrWhiteSpace(year) || string.IsNullOrWhiteSpace(semester) || string.IsNullOrWhiteSpace(department))
            {
                return BadRequest("Year, semester, and department are required.");
            }

            try
            {
                var result = FetchCourses.FetchCoursesData(yearCode, semesterCode, departmentCode);
                if (string.IsNullOrEmpty(result))
                {
                    return StatusCode(500, "Error fetching course data.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
