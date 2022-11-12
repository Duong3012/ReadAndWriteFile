using CsvHelper.Configuration;
using DemoProject.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoProject.BUL.MapperDTO
{
    public class CourseDto
    {
        public int CourseNo { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int TotalHour { get; set; }
    }

    public sealed class CourseMap : ClassMap<Course>
    {
        public CourseMap()
        {
            Map(m => m.CourseNo).Name("CourseNo");
            Map(m => m.Name).Name("Name");
            Map(m => m.Author).Name("Author");
            Map(m => m.TotalHour).Name("TotalHour");
        }
    }

    public sealed class CourseIndexMap : ClassMap<Course>
    {
        public CourseIndexMap()
        {
            Map(m => m.CourseNo).Index(0);
            Map(m => m.Name).Index(1);
            Map(m => m.Author).Index(2);
            Map(m => m.TotalHour).Index(3);
        }
    }
}
