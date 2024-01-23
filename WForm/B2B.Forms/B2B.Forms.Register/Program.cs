using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace B2B.Forms.Register
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Inicializa a configura��o da aplica��o
            ApplicationConfiguration.Initialize();

            // Configura��o de servi�os e obten��o do ServiceProvider
            var serviceProvider = ConfigureServices();

            // Obt�m inst�ncias necess�rias para passar para RegisterForm
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var context = serviceProvider.GetRequiredService<IdentityDataContext>();
            var registerService = new RegisterService(userManager);

            // Inicia o formul�rio de registro
            Application.Run(new RegisterForm(registerService, context));
        }

        // Configura��o de servi�os da aplica��o
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Adiciona logging
            services.AddLogging();

            // Configura��o do banco de dados
            services.AddDbContext<IdentityDataContext>(options =>
                options.UseSqlServer("Data Source=NOOTEBOOKOTAVIO;Initial Catalog=B2B;User Id=Otavio;Password=Otavio@pis123;TrustServerCertificate=True;encrypt=false")); // Substitua pela sua string de conex�o

            // Configura��o do Identity
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<IdentityDataContext>(); // Ajuste para o contexto correto
            return services.BuildServiceProvider();
        }
    }
}
