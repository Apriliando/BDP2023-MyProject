using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Models;
using MyProject.Repository;
using MyProject.Repository.Interface;

namespace MyProject.Controllers
{
    [Route("api/[controller]"), Authorize]
    [ApiController]

    public class DepartmentsController : ControllerBase
    {
        private readonly DepartmentRepository departmentRepository;

        public DepartmentsController(DepartmentRepository departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }
        [HttpPost]
        public ActionResult Insert(Department department)
        {
            if(departmentRepository.Insert(department) > 0)
            {
                return this.StatusCode(200, new { status = 200, message = "Berhasil menambahkan data Department", data = department });
            }
            else
            {
                return this.StatusCode(400, new { status = 400, message = "Gagal manambahkan data Department", data = department });
            }
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var get = await departmentRepository.Get();

            if (get == null)
            {
                return this.StatusCode(404, new { status = 404, message = "Gagal mengambil data Department", data = get });
            }
            return this.StatusCode(200, new {status = 200, message = "Berhasil mengambil data Department", data = get});
        }
        [HttpGet("{ID}")]
        public ActionResult Get(int ID)
        {
            var get = departmentRepository.Get(ID);

            if (get == null)
            {
                //return NotFound();
                ProblemDetails problemDetails = new ProblemDetails();
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Detail = $"ID {ID} tidak ditemukan di dalam database!";
                problemDetails.Extensions.Add(new KeyValuePair<string, object>("data", ID));

                return this.StatusCode((int)StatusCodes.Status404NotFound, problemDetails);
            }
            return this.StatusCode(200, new { status = 200, message = "Berhasil mengambil data Department", data = get });
        }
        [HttpPut]
        public ActionResult Edit(Department department)
        {
            if (departmentRepository.Update(department) > 0)
            {
                return this.StatusCode(200, new { status = 200, message = "Berhasil memperbaharui data Department", data = department });
            }
            else
            {
                ProblemDetails problemDetails = new ProblemDetails();
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Detail = $"Gagal memperbaharui departemen dengan ID {department.ID}, ID tidak ditemukan";
                problemDetails.Extensions.Add(new KeyValuePair<string, object>("data", department.ID));

                return this.StatusCode((int)StatusCodes.Status400BadRequest, problemDetails);
            }
        }
        [HttpDelete]
        public ActionResult Delete(int ID)
        {
            if (departmentRepository.Delete(ID) > 0)
            {
                return this.StatusCode(200, new { status = 200, message = "Berhasil menghapus data Department", data = ID });
            }
            else
            {
                ProblemDetails problemDetails = new ProblemDetails();
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Detail = $"Gagal menghapus departemen dengan ID {ID}, ID tidak ditemukan";
                problemDetails.Extensions.Add(new KeyValuePair<string, object>("data", ID));

                return this.StatusCode((int)StatusCodes.Status404NotFound, problemDetails);
            }
        }
    }
}
