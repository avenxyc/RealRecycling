using System;
using System.Linq;
using StructureMap;
using StructureMap.Configuration.DSL.Expressions;
using System.Collections;
using Castle.DynamicProxy;
using System.Linq.Expressions;

namespace Recycling.Domain.Infrastructure
{
    //this is temp, should include immediac.frameworks module
    public static class AutoFactoryExtensions
    {
        private static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();

        public static void CreateFactory<TPluginType>(this CreatePluginFamilyExpression<TPluginType> expression)
            where TPluginType : class
        {
            var callback = CreateFactoryCallback<TPluginType>();

            //expression.Use("AutoFactory builder for " + typeof(TPluginType).GetFullName(), callback);
            expression.Use(callback);
        }

        private static Expression<Func<IContext, TPluginType>> CreateFactoryCallback<TPluginType>()
            where TPluginType : class
        {
            return ctxt => new ProxyFactory<TPluginType>(proxyGenerator, ctxt).Create();            
        }
    }

    public static class Ext 
    {
        public static bool IsEnumerable(this Type type)
        {
            return (type.GetInterface("IEnumerable") != null);
        }
    }
}