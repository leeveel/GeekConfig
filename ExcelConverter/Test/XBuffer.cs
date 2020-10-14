public class XBuffer
{
    private static readonly int intSize = sizeof(int);
    private static readonly int shortSize = sizeof(short);
    private static readonly int longSize = sizeof(long);
    private static readonly int floatSize = sizeof(float);
    private static readonly int doubleSize = sizeof(double);
    private static readonly int byteSize = sizeof(byte);
    private static readonly int sbyteSize = sizeof(sbyte);
    private static readonly int boolSize = sizeof(bool);

    #region Write

    public static unsafe void WriteInt(int value, byte[] buffer, ref int offset)
    {
        if (offset + intSize > buffer.Length)
            throw new XBufferOutOfIndexException();
        fixed (byte* ptr = buffer)
        {
            *(int*) (ptr + offset) = System.Net.IPAddress.HostToNetworkOrder(value);
            offset += intSize;
        }
    }

    public static unsafe void WriteShort(short value, byte[] buffer, ref int offset)
    {
        if (offset + shortSize > buffer.Length)
            throw new XBufferOutOfIndexException();
        fixed (byte* ptr = buffer)
        {
            *(short*) (ptr + offset) = System.Net.IPAddress.HostToNetworkOrder(value);
            offset += shortSize;
        }
    }

    public static unsafe void WriteLong(long value, byte[] buffer, ref int offset)
    {
        if (offset + longSize > buffer.Length)
            throw new XBufferOutOfIndexException();
        fixed (byte* ptr = buffer)
        {
            *(long*) (ptr + offset) = System.Net.IPAddress.HostToNetworkOrder(value);
            offset += longSize;
        }
    }

    public static unsafe void WriteFloat(float value, byte[] buffer, ref int offset)
    {
        if (offset + floatSize > buffer.Length)
            throw new XBufferOutOfIndexException();
        fixed (byte* ptr = buffer)
        {
            *(float*) (ptr + offset) = value;
            *(int*) (ptr + offset) = System.Net.IPAddress.HostToNetworkOrder(*(int*) (ptr + offset));
            offset += floatSize;
        }
    }

    public static unsafe void WriteDouble(double value, byte[] buffer, ref int offset)
    {
        if (offset + doubleSize > buffer.Length)
            throw new XBufferOutOfIndexException();
        fixed (byte* ptr = buffer)
        {
            *(double*) (ptr + offset) = value;
            *(long*) (ptr + offset) = System.Net.IPAddress.HostToNetworkOrder(*(long*) (ptr + offset));
            offset += doubleSize;
        }
    }

    public static unsafe void WriteByte(byte value, byte[] buffer, ref int offset)
    {
        if (offset + byteSize > buffer.Length)
            throw new XBufferOutOfIndexException();
        fixed (byte* ptr = buffer)
        {
            *(ptr + offset) = value;
            offset += byteSize;
        }
    }

    public static unsafe void WriteSByte(sbyte value, byte[] buffer, ref int offset)
    {
        if (offset + sbyteSize > buffer.Length)
            throw new XBufferOutOfIndexException();
        fixed (byte* ptr = buffer)
        {
            *(sbyte*) (ptr + offset) = value;
            offset += sbyteSize;
        }
    }

    public static unsafe void WriteString(string value, byte[] buffer, ref int offset)
    {
        if (value == null)
            value = "";
        fixed (byte* ptr = buffer)
        {
            var len = System.Text.Encoding.UTF8.GetBytes(value, 0, value.Length, buffer, offset + shortSize);
            if (offset + len + shortSize > buffer.Length)
                throw new XBufferOutOfIndexException();
            WriteShort((short) len, buffer, ref offset);
            offset += len;
        }
    }

    public static unsafe void WriteBool(bool value, byte[] buffer, ref int offset)
    {
        if (offset + boolSize > buffer.Length)
            throw new XBufferOutOfIndexException();
        fixed (byte* ptr = buffer)
        {
            *(bool*) (ptr + offset) = value;
            offset += boolSize;
        }
    }

    #endregion

    #region Read

    public static unsafe int ReadInt(byte[] buffer, ref int offset)
    {
        fixed (byte* ptr = buffer)
        {
            var value = *(int*) (ptr + offset);
            offset += intSize;
            return System.Net.IPAddress.NetworkToHostOrder(value);
        }
    }

    public static unsafe short ReadShort(byte[] buffer, ref int offset)
    {
        fixed (byte* ptr = buffer)
        {
            var value = *(short*) (ptr + offset);
            offset += shortSize;
            return System.Net.IPAddress.NetworkToHostOrder(value);
        }
    }

    public static unsafe long ReadLong(byte[] buffer, ref int offset)
    {
        fixed (byte* ptr = buffer)
        {
            var value = *(long*) (ptr + offset);
            offset += longSize;
            return System.Net.IPAddress.NetworkToHostOrder(value);
        }
    }

    public static unsafe float ReadFloat(byte[] buffer, ref int offset)
    {
        fixed (byte* ptr = buffer)
        {
            *(int*) (ptr + offset) = System.Net.IPAddress.NetworkToHostOrder(*(int*) (ptr + offset));
            var value = System.BitConverter.ToSingle(buffer, offset);
            offset += floatSize;
            return value;
        }
    }

    public static unsafe double ReadDouble(byte[] buffer, ref int offset)
    {
        fixed (byte* ptr = buffer)
        {
            *(long*) (ptr + offset) = System.Net.IPAddress.NetworkToHostOrder(*(long*) (ptr + offset));
            var value = System.BitConverter.ToDouble(buffer, offset);
            offset += doubleSize;
            return value;
        }
    }

    public static unsafe byte ReadByte(byte[] buffer, ref int offset)
    {
        fixed (byte* ptr = buffer)
        {
            var value = *(ptr + offset);
            offset += byteSize;
            return value;
        }
    }

    public static unsafe sbyte ReadSByte(byte[] buffer, ref int offset)
    {
        fixed (byte* ptr = buffer)
        {
            var value = *(sbyte*) (ptr + offset);
            offset += byteSize;
            return value;
        }
    }

    public static unsafe string ReadString(byte[] buffer, ref int offset)
    {
        fixed (byte* ptr = buffer)
        {
            var len = ReadShort(buffer, ref offset);
            if (len == 0)
                return "";
            var value = System.Text.Encoding.UTF8.GetString(buffer, offset, len);
            offset += len;
            return value;
        }
    }

    public static unsafe bool ReadBool(byte[] buffer, ref int offset)
    {
        fixed (byte* ptr = buffer)
        {
            var value = *(bool*) (ptr + offset);
            offset += boolSize;
            return value;
        }
    }

    #endregion
}

public class XBufferOutOfIndexException : System.Exception
{
}