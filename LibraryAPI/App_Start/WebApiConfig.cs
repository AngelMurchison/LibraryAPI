﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using LibraryAPI.Models;
using LibraryAPI.Controller;

namespace LibraryAPI.View
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "LibraryAPI",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
