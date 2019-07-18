using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public static class TypeConvert
    {
        public static DbType OracleDbTypeToDbType(OracleDbType oraDbType)
        {
            switch (oraDbType)
            {
                case OracleDbType.Varchar2:
                case OracleDbType.Char:
                case OracleDbType.NChar:
                case OracleDbType.NVarchar2:
                case OracleDbType.NClob:
                    return DbType.String;
                case OracleDbType.Int32:
                    return DbType.Int32;
                case OracleDbType.Single:
                    return DbType.Single;
                case OracleDbType.Double:
                    return DbType.Double;
                case OracleDbType.Decimal:
                    return DbType.Decimal;
                case OracleDbType.Date:
                case OracleDbType.TimeStamp:
                case OracleDbType.TimeStampTZ:
                case OracleDbType.TimeStampLTZ:
                    return DbType.DateTime;
                case OracleDbType.IntervalDS:
                    return DbType.Time;
                case OracleDbType.IntervalYM:
                    return DbType.Int32;
                case OracleDbType.Long:
                case OracleDbType.Clob:
                    return DbType.String;
                case OracleDbType.Raw:
                case OracleDbType.LongRaw:
                case OracleDbType.Blob:
                case OracleDbType.BFile:
                    return DbType.Binary;
                case OracleDbType.RefCursor:
                    return DbType.Object;
                case OracleDbType.Boolean:
                    return DbType.Boolean;
                default:
                    throw new NotSupportedException();
            }
        }

        public static OracleDbType DbTypeToOracleDbType(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString:
                case DbType.String:
                    return OracleDbType.Varchar2;
                case DbType.AnsiStringFixedLength:
                case DbType.StringFixedLength:
                    return OracleDbType.Char;
                case DbType.Byte:
                case DbType.Int16:
                case DbType.SByte:
                case DbType.UInt16:
                case DbType.Int32:
                    return OracleDbType.Int32;
                case DbType.Single:
                    return OracleDbType.Single;
                case DbType.Double:
                    return OracleDbType.Double;
                case DbType.Date:
                    return OracleDbType.Date;
                case DbType.DateTime:
                    return OracleDbType.TimeStamp;
                case DbType.Time:
                    return OracleDbType.IntervalDS;
                case DbType.Binary:
                    return OracleDbType.Blob;
                case DbType.Boolean:
                    return OracleDbType.Boolean;
                case DbType.Int64:
                case DbType.UInt64:
                case DbType.VarNumeric:
                case DbType.Decimal:
                case DbType.Currency:
                    return OracleDbType.Int64;
                case DbType.Guid:
                    return OracleDbType.Raw;
                default:
                    throw new NotSupportedException();
            }
        }
        public static DbType TypeToDBType(Type type)
        {
            return typeMap[type];
        }
        public static Dictionary<Type, DbType> typeMap;
        static TypeConvert()
        {
            typeMap = new Dictionary<Type, DbType>();
            typeMap[typeof(byte)] = DbType.Byte;
            typeMap[typeof(sbyte)] = DbType.SByte;
            typeMap[typeof(short)] = DbType.Int16;
            typeMap[typeof(ushort)] = DbType.UInt16;
            typeMap[typeof(int)] = DbType.Int32;
            typeMap[typeof(uint)] = DbType.UInt32;
            typeMap[typeof(long)] = DbType.Int64;
            typeMap[typeof(ulong)] = DbType.UInt64;
            typeMap[typeof(float)] = DbType.Single;
            typeMap[typeof(double)] = DbType.Double;
            typeMap[typeof(decimal)] = DbType.Decimal;
            typeMap[typeof(bool)] = DbType.Boolean;
            typeMap[typeof(string)] = DbType.String;
            typeMap[typeof(char)] = DbType.StringFixedLength;
            typeMap[typeof(Guid)] = DbType.Guid;
            typeMap[typeof(DateTime)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset)] = DbType.DateTimeOffset;
            typeMap[typeof(byte[])] = DbType.Binary;
            typeMap[typeof(byte?)] = DbType.Byte;
            typeMap[typeof(sbyte?)] = DbType.SByte;
            typeMap[typeof(short?)] = DbType.Int16;
            typeMap[typeof(ushort?)] = DbType.UInt16;
            typeMap[typeof(int?)] = DbType.Int32;
            typeMap[typeof(uint?)] = DbType.UInt32;
            typeMap[typeof(long?)] = DbType.Int64;
            typeMap[typeof(ulong?)] = DbType.UInt64;
            typeMap[typeof(float?)] = DbType.Single;
            typeMap[typeof(double?)] = DbType.Double;
            typeMap[typeof(decimal?)] = DbType.Decimal;
            typeMap[typeof(bool?)] = DbType.Boolean;
            typeMap[typeof(char?)] = DbType.StringFixedLength;
            typeMap[typeof(Guid?)] = DbType.Guid;
            typeMap[typeof(DateTime?)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset?)] = DbType.DateTimeOffset;
        }

    }
}
