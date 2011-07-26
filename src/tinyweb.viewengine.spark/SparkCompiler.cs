﻿using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.IO;
using Spark;
using Spark.FileSystem;

namespace tinyweb.viewengine.spark
{
    public static class SparkCompiler
    {
        static ConcurrentDictionary<string, ISparkView> cache = new ConcurrentDictionary<string, ISparkView>();

        public static string Compile<T>(T model, string templatesPath, string templateName, string master)
        {
            var fullTemplatePath = Path.Combine(templatesPath, templateName);
            var templateFilename = Path.GetFileName(fullTemplatePath);

            var sparkEngine = new SparkViewEngine
            {
                ViewFolder = new FileSystemViewFolder(templatesPath),
                DefaultPageBaseType = typeof(SparkView).FullName
            };

            if (ConfigurationManager.GetSection("spark") == null)
            {
                sparkEngine.Settings = new SparkSettings()
                    .AddNamespace("System")
                    .AddNamespace("System.Collections.Generic")
                    .AddNamespace("System.Linq")
                    .AddNamespace("tinyweb.framework")
                    .AddNamespace("tinyweb.framework.Helpers");
            }

            var descriptor = new SparkViewDescriptor().AddTemplate(templateFilename);

            if (!String.IsNullOrEmpty(master))
            {
                descriptor.AddTemplate(master);
            }

            ISparkView view;
            var key = fullTemplatePath + master;
 
            if (!cache.ContainsKey(key))
            {
                view = sparkEngine.CreateInstance(descriptor);
                cache[key] = view;
            }
            else
            {
                view = cache[key];
            }

            if (model != null)
            {
                (view as SparkView<T>).Model = model;
            }

            var output = new StringWriter();
            view.RenderView(output);
            sparkEngine.ReleaseInstance(view);
            
            return output.ToString();
        }
    }
}