﻿using System;
using System.Web.Routing;

namespace tinyweb.framework.tests
{
    public class CustomFilterInvoker : IFilterInvoker
    {
        public IResult RunBefore(object filter, RequestContext requestContext, HandlerData handlerData)
        {
            throw new NotImplementedException();
        }

        public IResult RunAfter(object filter, RequestContext requestContext, HandlerData handlerData)
        {
            throw new NotImplementedException();
        }
    }
}