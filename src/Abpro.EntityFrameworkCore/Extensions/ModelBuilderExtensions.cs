using System;
using System.Globalization;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Abpro.EntityFrameworkCore.Extensions
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// 数据表前缀分隔符，例如 PayCenter_Trades
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static string ToPrefixWithSplitChar(this string tableName, string splitChar = SplitChar.Underline)
        {
            return tableName + splitChar;
        }

        /// <summary>
        /// 转换为复数形式
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public static string ToPluralize(this string entityName)
        {
#if NET461
            return System.Data.Entity.Design.PluralizationServices.PluralizationService.CreateService(new CultureInfo("en")).Pluralize(entityName);
#elif NETCOREAPP2_0
            return Inflector.PluralizationService.CreateService(new CultureInfo("en")).Pluralize(entityName);
#endif
        }

        /// <summary>
        /// 更改数据表前缀
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="prefix"></param>
        /// <param name="schemaName"></param>
        /// <param name="entities"></param>
        public static void ChangeTableNamingConvention(this ModelBuilder modelBuilder, string prefix, string schemaName = null, params Type[] entities)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.Public;
            var modelBuilderType = modelBuilder.GetType();
            MethodInfo method = modelBuilderType.GetMethod("Entity", flag);
            foreach (var type in entities)
            {
                var typeMethod = method.MakeGenericMethod(type);
                dynamic config = typeMethod.Invoke(modelBuilder, new object[] { });

                if (schemaName == null)
                {
                    config.ToTable(prefix + type.Name.ToPluralize());
                }
                else
                {
                    config.ToTable(prefix + type.Name.ToPluralize(), schemaName);
                }
            }
        }
    }
}
