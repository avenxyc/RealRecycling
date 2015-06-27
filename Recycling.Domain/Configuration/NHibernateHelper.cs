using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using Recycling.Domain.Models;

namespace Recycling.Nhibernate
{
    public class StringColumnLengthConvention : IPropertyConvention, IPropertyConventionAcceptance
    {
        private const int NVARCHARMAX = 4001;
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => x.Type == typeof(string)).Expect(x => x.Length == 0);
        }
        public void Apply(IPropertyInstance instance)
        {
            instance.Length(NVARCHARMAX);
        }
    }

    public static class NHibernateHelper
    {
        private const string SerializedConfiguration = "nhibernateConfiguration.serialized";
        private static bool IsConfigurationFileValid
        {
            get
            {
                var ass = System.Reflection.Assembly.GetAssembly(typeof(User));
                if (ass.Location == null)
                    return false;
                var configInfo = new FileInfo(SerializedConfiguration);
                var assInfo = new FileInfo(ass.Location);
                if (configInfo.LastWriteTime < assInfo.LastWriteTime)
                    return false;
                return true;
            }
        }

        public static NHibernate.Cfg.Configuration Configuration
        {
            get { return CreateOrLoadConfiguration(); }
        }

        private static NHibernate.Cfg.Configuration LoadConfigurationFromFile()
        {
            if (IsConfigurationFileValid == false)
                return null;
            try
            {
                using (var file = System.IO.File.Open(SerializedConfiguration, FileMode.Open))
                {
                    var bf = new BinaryFormatter();
                    return bf.Deserialize(file) as NHibernate.Cfg.Configuration;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static void SaveConfigurationToFile(NHibernate.Cfg.Configuration configuration)
        {
            using (var file = System.IO.File.Open(SerializedConfiguration, FileMode.Create))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(file, configuration);
            }
        }

        private static NHibernate.Cfg.Configuration FluentlyConfigure()
        {
            try
            {
                var mappings = AutoMap.AssemblyOf<User>()//.
                    .Where(x => typeof(DbEntity).IsAssignableFrom(x));

                var cfg = Fluently.Configure().CurrentSessionContext("web").Database(MsSqlConfiguration.MsSql2008.ConnectionString(x =>
                {
                    x.FromConnectionStringWithKey("default");
                }))
                .Mappings(m =>
                {
                    mappings.Conventions.Add(DefaultCascade.None());
                    mappings.IgnoreBase<DbEntity>();
                    mappings.Conventions.Add<StringColumnLengthConvention>();
                    m.AutoMappings.Add(mappings);
                })
                .BuildConfiguration()/*.DataBaseIntegration(
            db =>
            {
                db.LogFormattedSql = true;
                db.LogSqlInConsole = true;
                db.AutoCommentSql = true;
            })*/;
                return cfg;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private static NHibernate.Cfg.Configuration CreateOrLoadConfiguration()
        {
            var configuration = FluentlyConfigure();
            return configuration;
        }
    }
}