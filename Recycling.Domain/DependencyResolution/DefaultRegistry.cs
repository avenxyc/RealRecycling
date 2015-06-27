// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Recycling.Domain.DependencyResolution
{
    using NHibernate;
    using Recycling.Domain.Repository;
    using SharpArch.NHibernate;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
    using StructureMap.Web.Pipeline;
    using Recycling.Nhibernate;
    using Recycling.Domain.Services;
    using Recycling.Domain.Services.Impl;
    using Recycling.Domain.Infrastructure;
    public class DomainDefaultRegistry : Registry
    {
        #region Constructors and Destructors

        public DomainDefaultRegistry()
        {
            //For<IExample>().Use<Example>();
            var cfg = this;

            cfg.For<NHibernate.Cfg.Configuration>()
                .Singleton()
                .Use(x => NHibernateHelper.Configuration);

            cfg.For<ISessionFactory>()
                .Singleton()
                .Use(x => x.GetInstance<NHibernate.Cfg.Configuration>().BuildSessionFactory());

            cfg.For<ISession>()
                .LifecycleIs<HttpContextLifecycle>()
                //.HybridHttpOrThreadLocalScoped()
                .Use(context => context.GetInstance<ISessionFactory>().OpenSession());

            cfg.For<ISessionFactoryKeyProvider>().Use<DefaultSessionFactoryKeyProvider>().Named("sessionFactoryKeyProvider");

            cfg.For<ISessionProvider>().CreateFactory();
            cfg.For<IUnitOfWorkFactory>().CreateFactory();
            cfg.For<IUnitOfWork>().Use<UnitOfWork>();

            Scan(
               scan =>
               {
                   scan.AssemblyContainingType(typeof(IRepository<>));
                   scan.WithDefaultConventions();
               });
        }

        #endregion
    }
}