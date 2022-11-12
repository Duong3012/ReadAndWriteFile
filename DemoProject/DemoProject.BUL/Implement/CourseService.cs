using CsvHelper;
using CsvHelper.Configuration;
using DemoProject.BUL.Interface;
using DemoProject.BUL.MapperDTO;
using DemoProject.DAL.Interface.UnitOfWork;
using DemoProject.Models;
using DinkToPdf;
using DinkToPdf.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DemoProject.BUL.Implement
{
    public class CourseService : ICourseService
    {
        private IUnitOfWork _unitOfWork;
        private IConverter _converter;
        public CourseService(IUnitOfWork unitOfWork, IConverter converter)
        {
            _unitOfWork = unitOfWork;
            _converter = converter;
        }

        public IEnumerable<Course> GetList()
        {
            try
            {
                return _unitOfWork.CourseRepository.GetList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Insert(Course obj)
        {
            try
            {
                if (obj == null)
                {
                    return false;
                }
                var entity = _unitOfWork.CourseRepository.InsertObject(obj);
                return entity;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool ReadCSVFile(string location)
        {
            try
            {
                using (var reader = new StreamReader(location, Encoding.Default))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<CourseMap>();
                    var records = csv.GetRecords<Course>().ToList();
                    var result = InsertToDataBase(records);
                    MemoryStream stream = new MemoryStream();
                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool ReadCSVFileNoHeader(string location)
        {
            try
            {
                var result = false;
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                };
                using (var reader = new StreamReader(location))
                using (var csv = new CsvReader(reader, config))
                {
                    csv.Context.RegisterClassMap<CourseIndexMap>();
                    var records = csv.GetRecords<Course>();
                    result = InsertToDataBase(records.ToList());

                }
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private bool InsertToDataBase(List<Course> input)
        {
            try
            {
                var listCourse = new List<Course>();
                foreach (var item in input)
                {
                    var entity = new Course()
                    {
                        Author = item.Author,
                        Name = item.Name,
                        TotalHour = item.TotalHour
                    };
                    listCourse.Add(entity);
                }
                var result = _unitOfWork.CourseRepository.InsertBulk(listCourse);
                _unitOfWork.SaveChange();
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public StringBuilder WriteCSVFile(List<Course> input)
        {
            try
            {
                var builder = new StringBuilder();
                builder.AppendLine("Id,Name,Author,TotalHour");
                foreach (var item in input)
                {
                    builder.AppendLine($"{item.CourseNo},{item.Name},{item.Author},{item.TotalHour}");
                }
                return builder;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

       private string TemplateGenerator(List<Course> input)
        {
            try
            {
                var builder = new StringBuilder();

                builder.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <h1>INFORMATION ABOUT COURSE 2022</h2>
                                <table align='center'>
                                    <tr>
                                        <th> Id </th>
                                        <th> Name </th>
                                        <th> Athour </th>
                                        <th> TotalHour </th>
                                    </tr>");
                var total = 0;
                foreach (var item in input)
                {
                    total = total + item.TotalHour;
                    builder.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>", item.CourseNo, item.Name, item.Author, item.TotalHour);
                    
                }

                builder.AppendFormat(@"<br/><tr>
                                    <td>Total Hour: </td>
                                    <td></td>
                                    <td></td>
                                    <td>{0}</td>
                                  </tr>",total.ToString("N2"));

                builder.Append(@"</table>
                            </body>
                        </html>");

                builder.Append(@"<div class='footter-content'><b>Ghi chú/Note: </b> <br/> 
                    <b>Thông tin lương/thu nhập là thông tin Confidential, đề nghị các anh/chị không chia sẻ thông tin với những người khác ngoại trừ
                    quản lý trực tiếp hoặc nhân sự/ </b> 
                Salary/income is Confidential Information, Please do not share information with others except for the direct management or HR.<br/><br/>
                <b>Mọi thắc mắc về thông tin lương, anh/chị vui lòng liên hệ với HRBP để được giải đáp/</b><br/>
                Any questions about your salary, please contact HRBP to be answered.<br/>
                <p>
                       <b>Please Informed</b><br/>
                           <span class ='mail'>Abc@fsoft.com.vn</span>
                </p>
                </div>
                ");
                
                return builder.ToString();
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public byte[] WritePdfFile(List<Course> input)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report"
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator(input),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            try
            {
                var file = _converter.Convert(pdf);
                return file;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}
