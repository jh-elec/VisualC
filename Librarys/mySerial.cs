using System;
using System.IO.Ports;

public class mySerial
{
    public enum Baudrates : UInt32
    {
        50
        110
        150
        300
        1200
        2400
        4800
        9600
        19200
        38400
        57600
        115200
        230400
        460800
        500000
    };

    public enum mySerialError : int
    {
        MY_SERIAL_COMPORT_NAME_ERROR = -1,
        MY_SERIAL_BAUDRATE_ERROR,
        MY_SERIAL_NOT_OPEN_ERROR,
        MY_SERIAL_TIMEOUT_VALUE_ERROR,
        MY_SERIAL_NO_PORTS_FOUND,
    };

    /// <summary>
    /// Neue Serial Port Instanz generieren
    /// </summary>
    SerialPort port = new SerialPort();

    /// <summary>
    /// Listet alle verfügbaren Schnittstellen auf
    /// </summary>
    /// <returns></returns>
    public char[] listPorts()
    {
        string ports = null;
        foreach( string name in System.IO.Ports.SerialPort.GetPortNames())
        {
            ports += name;
        }
        return ref ports;
    }



    /// <summary>
    /// Initalisieren eines neuen ComPorts. Erzeugt eine neue Instanz
    /// </summary>
    /// <param name="_baud"> Baudrate</param>
    /// <param name="_comName">Name der Seriellenschnittstelle</param>
    /// <param name="_timeout">Zeit die vergehen darf bis ein Timeout auftritt</param>
    /// <returns></returns>
	public mySerialError init(Baudrates _baud , string _comName , UInt32 _timeout)
	{
        if(_comName.Length != 0) // Gültige Schnittstelle?
        {
            return mySerialError.MY_SERIAL_COMPORT_NAME_ERROR;
        }port.PortName = _comp;

        if(_baud!=0) // Gültige Baudrate 
        {
            return mySerialError.MY_SERIAL_BAUDRATE_ERROR;
        }port.BaudRate = _baud;

        if(_timeout) // Timeout 
        {
            return mySerialError.MY_SERIAL_PORT_TIMEOUT_VALUE_ERROR;
        }port.WriteTimeout = _timeout;
	}

    /// <summary>
    /// Zeichen von der Seriellen Schnittstelle aholen
    /// </summary>
    /// <returns>
    /// Liefert ggf. einen Fehler aus "mySerialError" enum
    /// </returns>
    public char getChar()
    {
        if (portOpen() == mySerialError.MY_SERIAL_PORT_NOT_OPEN)
        {
            return mySerialError.MY_SERIAL_PORT_NOT_OPEN;
        }
        return port.ReadChar();
    }

    /// <summary>
    /// Einen String bis zu "\n" abholen
    /// </summary>
    /// <returns>
    /// Liefert ggf. einen Fehler aus "mySerialError" enum
    /// </returns>
    public string getNewLine()
    {
        if (portOpen() == mySerialError.MY_SERIAL_PORT_NOT_OPEN)
        {
            return mySerialError.MY_SERIAL_PORT_NOT_OPEN;
        }
        return port.ReadLine();
    }

    /// <summary>
    /// Alles lesen was vorhanden ist abholen
    /// </summary>
    /// <returns>
    /// Liefert ggf. einen Fehler aus "mySerialError" enum
    /// </returns>
    public string getExist()
    {
        if (portOpen() == mySerialError.MY_SERIAL_PORT_NOT_OPEN)
        {
            return mySerialError.MY_SERIAL_PORT_NOT_OPEN;
        }
        return port.ReadExisting();
    }

    /// <summary>
    /// Ein Byte abholen
    /// </summary>
    /// <returns>
    /// Liefert ggf. einen Fehler aus "mySerialError" enum
    /// </returns>
    public byte getByte()
    {
        if( portOpen() == mySerialError.MY_SERIAL_PORT_NOT_OPEN )
        {
            return mySerialError.MY_SERIAL_PORT_NOT_OPEN;
        }
        return port.ReadByte();
    }

    /// <summary>
    /// String schreiben
    /// </summary>
    /// <param name="_in"> String der gesendet werden soll
    /// </param>
    /// <returns></returns>
    public mySerialError write( string _in )
    {
        if (portOpen() == mySerialError.MY_SERIAL_PORT_NOT_OPEN)
        {
            return mySerialError.MY_SERIAL_PORT_NOT_OPEN;
        }
        port.Write(_in);
    }

    public mySerialError writeLine( string _in )
    {
        if (portOpen() == mySerialError.MY_SERIAL_PORT_NOT_OPEN)
        {
            return mySerialError.MY_SERIAL_PORT_NOT_OPEN;
        }
        port.WriteLine(_in);
    }





    private mySerialError portOpen()
    {
        if( !port.IsOpen )
        {
            return mySerialError.MY_SERIAL_PORT_NOT_OPEN;
        }
    }
}
