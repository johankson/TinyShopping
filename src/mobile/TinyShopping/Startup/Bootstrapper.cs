using System;
using System.Reflection;
using Autofac;
using TinyCache;
using TinyEditor.Controls;
using TinyMvvm.Autofac;
using TinyMvvm.IoC;
using TinyNavigationHelper;
using TinyPubSubLib;
using TinyTranslation.Forms;

namespace TinyShopping
{
    public class CustomTranslationClient : TinyTranslation.TranslationClient
    {
        public CustomTranslationClient() : base(new System.Uri("http://tinytranslation.azurewebsites.net"), "f17528d1-0dd0-4181-90b8-0853c62178a9")
        {

        }

        public override System.Net.Http.HttpClient GetHttpClient()
        {
            var cli = base.GetHttpClient();
            if (!cli.DefaultRequestHeaders.Contains("apikey"))
                cli.DefaultRequestHeaders.Add("apikey", "f17528d1-0dd0-4181-90b8-0853c62178a9");
            Console.WriteLine("Got httpclient");
            return cli;
        }
    }

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

            builder.RegisterAssemblyTypes(typeof(Core.Services.ShoppingService).Assembly)
                   .Where(x => x.Name.EndsWith("Service", StringComparison.Ordinal)).SingleInstance();

            // Navigation
            var navigationHelper = new TinyNavigationHelper.Forms.FormsNavigationHelper(app);
            navigationHelper.RegisterViewsInAssembly(ass);
            builder.RegisterInstance<INavigationHelper>(navigationHelper);

            // Setup translation backend https://github.com/TinyStuff/TinyTranslation for translation backend
            var translator = new TranslationHelper(new CustomTranslationClient());
            //var translator = new TranslationHelper(new System.Uri("http://tinytranslation.azurewebsites.net"), "f17528d1-0dd0-4181-90b8-0853c62178a9");

            ObjectEditor.Translate = (string arg) => {
                Console.WriteLine("Translate from ObjectEditor");
                return translator.Translate(arg);
            };

            // Register translator
            builder.Register<TranslationHelper>((a) =>
            {
                return translator;
            }).SingleInstance();

            // Build and set
            var container = builder.Build();
            var resolver = new AutofacResolver(container);
            Resolver.SetResolver(resolver);

            // Init cache and settings
            TinyCache.TinyCache.SetCacheStore(new XamarinPropertyStorage());
            var cacheFirstPolicy = new TinyCachePolicy().SetMode(TinyCacheModeEnum.FetchFirst).SetFetchTimeout(1600);
            TinyCache.TinyCache.SetBasePolicy(cacheFirstPolicy);

            //// Enable cache for translations
            //var oldMethod = translator.FetchLanguageMethod;
            //translator.FetchLanguageMethod = async (locale) => await TinyCache.TinyCache.RunAsync<TinyTranslation.TranslationDictionary>("trans-" + locale, async () =>
            //{
            //    return await oldMethod(locale);
            //}, new TinyCachePolicy() {
            //    Mode = TinyCacheModeEnum.CacheFirst
            //});

            // Set translator to markup extension
            ansExtension.Translator = translator;

            var lng = translator.CurrentLocale;
            translator.Init("sv");

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
