using System;
using System.Web.Mvc;
using BooksEditor.Models.Context;
using BooksEditor.Models.Abstract;
using System.Web.Routing;
using Ninject;

namespace BooksEditor.Models.Infrastructure
{
    public class NinjectContollerFactory : DefaultControllerFactory
    {
        private IKernel _ninjectKernel;

        public NinjectContollerFactory()
        {
            _ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)_ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            _ninjectKernel.Bind<IBookContainer>().To<EFBookContainer>();
        }
    }
}