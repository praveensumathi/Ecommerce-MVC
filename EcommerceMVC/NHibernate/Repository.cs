using NHibernate;
using System.Collections.Generic;

namespace EcommerceMVC.NHibernate
{
    public class Repository<T> where T : class
    {
        private readonly ISession _session;

        protected ISession Session { get { return _session; } }

        public Repository(ISession session)
        {
            _session = session;
        }

        public IEnumerable<T> GetAll()
        {
            return _session.Query<T>();
        }

        public T GetById(object id)
        {
            return _session.Get<T>(id);
        }

        public T GetById(long id)
        {
            return _session.Get<T>(id);
        }

        public void Create(T entity)
        {
            _session.Save(entity);
            _session.Flush();
        }

        public void Update(T entity)
        {
            _session.Update(entity);
            _session.Flush();
        }

        public void Merge(T entity)
        {
            _session.Merge(entity);
            _session.Flush();
        }

        public void Delete(T entity)
        {
            _session.Delete(entity);
            _session.Flush();
        }

        public void CreateOrUpdate(T entity)
        {
            _session.SaveOrUpdate(entity);
            _session.Flush();
        }
    }
}
