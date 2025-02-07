//using Microsoft.Extensions.DependencyInjection;
//using Autofac.Core;
//using Neatoo.Core;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Neatoo.Autofac
//{
//    /// <summary>
//    /// TODO: I think I should get rid of this class and just use .NET IServiceScope
//    /// </summary>
//    public class ServiceScope : IServiceScope
//    {
//        private IServiceScope scope { get; }
//        public ServiceScope(IServiceScope scope)
//        {
//            this.scope = scope;
//        }

//        public IServiceScope BeginNewScope(object tag = null)
//        {
//            return new ServiceScope(scope.BeginLifetimeScope(tag));
//        }

//        public T Resolve<T>()
//        {
//            return scope.GetRequiredService<T>();
//        }

//        public object Resolve(Type t)
//        {
//            return scope.GetRequiredService(t);
//        }

//        public bool TryResolve<T>(out T result) where T : class
//        {
//            return scope.TryResolve<T>(out result);
//        }

//        public bool TryResolve(Type T, out object result)
//        {
//            return scope.TryResolve(T, out result);
//        }

//        public bool IsRegistered(Type type)
//        {
//            return scope.IsRegistered(type);
//        }

//        public bool IsRegistered<T>()
//        {
//            return scope.IsRegistered<T>();
//        }

//        public Type ConcreteType<T>()
//        {
//            return ConcreteType(typeof(T));
//        }

//        public Type ConcreteType(Type t)
//        {
//            IComponentRegistration registration = scope.ComponentRegistry.RegistrationsFor(new TypedService(t)).FirstOrDefault();

//            if (registration != null)
//            {
//                return registration.Activator.LimitType;
//            }
//            return null;
//        }

//        public void Dispose()
//        {
//            scope.Dispose();
//        }
//    }
//}
