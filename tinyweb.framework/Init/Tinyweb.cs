﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace tinyweb.framework
{
    public static class Tinyweb
    {
        public static IEnumerable<HandlerData> Handlers { get; private set; }

        public static int Init()
        {
            Handlers = HandlerScanner.Current.FindAll();

            Handlers.ForEach(handler => RouteTable.Routes.Add(new System.Web.Routing.Route(handler.Uri, new RouteValueDictionary(handler.DefaultRouteValues), new RouteHandler(handler))));

            return Handlers.Count();
        }

        public static int Init(Registry registry)
        {
            ObjectFactory.Initialize(x => x.AddRegistry(registry));

            return Init();
        }

        public static string WhatHaveIGot()
        {
            var report = new StringBuilder();

            foreach (var handler in Handlers.OrderBy(h => h.Uri))
            {
                report.AppendLine(String.Format("/{0} -> {1}", handler.Uri, handler.Type.Name));
            }

            return report.ToString();
        }
    }
}