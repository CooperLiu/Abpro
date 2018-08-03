using System.Linq;
using System.Reflection;
using Abpro.MessageBus.Publisher;
using AutoMapper;

namespace Abpro.MessageBus.EventMessage
{
    public static class EventDataMqMessageConvertor
    {
        /// <summary>
        /// 自动注册EvenData与MqMessage 映射关系
        /// <code>
        /// public static void CreateMappingsInternal(IMapperConfigurationExpression mapper)
        /// {
        ///     mapper.CreateMap&lt;EventData, MqMessage&amp;lt;();
        ///     mapper.CreateEventsToMqMessagesMappings(Assembly.GetExecutingAssembly());
        /// }
        /// </code>
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="assembly"></param>
        public static void CreateConventionalMapping(this IMapperConfigurationExpression mapper, Assembly assembly)
        {
            var typesToRegister = assembly.GetTypes()
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null
                               && type.BaseType.IsGenericType
                               && (type.BaseType.GetGenericTypeDefinition() == typeof(EventDataPublishHandlerBase<,>)));

            foreach (var type in typesToRegister)
            {
                if (type.BaseType != null)
                {
                    var genericArgs = type.BaseType.GetGenericArguments();
                    var memberInfo = genericArgs[0].BaseType;
                    if (memberInfo != null && (genericArgs.Length > 1 && !memberInfo.IsGenericType))
                    {
                        mapper.CreateMap(genericArgs[0], genericArgs[1]);
                    }
                }
            }
        }
    }
}