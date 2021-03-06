﻿using andboard.Core.Entities;
using andboard.Core.Interfaces;
using andboard.Core.Models;
using System.Linq;

namespace andboard.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        public readonly IContext Context;
        public IQueryable<T> Query { get; private set; }

        public Repository(IContext context)
        {
            Context = context;
            Query = context.Query<T>();
        }

        public IResult<T> Create(T obj)
        {
            var result = new Result<T>();
            Context.Add(obj);
            var totalRecordsSaved = Context.SaveChanges();
            result.HasErrors = totalRecordsSaved <= 0;
            result.ResultObject = obj;
            return result;
        }

        public IResult<IQueryable<T>> FindAll()
        {
            var result = new Result<IQueryable<T>>();
            result.ResultObject = Context.Query<T>();
            return result;
        }

        public IResult<T> FindById(long id)
        {
            var result = new Result<T>();
            result.ResultObject = Query.FirstOrDefault(e => e.Id == id);
            return result;
        }

        public IResult<T> Update(T obj)
        {
            var entity = FindById(obj.Id);
            entity.ResultObject = obj;
            Context.Update(entity);
            return entity;
        }
    }
}
