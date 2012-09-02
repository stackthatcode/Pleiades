using System;
using Autofac;
using Pleiades.Execution;
using Pleiades.Injection;
using Pleiades.Web.Security.Aspect;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Execution.Step;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web
{
    public class WebModule : Module
    {        
        protected override void Load(ContainerBuilder builder)
        {
            WebInjectionBroker.Build(builder);
        }
    }
}