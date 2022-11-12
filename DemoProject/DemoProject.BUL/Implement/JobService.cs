using ClosedXML.Excel;
using DemoProject.BUL.Interface;
using DemoProject.DAL.Interface.UnitOfWork;
using DemoProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using SautinSoft.Document;
using System.IO;

namespace DemoProject.BUL.Implement
{
    public class JobService : IJobService
    {
        private IUnitOfWork _unitOfWork;
        public JobService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<Job> GetList()
        {
            try
            {
                return _unitOfWork.JobRepository.GetList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Insert(Job obj)
        {
            try
            {
                if (obj == null)
                {
                    return false;
                }
                var entity = _unitOfWork.JobRepository.InsertObject(obj);
                return entity;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool ReadFileXlsToDatatable(string filePath)
        {
            // Open the Excel file using ClosedXML.
            // Keep in mind the Excel file cannot be open when trying to read it
            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet(1);

                //Create a new DataTable.
                DataTable dt = new DataTable();

                //Loop through the Worksheet rows.
                bool firstRow = true;
                foreach (IXLRow row in workSheet.Rows())
                {
                    //Use the first row to add columns to DataTable.
                    if (firstRow)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                    }
                    else
                    {
                        //Add rows to DataTable.
                        dt.Rows.Add();
                        int i = 0;

                        foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                        }
                    }
                }
                var result = InsertDataToDataBase(dt);
                return result;
            }
        }

        private bool InsertDataToDataBase(DataTable dt)
        {
            if (dt.Rows != null)
            {
                var listJob = (from DataRow d in dt.Rows
                               select new Job()
                               {
                                   JobName = d["JobName"].ToString(),
                                   StartJob = Convert.ToDateTime(d["StartJob"].ToString()),
                                   EndJob = Convert.ToDateTime(d["EndJob"].ToString()),
                                   Description = d["Description"].ToString()
                               }).ToList();
                #region Insert for each employee
                //foreach (var item in listEmployee)
                //{
                //    _unitOfWork.EmployeeRepository.InsertObject(item);
                //    _unitOfWork.SaveChange();
                //}
                #endregion

                ///Insert many employee
                var reusult = _unitOfWork.JobRepository.InsertBulk(listJob);
                _unitOfWork.SaveChange();
                return reusult;
            }
            return false;
        }

        private string TemplateGeneratorDocx(List<Job> input)
        {
            try
            {
                var builder = new StringBuilder();

                builder.Append(@"
                                <h2 style = 'text-align: center; color : green;'>INFORMATION ABOUT COURSE 2022</h2>
                                <table align='center' class ='table table-bordered' width = '100%'>
                                    <tr style = 'background-color:green;font-size : 16px; color:white;height:15px'>
                                        <th style = 'border: 1px solid gray;padding: 2px; font - size: 22px;text - align: center;'> Id </th>
                                        <th style = 'border: 1px solid gray;padding: 2px; font - size: 22px;text - align: center;'> JobName </th>
                                        <th style = 'border: 1px solid gray;padding: 2px; font - size: 22px;text - align: center;'> StartDate </th>
                                        <th style = 'border: 1px solid gray;padding: 2px; font - size: 22px;text - align: center;'> EndDate </th>
                                        <th style = 'border: 1px solid gray;padding: 2px; font - size: 22px;text - align: center;'> Description </th>
                                    </tr>");

                foreach (var item in input)
                {

                    builder.AppendFormat(@"<tr style ='font-size : 15px;'>
                                    <td style = 'border: 1px solid gray;padding: 2px; font - size: 16px;text - align: center;line-height :10px'>{0}</td>
                                    <td style = 'border: 1px solid gray;padding: 2px; font - size: 16px;text - align: center;'>{1}</td>
                                    <td style = 'border: 1px solid gray;padding: 2px; font - size: 16px;text - align: center;'>{2}</td>
                                    <td style = 'border: 1px solid gray;padding: 2px; font - size: 16px;text - align: center;'>{3}</td>
                                    <td style = 'border: 1px solid gray;padding: 2px; font - size: 16px;text - align: center;'>{4}</td>
                                  </tr>", item.Id, item.JobName, item.StartJob, item.EndJob, item.Description);

                }

                builder.Append(@"</table>");

                builder.Append(@"<div class='footter-content' style = 'margin-top:60px;'><b>Ghi chú/Note: </b> <br/> 
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


        public byte[] WriteDocxFile(List<Job> input)
        {
            try
            {

                var result = new Byte[] { };
                var file = Encoding.UTF8.GetBytes(TemplateGeneratorDocx(input));
                using (MemoryStream oMemoryStream = new MemoryStream(file))
                {
                    DocumentCore oDocumentCore = DocumentCore.Load(oMemoryStream, new HtmlLoadOptions() { });
                    using (MemoryStream ms = new MemoryStream())
                    {
                        oDocumentCore.Save(ms, new DocxSaveOptions());
                        result = ms.ToArray();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
