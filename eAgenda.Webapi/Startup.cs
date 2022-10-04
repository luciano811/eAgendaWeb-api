using eAgenda.Aplicacao.ModuloTarefa;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Dominio;
using eAgenda.Infra.Configs;
using eAgenda.Infra.Orm.ModuloTarefa;
using eAgenda.Infra.Orm;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eAgenda.Webapi.Filters;
using eAgenda.Aplicacao.ModuloContato;
using eAgenda.Infra.Orm.ModuloContato;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Aplicacao.ModuloCompromisso;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Infra.Orm.ModuloCompromisso;
using Microsoft.OpenApi.Any;
using eAgenda.Aplicacao.ModuloDespesa;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Infra.Orm.ModuloDespesa;
using eAgenda.Webapi.Config.AutoMapperConfig.ModuloCompromisso;
using eAgenda.Webapi.Config.AutoMapperConfig.ModuloContato;
using eAgenda.Webapi.Config.AutoMapperConfig.ModuloDespesa;
using eAgenda.Webapi.Config.AutoMapperConfig.ModuloTarefa;
using eAgenda.Webapi.Config.AutoMapperConfig.ModuloCategoria;
using eAgenda.Webapi.Config.AutoMapperConfig;
using Microsoft.AspNetCore.Identity;
using eAgenda.Dominio.ModuloAutenticacao;
using eAgenda.Aplicacao.ModuloAutenticacao;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace eAgenda.Webapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(config =>
            {
                //pra suprimir traceid, status e umns blablabla do retorno da request
                config.SuppressModelStateInvalidFilter = true;
            });

            services.AddAutoMapper(config =>
            {
                 config.AddProfile <TarefaProfile> ();
                 config.AddProfile <ContatoProfile> ();
                 config.AddProfile <CompromissoProfile> ();
                 config.AddProfile <DespesaProfile> ();
                 config.AddProfile <CategoriaProfile> ();
                 config.AddProfile<UsuarioProfile>();
            });

            services.AddSingleton((x) => new ConfiguracaoAplicacaoeAgenda().ConnectionStrings);

            services.AddScoped<eAgendaDbContext>();

            services.AddIdentity<Usuario, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<eAgendaDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<UserManager<Usuario>>();
            services.AddTransient<SignInManager<Usuario>>();

            services.AddScoped<IContextoPersistencia, eAgendaDbContext>();
            services.AddScoped<IRepositorioTarefa, RepositorioTarefaOrm>();
            services.AddScoped<IRepositorioContato, RepositorioContatoOrm>();
            services.AddScoped<IRepositorioCompromisso, RepositorioCompromissoOrm>();
            services.AddScoped<IRepositorioDespesa, RepositorioDespesaOrm>();
            services.AddScoped<IRepositorioCategoria, RepositorioCategoriaOrm>();
            services.AddTransient<ServicoTarefa>();
            services.AddTransient<ServicoContato>();
            services.AddTransient<ServicoCompromisso>();
            services.AddTransient<ServicoDespesa>();
            services.AddTransient<ServicoCategoria>();
            services.AddTransient<ServicoAutenticacao>();

            services.AddControllers(config =>
            {
                config.Filters.Add(new ValidarViewModelActionFilter());
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "eAgenda.Webapi", Version = "v1" });
                c.MapType<TimeSpan>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("00:00:00")
                });

                var key = Encoding.ASCII.GetBytes("SegredoSuperSecretoDoeAgenda");

                services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                }).AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;

                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidAudience = "http://localhost",
                        ValidIssuer = "eAgenda"
                    };
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "eAgenda.Webapi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
