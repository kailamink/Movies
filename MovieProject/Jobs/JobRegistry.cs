using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieProject.Jobs
{
    public class JobRegistry : Registry
    {
        public JobRegistry()
        {
            Schedule<GenerateToken>().ToRunNow();
        }
    }
}