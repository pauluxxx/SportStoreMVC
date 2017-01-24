using Moq;
using Ninject;
using System;
using SportsStore.Domain.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Concrete;
using System.Configuration;
using SportsStore.WebUI.Infrastructure.Abstract;
using SportsStore.WebUI.Infrastructure.Concrete;
namespace SportsStore.WebUI.Infrastructure
{
    public class NinjectControllerFactory: DefaultControllerFactory
    {
        private IKernel kernel;
        public NinjectControllerFactory() {
            kernel = new StandardKernel();
            AddBindings();
        }
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return controllerType == null
                ? null
                : (IController)kernel.Get(controllerType);
        } 

        private void AddBindings()
        {//simulate repository/*
 /*          Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product> {
                new Product{Name = "Football",Price = 25m},
                new Product{Name = "Surf board",Price = 179m},
                new Product{Name = "Runnig shoes",Price = 95m},
            }.AsQueryable());*/
            //reall repo on sqlserver on (localdb)\v11.9
            kernel.Bind<IProductRepository>().To<EFProductRespository>();
            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings",emailSettings);
            kernel.Bind<IAuthProvider>().To<FormAuthProvider>();
        }
    }
}