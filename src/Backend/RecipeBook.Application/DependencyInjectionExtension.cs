using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Application.Cryptography;
using RecipeBook.Application.Services.AutoMapper;
using RecipeBook.Application.UseCases.Login.DoLogin;
using RecipeBook.Application.UseCases.User.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            AddUseCases(services);
            AddAutoMapper(services);
            AddPasswordEncrypter(services);
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            var mapper = new AutoMapper.MapperConfiguration(cfg => cfg.AddProfile<AutoMapping>()).CreateMapper();
            services.AddScoped(option => mapper);


        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        }

        private static void AddPasswordEncrypter(IServiceCollection services)
        {
            services.AddScoped(option => new PasswordEncripter());
        }
    }
}
