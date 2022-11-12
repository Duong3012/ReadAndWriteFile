using DemoProject.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoProject.DAL.Interface
{
    public interface IJobRepository : IGenericReopository<Job>
    {
        bool InsertBulk(List<Job> jobs);

        bool UpdateBulk(List<Job> jobs);
    }
}
