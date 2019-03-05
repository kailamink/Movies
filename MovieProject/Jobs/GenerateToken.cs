using FluentScheduler;
using MovieProject.Jobs.JobUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace MovieProject.Jobs
{
    public class GenerateToken : IJob, IRegisteredObject
    {
        public GenerateToken()
        {
            HostingEnvironment.RegisterObject(this);
        }
        public void Execute()
        {
            var movieAPIUtils = new MovieAPIUtils();
            //movieAPIUtils.generateToken();

        }

        public void Stop(bool immediate)
        {
            throw new NotImplementedException();
        }
    }
}