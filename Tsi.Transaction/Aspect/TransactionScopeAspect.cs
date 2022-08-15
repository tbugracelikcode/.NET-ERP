﻿using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Tsi.IoC.IoC.Autofac.Interceptors;

namespace Tsi.Transaction.Aspect
{
    public class TransactionScopeAspect : MethodInterception
    {
        public override void Intercept(IInvocation invocation)
        {
            TransactionScope transactionScope = new TransactionScope();

            try
            {
                invocation.Proceed();
                transactionScope.Complete();
            }
            catch (System.Exception e)
            {
                transactionScope.Dispose();
                throw new Exception();
            }

        }
    }
}
