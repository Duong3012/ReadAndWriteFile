using DemoProject.BUL.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private IJobService _jobService;
        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet("get-list")]
        public IActionResult GetListJob()
        {
            try
            {
                var result = _jobService.GetList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost("read-file-xls")]
        public IActionResult ReadFileXls()
        {
            try
            {
                var result = false;
                //var filePath = Directory.GetFiles(@"ReadFileExcel.xlsx");
                var filePath = Path.Combine(Environment.CurrentDirectory, @"D:\TestJob.xls");
                result = _jobService.ReadFileXlsToDatatable(filePath);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost("export-file-docx")]
        public IActionResult ExportFileDocx()
        {
            try
            { 
                var result = _jobService.WriteDocxFile(_jobService.GetList().ToList());
                var fileName = $"{DateTime.Now.ToString("yyyyMMdd")}_jox.docx";
                return File(result, "application /vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
