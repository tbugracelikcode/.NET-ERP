using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Caching;
using Tsi.Core.Utilities.Interceptors;
using Tsi.Core.Utilities.IoC;

namespace Tsi.Core.Aspects.Autofac.Caching
{
    public class CacheAspectWithRemove : MethodInterception
    {
        private int _duration;
        private ICacheManager _cacheManager;
        private string _pattern;

        public CacheAspectWithRemove(int duration = 60, string pattern=null)
        {
            _duration = duration;
            _pattern = pattern;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        public override void Intercept(IInvocation invocation)
        {

            _cacheManager.RemoveByPattern(_pattern);

            var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}");
            var arguments = invocation.Arguments.ToList();
            var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})";
            if (_cacheManager.IsAdd(key))
            {
                invocation.ReturnValue = _cacheManager.Get(key);
                return;
            }
            invocation.Proceed();
            _cacheManager.Add(key, invocation.ReturnValue, _duration);
        }
    }
}
