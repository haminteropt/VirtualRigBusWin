# VirtualRigBusWin
Windows Version of VirtualRigBus

This emulate a Kenwood radio for legacy applications that don't support the HamBus Suite.  If the application supports CAT control for Kenwood, then with the Virtual RigBus it can attach to the HamBus.

You will need a virtual serial port driver.

There is two I would recommend:

1. [VSP Manager](http://k5fr.com/DDUtilV3wiki/index.php?title=VSP_Manager) Must be a ham with a valid license for personal use.  
1. [Com0Com](http://http://com0com.sourceforge.net/) An Open Source application.

If you get a permission denied error then you will have to add HamBus Ports to the 
HTTP Access Control List urlacl or run as admin.
``` 
netsh http add urlacl http://*:7300/ user=EVERYONE
netsh http add urlacl http://*:7301/ user=EVERYONE
netsh http add urlacl http://*:7302/ user=EVERYONE
...
netsh http add urlacl http://*:73xx/ user=EVERYONE
``` 



