/*
    IFieldTypes.cs
    Represents special interfaces for special types.
    ***
    ISTE 330.02 - Group 16
    Ian Effendi
    Jacob Toporoff
 */

// Using statements.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTE.DAL.Database
{
    /// <summary>
    /// Wrapper for boolean IFieldType.
    /// </summary>
    public interface IBooleanFieldType : IFieldType<bool>
    {
    }

    /// <summary>
    /// Wrapper for string IFieldType.
    /// </summary>
    public interface IStringFieldType : IFieldType<string>
    {
    }
    
    /// <summary>
    /// Wrapper for integer IFieldType.
    /// </summary>
    public interface IIntegerFieldType : IFieldType<int>
    {
    }
    
    /// <summary>
    /// Wrapper for double IFieldType.
    /// </summary>
    public interface IDoubleFieldType : IFieldType<double>
    {
    }

    /// <summary>
    /// Wrapper for float IFieldType.
    /// </summary>
    public interface IFloatFieldType : IFieldType<float>
    {
    }

    /// <summary>
    /// Wrapper for Date IFieldType.
    /// </summary>
    public interface IDateFieldType : IFieldType<DateTime>
    {
    }

    /// <summary>
    /// Wrapper for integer range IFieldType.
    /// </summary>
    public interface IClampedIntegerFieldType : IFieldType<int>, IClamp<int>
    {
    }

    /// <summary>
    /// Wrapper for double range IFieldType.
    /// </summary>
    public interface IClampedDoubleFieldType : IFieldType<double>, IClamp<double>
    {
    }
    
    /// <summary>
    /// Wrapper for float range IFieldType.
    /// </summary>
    public interface IClampedFloatFieldType : IFieldType<float>, IClamp<float>
    {
    }

    /// <summary>
    /// Wrapper for Date range IFieldType.
    /// </summary>
    public interface IClampedDateFieldType : IFieldType<DateTime>, IClamp<DateTime>
    {
    }

}


