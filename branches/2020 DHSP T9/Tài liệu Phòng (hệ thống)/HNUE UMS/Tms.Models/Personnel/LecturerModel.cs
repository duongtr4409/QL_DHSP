using System.Collections.Generic;
using Ums.Core.Domain.Data;
using Ums.Core.Domain.Personnel;
using Ums.Core.Domain.Task;

namespace Ums.Models.Personnel
{
    public class LecturerTasks
    {
        public List<TaskTeaching> Teachings { get; set; }
        public List<TaskResearching> Researchings { get; set; }
        public List<TaskWorking> Workings { get; set; }
        public Staff Staff { get; set; }
        public Year Year { get; set; }
    }
}