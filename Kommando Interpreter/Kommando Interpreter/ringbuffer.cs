using System;

public class Ringbuffer
{
    private struct Buffer_Struct
    {
        public uint	    Actual;  		/**< Current storage location in the circular buffer. */
        public uint	    Next;  			/**< Current retrieval location in the circular buffer. */
        public uint     Size;           /**< size of the buffer's underlying storage array. */
        public uint     Count;          /**< Number of bytes currently stored in the buffer. */
        public byte[]   buffer;
    }
    private Buffer_Struct ringbuffer;


	public Ringbuffer(uint size)
    { 
        ringbuffer.buffer   = new byte[size];
        ringbuffer.Actual	= 0;
        ringbuffer.Next 	= 0;
        ringbuffer.Size     = size;
        ringbuffer.Count    = 0;
    }


    public uint GetCount
    {
        get { return ringbuffer.Count; }
    }

    public uint GetFreeCount
    {
        get { return ringbuffer.Size - GetCount; }
    }


    public bool IsEmpty
    {
        get { return GetCount == 0; }
    }

    public bool IsFull
    {
        get { return GetCount == ringbuffer.Size; }
    }

    public void Insert(byte data)
    {
        ringbuffer.buffer[ringbuffer.Actual++] = data;

        if (ringbuffer.Actual == ringbuffer.Size)
	    {
            ringbuffer.Actual = 0;
        }

        ringbuffer.Count++;
    }

    public byte Get()
    {
        byte ret = ringbuffer.buffer[ringbuffer.Next];

        ringbuffer.Next++;
        if (ringbuffer.Next == ringbuffer.Size )
	    {
            ringbuffer.Next = 0;
        }

        ringbuffer.Count--;

        return ret;
    }

    public byte[] Buffer
    {
        get { return ringbuffer.buffer; }
    }
}
