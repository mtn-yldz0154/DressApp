using DressApp.WebUi.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DressApp.WebUi.Repository
{
    public class GenericRepository<T> where T : class, new()
    {
        Context db= new Context();
        public List<T> TList()
        {
            return db.Set<T>().ToList();
        }

        public void TAdd(T t)
        {
            db.Set<T>().Add(t);
            db.SaveChanges();
        }
        
        public void TDelete(T t)
        {
            db.Set<T>().Remove(t);
            db.SaveChanges();
        }

        public virtual void TUpdate(T t)
        {
            db.Set<T>().Update(t);
            db.SaveChanges();
        }

        public T TGet(int id)
        {
            return db.Set<T>().Find(id);
        }

        public T TGetUserId(string id)
        {
            return db.Set<T>().Find(id);
        }
        public List<T> TList(string ryn)
        {
            return db.Set<T>().Include(ryn).ToList();
        }
        public List<T> List(Expression<Func<T,bool>> filter )
        {
            return db.Set<T>().Where(filter).ToList();
        }

        

       
    }
}
