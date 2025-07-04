# UHPPOTED.NET
This is a Server/Client UDP application for access control systems based on the UHPPOTE UT0311-L0x TCP/IP Wiegand access control boards.

This is a work in progress and developped out of GitHub (this repository will not get updated ATM)

This project is written in VB.NET, targeting .NET9




## UHPPOTED
Thanks to the awesome work of [uhppoted](https://github.com/uhppoted) in providing a .NET API (https://github.com/uhppoted/uhppoted-dll)

I have now a working concept




## ACS.Messaging
Many thanks to [Jonathon Aroutsidis](https://github.com/Johno-ACSLive) for ACS.Messaging




## Notes
The Server (AccessControlMonitor) is a Windows Service, and must be registered into Services (sc.exe).

The Client (AccessControl) is the GUI.


DLLs:

Cipher9 - is my own Encription method (used for getting SQL connection)

ACS.Messaging - this is the Server/Client UDP Socket from here [ACS-Messaging](https://github.com/Johno-ACSLive/ACS-Messaging)
