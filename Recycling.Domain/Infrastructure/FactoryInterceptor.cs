using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using StructureMap;

namespace Recycling.Domain.Infrastructure
{
    public class FactoryInterceptor : IInterceptor
    {
        private readonly IContext _context;



        public FactoryInterceptor(IContext context)
        {
            _context = context;
        }

        public void Intercept(IInvocation invocation)
        {
            Type pluginType;
            var container = _context;

            if ((invocation.Arguments.Length > 0) && (invocation.Arguments[0] is Type))
            {
                pluginType = (Type)invocation.Arguments[0];
                invocation.ReturnValue = container.GetInstance(pluginType);
            }

            pluginType = invocation.Method.ReturnType;

            if (pluginType.IsArray)
            {
                var contextGetAllInstances = container.GetAllInstances(pluginType.GetElementType());
                var lst = (IList)Activator.CreateInstance(typeof(IList<>).MakeGenericType(pluginType.GetElementType()));
                foreach (var item in contextGetAllInstances)
                    lst.Add(item);

                var arr = Array.CreateInstance(pluginType.GetElementType(), lst.Count);
                lst.CopyTo(arr, 0);
                invocation.ReturnValue = arr;
            }
            else if (pluginType.IsEnumerable())
            {
                pluginType = pluginType.GetGenericArguments().FirstOrDefault();
                var contextGetAllInstances = container.GetAllInstances(pluginType);
                var lst = (IList)Activator.CreateInstance(typeof(IList<>).MakeGenericType(pluginType.GetElementType()));
                foreach (var item in contextGetAllInstances)
                    lst.Add(item);
                var arr = Array.CreateInstance(pluginType, lst.Count);
                lst.CopyTo(arr, 0);
                invocation.ReturnValue = arr;
            }
            else
            {                
                invocation.ReturnValue = container.GetInstance(pluginType);
            }
        }
    }
}
