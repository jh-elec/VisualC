using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;

public class Ringbuffer
{
    const Int32 Timeout = 1000;

    private byte[] Buffer;

    ReaderWriterLock Lock = new ReaderWriterLock();

    private int ReadPos;

    private int WritePos;

    private bool IsFull;

    public Ringbuffer( int Capacity )
    {
        if ( Capacity < 3 )
        {
            throw new ArgumentException("Invalid capacity. Must be 3 or larger. Values below 64 are not recommended!", "Capacity");
        }

        Lock.AcquireWriterLock(Timeout);
        try
        {
            Buffer = new byte[Capacity - 1];
        }
        finally
        {
            Lock.ReleaseWriterLock();
        }
    }

    public int Capacity
    {
        get { return Buffer.Length; }

        set
        {
            if ( value < 3 )
            {
                throw new ArgumentException("New capacity must be 3 or larger.");
            }
            Lock.AcquireWriterLock(Timeout);

            try
            {
                Buffer = new byte[value - 1];
                ReadPos = 0;
                WritePos = 0;
            }
            finally
            {
                Lock.ReleaseWriterLock();
            }
        }
    }

    private int Leng;
    public int Length
    {
        get
        {
            Lock.AcquireReaderLock(Timeout);
            try
            {
                if ( IsFull )
                {
                    return Buffer.Length;
                }
                else
                {
                    if ( ReadPos == WritePos )
                    {
                        Leng = 0;
                    }
                    else if ( ReadPos < WritePos )
                    {
                        Leng = WritePos - ReadPos;
                    }
                    else
                    {
                        Leng = (Buffer.Length - ReadPos) + WritePos;
                    }
                }
            }
            finally
            {
                Lock.ReleaseReaderLock();
            }

            return Leng;
        }
    }

    public void Clear()
    {
        Lock.AcquireWriterLock(Timeout);
        ReadPos = 0;
        WritePos = 0;
        Lock.ReleaseWriterLock();
    }

    public void Push( byte[] Data )
    {
        if ( Data == null || Data.Length == 0 )
        {
            throw new ArgumentException("Are you serious to store nothing? 0.o", "Data");
        }

        if ( Data.Length > Buffer.Length )
        {
            throw new InternalBufferOverflowException("Received more data than the buffer capacity can store.");
        }

        Lock.AcquireWriterLock(Timeout);

        try
        {
            if ( WritePos + Data.Length <= Buffer.Length )
            {
                Array.Copy(Data, 0, Buffer, WritePos, Data.Length);
                WritePos += Data.Length;
            }
            else
            {
                int i = (WritePos + Data.Length) - Buffer.Length;
                if ( ReadPos < i )
                {
                    throw new InternalBufferOverflowException("Received data would override unread data.");
                }

                Array.Copy(Data, 0, Buffer, WritePos, Buffer.Length - WritePos);
                Array.Copy(Data, Data.Length - i, Buffer, 0, i);
                WritePos = i;
                Console.WriteLine("CircularBuffer: Parted data (Push)");
            }

            if ( ReadPos == WritePos )
            {
                IsFull = true;
            }
        }
        finally
        {
            Lock.ReleaseWriterLock();
        }
    }

    public byte[] Pull( int Length )
    {
        return PullPeek(Length, false);
    }

    public int IndexOf( byte[] Search )
    {
        int IndexOf = -1;

        Lock.AcquireReaderLock(Timeout);
        try
        {
            int len = Length;
            if ( len >= Search.Length )
            {
                for ( int i = 0; i < len - Search.Length; i++)
                {
                    int Found = 0;
                    for ( int j = 0; j < Search.Length - 1; j++ )
                    {
                        int Pos = (ReadPos + i + j) % Buffer.Length;
                        if ( Buffer[Pos] == Search[j] )
                        {
                            Found += 1;
                        }
                    }
                    if ( Found == Search.Length )
                    {
                        IndexOf = i;
                        break;
                    }
                }
            }
        }
        finally
        {
            Lock.ReleaseReaderLock();
        }

        return IndexOf;
    }

    public byte[] Peek( int Length )
    {
        return PullPeek(Length, true);
    }

    private byte[] PullPeek( int Length , bool PeekOnly )
    {
        byte[] RetVal;

        if ( Length <= 0 )
        {
            throw new ArgumentException("Are you serious to request 0 bytes or less? 0.o", "Length");
        }

        if ( Length > Buffer.Length )
        {
            throw new InternalBufferOverflowException("Requested more data than the buffer capacity can store.");
        }

        Lock.AcquireReaderLock(Timeout);
        try
        {
            RetVal = new byte[Length];
            if ( (ReadPos + Length) <= Buffer.Length )
            {
                Array.Copy(Buffer, ReadPos, RetVal, 0, Length);

                if (!PeekOnly)
                {
                    ReadPos += Length;
                }
            }
            else
            {
                int l = (ReadPos + Length) - Buffer.Length;
                if ( ReadPos < l )
                {
                    throw new InternalBufferOverflowException("Requested data length larger than stored data. Buffer underrun!");
                }
                Array.Copy(Buffer, ReadPos, RetVal, 0, Length - l);
                Array.Copy(Buffer, 0, RetVal, Length - l, l);

                if ( !PeekOnly )
                {
                    ReadPos = l;
                }
            }
           
        }
        finally
        {
            Lock.ReleaseReaderLock();
        }

        return RetVal;
    }
}
