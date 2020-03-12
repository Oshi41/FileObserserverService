using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleDI.DI
{
    public abstract class DependencyInjectionBase : IDisposable
    {
        private readonly IServiceCollection _collection;
        private readonly IConfigurationBuilder _builder;
        private ServiceProvider _provider;

        protected DependencyInjectionBase(IServiceCollection collection, IConfigurationBuilder builder)
        {
            _collection = collection;
            _builder = builder;
        }

        /// <summary>
        /// Начальная инициализация приложения
        /// </summary>
        /// <param name="collection">Коллекция для сервивос</param>
        /// <param name="builder">Сервис построения приложения</param>
        protected abstract void Init(IServiceCollection collection, IConfigurationBuilder builder);
        
        /// <summary>
        /// Применяем настройки приложения
        /// </summary>
        /// <param name="collection">Коллекция сервисов</param>
        /// <param name="settings">Насройки приложения</param>
        protected abstract void ApplySettings(IServiceCollection collection, IConfigurationRoot settings);

        public virtual void Start()
        {
            Init(_collection, _builder);

            var root = _builder.Build();

            _collection
                .AddSingleton(root)
                .AddSingleton<IConfiguration>(provider => provider.GetService<IConfigurationRoot>());
            
            ApplySettings(_collection, root);

            _provider = _collection.BuildServiceProvider();
        }
        
        public virtual void Dispose()
        {
            _provider?.Dispose();
        }

        /// <summary>
        /// Возвращаю сервия из списка
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetService<T>()
        {
            return _provider.GetService<T>();
        }
    }
}