using DemoProject.BUL.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Controllers
{
    [Route("api/course")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("get-list")]
        public IActionResult GetList()
        {
            try
            {
                var list = _courseService.GetList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("read-csv")]
        public IActionResult ImportFileCSV()
        {
            try
            {
                var path = @"D:\Course.csv";
                var resultData = _courseService.ReadCSVFile(path);
                //_courseService.ReadCSVFile(@"D:\Course.csv", _courseService.GetList().ToList());
                return Ok(resultData);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [HttpPost("read-csv-noheader")]
        public IActionResult ImportFileCSVNoHeader()
        {
            try
            {
                var path = @"D:\CourseNoHeader.csv";
                var resultData = _courseService.ReadCSVFileNoHeader(path);
                return Ok(resultData);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost("export-csv")]
        public IActionResult ExportFileCSV()
        {
            try
            {
                var resultData = _courseService.WriteCSVFile(_courseService.GetList().ToList());
                var fileName = $"{DateTime.Now.ToString("yyyyMMdd")}_Course.csv";
                return File(Encoding.UTF8.GetBytes(resultData.ToString()), "text/csv", fileName);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost("export-pdf")]
        public IActionResult ExportFileFdf()
        {
            try
            {
                var resultData = _courseService.WritePdfFile(_courseService.GetList().ToList());
                var fileName = $"{DateTime.Now.ToString("yyyyMMdd")}_Course.pdf";
                return File(resultData, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
