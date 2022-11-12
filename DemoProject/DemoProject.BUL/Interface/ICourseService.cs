using DemoProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DemoProject.BUL.Interface
{
    public interface ICourseService
    {
        IEnumerable<Course> GetList();
        bool Insert(Course obj);

        public bool ReadCSVFile(string location);

        public bool ReadCSVFileNoHeader(string location);

        public StringBuilder WriteCSVFile(List<Course> input);

        public Byte[] WritePdfFile(List<Course> input);
       // public Byte[] PdfSharpConvert(String html);
    }
}
