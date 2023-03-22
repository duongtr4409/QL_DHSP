using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Core.Domain.Students;

namespace Ums.Services.Students
{
    public interface IMasterStudentService
    {
        MasterStudent GetStudent(string username);
    }
}