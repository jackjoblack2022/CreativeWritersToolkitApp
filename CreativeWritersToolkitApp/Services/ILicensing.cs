using CreativeWritersToolkitApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreativeWritersToolkitApp.Services
{
    public interface ILicensing
    {
        bool IsActive(License license);
    }
}
