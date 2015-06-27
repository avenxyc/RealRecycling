﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Recycling.Domain.Models;

namespace Recycling
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AutoMapper.Mapper.CreateMap<Project, Project>()
            //   .ForMember(d => d.UserProjects, o => o.Ignore())
            //   ;

            //AutoMapper.Mapper.CreateMap<User, User>()
            //  .ForMember(d => d.UserProjects, o => o.Ignore())
            //  .ForMember(d => d.FileLogs, o => o.Ignore())
            //  ;


            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
