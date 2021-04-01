using EcommerceMVC.Models;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace EcommerceMVC.NHibernate
{
    public static class Helper
    {
        private static ISessionFactory _sessionFactory;
        private static string _connectionString;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2012.ConnectionString(_connectionString)
                    .DefaultSchema("dbo"))
                    .Mappings(m => m.FluentMappings
                        .AddFromAssemblyOf<Product>())
                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg)
                    .Execute(true, true))
                    .BuildSessionFactory();
                }

                return _sessionFactory;
            }
        }

        public static ISession OpenSession(string connectionString)
        {
            _connectionString = connectionString;
            return SessionFactory.OpenSession();
        }
    }
}
