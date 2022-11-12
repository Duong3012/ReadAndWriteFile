using DemoProject.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoProject.BUL.Interface
{
    public interface IJobService 
    {
        IEnumerable<Job> GetList();
        bool Insert(Job obj);

        bool ReadFileXlsToDatatable(string filePath);

        public byte[] WriteDocxFile(List<Job> input);
    }
}
