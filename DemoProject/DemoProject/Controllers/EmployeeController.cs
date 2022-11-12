using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using DemoProject.BUL.Interface;
using DemoProject.BUL.MapperDTO;
using DemoProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("get-list")]
        public IActionResult GetListEmployee()
        {
            try
            {
                var listEmp = _employeeService.GetList();
                return Ok(listEmp.ToList());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var obj = _employeeService.GetById(id);
                return Ok(obj);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost("add-employee")]
        public IActionResult Insert(Employee obj)
        {
            try
            {
                var entity = _employeeService.Insert(obj);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("update-employee")]
        public IActionResult Update(Employee obj)
        {
            try
            {
                var entity = _employeeService.Update(obj);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete("delete-employee")]
        public IActionResult Delete(string id)
        {
            try
            {
                var entity = _employeeService.Delete(id);
                return Ok(entity);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost("read-excel-xsls")]
        public IActionResult ImportFileExcel(IFormFile files)
        {
            try
            {
                var result = false;
                //var filePath = Directory.GetFiles(@"ReadFileExcel.xlsx");
                var filePath = Path.Combine(Environment.CurrentDirectory, @"D:\ReadFileExcel.xlsx");
                result = _employeeService.ReadFiletoDatatable(filePath);
                //if (filePath.Length > 0)
                //{
                //    foreach (var item in filePath)
                //    {
                //        result = _employeeService.ReadFiletoDatatable(item);
                //    }
                //}
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [HttpPost("read-csv")]
        public IActionResult ImportFileCsv()
        {
            try
            {
                var listEmployee = new List<Employee>();

                using (var reader = new StreamReader(@"D:\test.csv", Encoding.Default))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<EmployeeMap>();
                    var records = csv.GetRecords<Employee>().ToList();
                    //var result = InsertToDataBase(records);
                    //MemoryStream stream = new MemoryStream();
                    //return result;
                }
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        /// <summary>
        /// Export excel using datatable.
        /// </summary>
        /// <returns></returns>
        [HttpPost("export-excel-using-datatable")]
        public IActionResult ExportFileExcel()
        {
            try
            {
                var listEmployee = _employeeService.GetList().ToList();
                DataTable dt = _employeeService.ToDataTable(listEmployee);
                //DataTable dt = list as DataTable;
                //Name of File  
                string fileName = $"{DateTime.Now.ToString("ddMMyyyy")}_Employees.xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    //Add DataTable in worksheet  
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        //Return xlsx Excel File  
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost("export-excel-closexml")]
        public IActionResult ExportFileExcelUsingClosedXml()
        {
            var fileName = $"{DateTime.Now.ToString("yyyyMMdd")}_Employees";
            var fileDto = _employeeService.ExportExcel(_employeeService.GetList().ToList());
            return File(fileDto, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

    }
}
