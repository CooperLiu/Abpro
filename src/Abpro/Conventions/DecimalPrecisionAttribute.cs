using System;
using System.Collections.Generic;
using System.Text;

namespace Abpro.Conventions
{
    /// <summary>
    /// <para>自定义Decimal类型的精度属性</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DecimalPrecisionAttribute : Attribute
    {
        public DecimalPrecisionAttribute(byte scale) : this(18, scale)
        {

        }

        public DecimalPrecisionAttribute(byte precision, byte scale)
        {
            Precision = precision;
            Scale = scale;
        }
        /// <summary>
        /// 精度
        /// </summary>
        public byte Precision { get; set; }

        /// <summary>
        /// 位数
        /// </summary>
        public byte Scale { get; set; }
    }
}
