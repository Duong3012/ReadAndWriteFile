using System;
using System.Collections.Generic;
using System.Text;

namespace DemoProject.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string JobName { get; set; }

        public DateTime StartJob { get; set; }
        public DateTime EndJob { get; set; }

        public string Description { get; set; }

        public Job()
        {

        }
    }
}
