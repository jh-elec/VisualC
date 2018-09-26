// Pcan_usb.cs
//
// !! C# Declarations for PCAN-Light Driver USB Version !!
// (c) 2002 PEAK-System Technik GmbH
// Rev. 1.0
// 15.08.2002
//
// This software is NO freeware.
// You are only permitted to use this software if you have hardware from
// PEAK-System Technik GmbH.
//
// Do not use the software or parts from it to communicate with non PEAK-Software/Hardware.
//
// If you like a more performant and powerful device driver, take a look at the
// PCAN-Tools which allow:
// - full buffered send/transmit by driver (up to 4096 CAN-Msg )
// - timer resolution 1 µs
// - callback function for receive
// - define message filter for application
// - write one software for all hardware ( no recompile )
// - communication between every hard & software
// - powerful development tools (monitor, logger etc.)

using System;
using System.Text;
using System.Runtime.InteropServices;

class PCAN_USB
{
	// CAN message
	[StructLayout(LayoutKind.Sequential, Pack=1)]
	public struct TCANMsg
	{
		public uint ID;        // 11/29 bit identifier
		public byte MSGTYPE;   // Bits from MSGTYPE_*
		public byte LEN;       // Data Length Code of the Msg (0..8)
		public ulong DATA;     // Data bytes 0..7
	}

	#region Baudrate Codes
	// BTR0BTR1 register
	// Baudrate code = register value BTR0/BTR1
	public const int CAN_BAUD_1M = 0x0014;     //   1 MBit/sec
	public const int CAN_BAUD_500K = 0x001C;   // 500 KBit/sec
	public const int CAN_BAUD_250K = 0x011C;   // 250 KBit/sec
	public const int CAN_BAUD_125K = 0x031C;   // 125 KBit/sec
	public const int CAN_BAUD_100K = 0x432F;   // 100 KBit/sec
	public const int CAN_BAUD_50K = 0x472F;    //  50 KBit/sec
	public const int CAN_BAUD_20K = 0x532F;    //  20 KBit/sec
	public const int CAN_BAUD_10K = 0x672F;    //  10 KBit/sec
	public const int CAN_BAUD_5K = 0x7F7F;     //   5 KBit/sec
	// You can define your own Baudrate for the BTROBTR1 register.
	// Take a look at www.peak-system.com for our software BAUDTOOL to
	// calculate the BTROBTR1 register for every baudrate and sample point.
	#endregion

	#region Error Codes
	// Error codes (bit code)
	public const int CAN_ERR_OK = 0x0000;             // No error
	public const int CAN_ERR_XMTFULL = 0x0001;        // Sendbuffer in controller full
	public const int CAN_ERR_OVERRUN = 0x0002;        // Read msg in CAN-Controller too late
	public const int CAN_ERR_BUSLIGHT = 0x0004;       // Buserror: an errorcounter reached limit
	public const int CAN_ERR_BUSHEAVY = 0x0008;       // Buserror: an errorcounter reached limit
	public const int CAN_ERR_BUSOFF = 0x0010;         // Buserror: CAN controller is 'Bus-Off'
	public const int CAN_ERR_QRCVEMPTY = 0x0020;      // RcvQueue is empty
	public const int CAN_ERR_QOVERRUN = 0x0040;       // RcvQueue was read too late
	public const int CAN_ERR_QXMTFULL = 0x0080;       // Sendequeue is full
	public const int CAN_ERR_REGTEST = 0x0100;        // Error while try to check register of SJA100. no hardware detect
	public const int CAN_ERR_NOVXD = 0x0200;          // Driver not loaded, no rights for license, trial license is expired...
	public const int CAN_ERR_RESOURCE = 0x2000;       // Could not create resource (FIFO, Client, Timeout)
	public const int CAN_ERR_ILLPARAMTYPE = 0x4000;   // Wrong parameter
	public const int CAN_ERR_ILLPARAMVAL = 0x8000;    // Wrong parameter type II
	public const int CAN_ERRMASK_ILLHANDLE = 0x1C00;  // Bit mask for handle error
	public const int CAN_ERR_ANYBUSERR = (CAN_ERR_BUSLIGHT | CAN_ERR_BUSHEAVY | CAN_ERR_BUSOFF);
	#endregion

	// Initialization constants
	public const int CAN_INIT_TYPE_EX = 1;     // Extended Frames
	public const int CAN_INIT_TYPE_ST = 0;     // Standard Frames

	// CAN message types
	public const int MSGTYPE_STANDARD = 0x00;  // Standard Frame (11 bit ID)
	public const int MSGTYPE_RTR      = 0x01;  // 1, if remote request, if 0 a data msg
	public const int MSGTYPE_EXTENDED = 0x02;  // 1, if CAN 2.0B Frame (29 bit ID)
	public const int MSGTYPE_STATUS   = 0x80;  // 1, if msg is a status msg


	///////////////////////////////////////////////////////////////////////////////
	//  Init()
	//  Aktiviert eine Hardware, macht Registertest des 82C200/SJA1000,
	//  teilt einen Sendepuffer und ein HardwareHandle zu.
	//  Programmiert Konfiguration der Sende/Empfangstreiber.
	//  Controller bleibt im Resetzustand.
	//  Uebergibt die Baudratenregister
	//  Wenn CANMsgType=0  ---> 11Bit ID Betrieb
	//  Wenn CANMsgType=1  ---> 11/29Bit ID Betrieb
	//  moegliche Fehler: NOVXD ILLHW REGTEST RESOURCE
	[DllImport("pcan_usb.dll", EntryPoint="CAN_Init")]
	public static extern uint Init(ushort BTR0BTR1, int CANMsgType);

	///////////////////////////////////////////////////////////////////////////////
	//  Close()
	//  alles beenden und Hardware freigeben
	//  moegliche Fehler: NOVXD
	[DllImport("pcan_usb.dll", EntryPoint="CAN_Close")]
	public static extern uint Close();

	///////////////////////////////////////////////////////////////////////////////
	//  Status()
	//  aktuellen Status (zB BUS-OFF) der Hardware zurueckgeben
	//  moegliche Fehler: NOVXD BUSOFF BUSHEAVY OVERRUN
	[DllImport("pcan_usb.dll", EntryPoint="CAN_Status")]
	public static extern uint Status();

	///////////////////////////////////////////////////////////////////////////////
	//  Write()
	//  Schreibt eine Message
	//  moegliche Fehler: NOVXD RESOURCE BUSOFF QXMTFULL
	[DllImport("pcan_usb.dll", EntryPoint="CAN_Write")]
	public static extern uint Write(ref TCANMsg msg);

	///////////////////////////////////////////////////////////////////////////////
	//  Read()
	//  gibt die naechste Message oder den naechsten Fehler aus dem
	//  RCV-Queue des Clients zurueck.
	//  Message wird nach 'msgbuff' geschrieben.
	//  moegliche Fehler: NOVXD  QRCVEMPTY
	[DllImport("pcan_usb.dll", EntryPoint="CAN_Read")]
	public static extern uint Read(out TCANMsg msg);

	///////////////////////////////////////////////////////////////////////////////
	//  VersionInfo()
	//  Holt Treiberinformationen (Version, (c) usw...)
	[DllImport("pcan_usb.dll", EntryPoint="CAN_VersionInfo")]
	public static extern uint VersionInfo(StringBuilder buffer);
}