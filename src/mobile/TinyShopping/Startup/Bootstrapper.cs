using System;
using System.Reflection;
using Autofac;
using TinyCache;
using TinyMvvm.Autofac;
using TinyMvvm.IoC;
using TinyNavigationHelper;
using TinyPubSubLib;

namespace TinyShopping
{
    public static class Bootstrapper
    {
        public static void Initialize(App app)
        {
            var builder = new ContainerBuilder();

            // Views
            var ass = app.GetType().Assembly;
            builder.RegisterAssemblyTypes(ass)
                   .Where(x => x.Name.EndsWith("View", StringComparison.Ordinal));

            // ViewModels
            builder.RegisterAssemblyTypes(ass)
                   .Where(x => x.Name.EndsWith("ViewModel", StringComparison.Ordinal));

            builder.RegisterAssemblyTypes(typeof(Core.services.ShoppingService).Assembly)
                   .Where(x => x.Name.EndsWith("Service", StringComparison.Ordinal));

            // Navigation
            var navigationHelper = new TinyNavigationHelper.Forms.FormsNavigationHelper(app);
            navigationHelper.RegisterViewsInAssembly(ass);
            builder.RegisterInstance<INavigationHelper>(navigationHelper);

            // Build and set
            var container = builder.Build();
            var resolver = new AutofacResolver(container);
            Resolver.SetResolver(resolver);

            // Init cache and settings
            TinyCache.TinyCache.SetCacheStore(new XamarinPropertyStorage());
            var cacheFirstPolicy = new TinyCachePolicy().SetMode(TinyCacheModeEnum.FetchFirst).SetFetchTimeout(600);
            TinyCache.TinyCache.SetBasePolicy(cacheFirstPolicy);

            // Init TinyMvvm
            TinyMvvm.Forms.TinyMvvm.Initialize();

            var asm = typeof(App).GetTypeInfo().Assembly;
            navigationHelper.RegisterViewsInAssembly(asm);

            // Init TinyPubSub
            TinyPubSubLib.TinyPubSubForms.Init(app);

            // Platform specifics
            Platform?.Initialize(app, builder);
        }

        public static IBootstrapper Platform { get; set; }
    }

    public interface IBootstrapper
    {
        void Initialize(App app, ContainerBuilder builder);
    }
}
