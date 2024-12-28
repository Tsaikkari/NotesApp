using Data.Interfaces;
using Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NotesApp.UI;
using System;
using System.Configuration;

namespace NotesApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            ServiceCollection services = ConfigureServices();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            var startForm = serviceProvider.GetRequiredService<NotesForm>();
            Application.Run(startForm);
        }

        static ServiceCollection ConfigureServices()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddTransient<INotesRepository>(_ => new NotesRepository());
            services.AddTransient<ICategoriesRepository>(_ => new CategoriesRepository());
            services.AddTransient<ISubcategoriesRepository>(_ => new SubcategoriesRepository());

            services.AddTransient<NotesForm>();
            services.AddTransient<CreateOrEditNoteForm>();
            services.AddTransient<CategoriesForm>();
            services.AddTransient<SubcategoriesForm>();
                
            return services;
        }
    }
}