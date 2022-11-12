using ClosedXML.Excel;
using DemoProject.BUL.Interface;
using DemoProject.DAL.Interface.UnitOfWork;
using DemoProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Globalization;

namespace DemoProject.BUL.Implement
{
    public class EmployeeService : IEmployeeService
    {
        private IUnitOfWork _unitOfWork;
        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public Employee GetById(string Id)
        {
           return _unitOfWork.EmployeeRepository.GetById(Id);
        }

        public IEnumerable<Employee> GetList()
        {
            return _unitOfWork.EmployeeRepository.GetList();
        }

        public bool Insert(Employee obj)
        {
            try
            {
                if(obj == null)
                {
                    return false;
                }
                var entity = _unitOfWork.EmployeeRepository.InsertObject(obj);
                return entity;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool Update(Employee obj)
        {
            try
            {
                if(obj == null)
                {
                    return false;
                }
                var entity = _unitOfWork.EmployeeRepository.GetById(obj.Id);
                entity.Name = obj.Name;
                entity.Address = obj.Address;
                entity.Email = obj.Email;
                entity.Description = obj.Description;
                _unitOfWork.EmployeeRepository.UpdateObject(entity);
                return true;
                    
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool Delete(string Id)
        {
            try
            {
                var entity = _unitOfWork.EmployeeRepository.DeleteObject(Id);
                return entity;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool ReadFiletoDatatable(string filePath)
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
                var listEmployee = (from DataRow d in dt.Rows
                                    select new Employee()
                                    {
                                        Id = d["Id"].ToString(),
                                        Name = d["Name"].ToString(),
                                        Address = d["Address"].ToString(),
                                        Email = d["Email"].ToString(),
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
                var reusult = _unitOfWork.EmployeeRepository.InsertBulk(listEmployee);
                _unitOfWork.SaveChange();
                return reusult;
            }
            return false;
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public byte[] ExportExcel(List<Employee> input)
        {
            //CultureInfo _cultureInfo = new CultureInfo("en-US");
            var currentRow = 0;
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Employees");
                currentRow += 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Name";
                worksheet.Cell(currentRow, 3).Value = "Address";
                worksheet.Cell(currentRow, 4).Value = "Email";
                worksheet.Cell(currentRow, 5).Value = "Salary";
                worksheet.Cell(currentRow, 6).Value = "Description";

                worksheet.Range(currentRow, 1, currentRow, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(currentRow, 1, currentRow, 6).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Range(currentRow, 1, currentRow, 6).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(currentRow, 1, currentRow, 6).Style.Border.InsideBorderColor = XLColor.Black;
                var salary = 20384875740;
                foreach (var item in input)
                {
                    currentRow++;

                    worksheet.Cell(currentRow, 1).SetValue(item.Id);
                    worksheet.Cell(currentRow, 2).SetValue(item.Name);
                    worksheet.Cell(currentRow, 3).SetValue(item.Address);
                    worksheet.Cell(currentRow, 4).SetValue(item.Email);
                    worksheet.Cell(currentRow, 5).SetValue((salary + currentRow).ToString("N2"));
                    worksheet.Cell(currentRow, 6).SetValue((salary+currentRow).ToString("0,0#,##"));
                    //worksheet.Cell(currentRow, 13).SetValue(item.ChargeAmount.ToString("N2"));
                }
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
