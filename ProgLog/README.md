# Progress Log



Below you will find documentation of the key development points of my project as I continue through the sprints, taking note of any potential problems and how they've been resolved.



###### Pre-Dev/Sprint 0 - N/A

* Small-Form Factor pc with Proxmox VE installed to serve as a hypervisor for managing the entire infrastructure
* FortiGate Virtual Machine (VM) installed for remote access and management of environment
* Kanban style Trello board set up, user stories in place



###### Sprint 1 - 27-10-2025

* Research on how infrastructure needs to be laid out. Planned to utilise an AD sever for logins, OPNSense router for main network management, Web server for locally stored lab-relevant websites, DNS server for address resolution (ease of access for beginners instead of having to manoeuvre IP address/no A record which isn't common at the targeted level).
* Surface level research on method of deployment \& management of VMs within the lab environment



###### Sprint 2 - 10/11/2025

* Uploaded Ubuntu Server, OPNSense router, and Windows Server ISO to Proxmox
* Cancelled Windows Server upload because of potential licensing issues, being granted only 180 days "free trial" which would pose an issue against my goals of minimizing budget and would prevent users from being able to verify once it expired
* At current, researching alternatives for Windows Server as a Domain Controller (DC)



###### Sprint 3 - 24/11/2025

* Alternative for Windows Server found, using Ubuntu Server with Samba. It is open source and operates using the same syntax (LDAP) so situations requiring scaling or realism to modern technology won't have a negative impact on my platform.
* Decided to merge web and DNS into one server for better resource management. Considering that both parts are used for user specific tasks, it seemed a viable move.
* Deployed OPNSense router and Ubuntu server VMs. Basic initial configurations completed
* Struggling to map network connectivity between devices correctly. Will need to set aside time to better understand virtual networking, oriented around the use of Proxmox and its "Linux Bridge" feature.
* Currently researching LDAP basics and how to set up one of the Ubuntu servers as a Samba DC.
* Research into how to automate VM creation using Proxmox "templates". Two approaches have been theorized. Approach 1 is to have a dedicated VM which handles and forwards requests from users regarding the labs, approach 2 is to send the requests directly to the platform/Proxmox.
  Approach 1 is more secure as I can more easily filter the request as the VM acts as a proxy, but will cost more resources and may affect scalability in future plans. Approach 2 is more efficient, but leaves less room (as of current knowledge) for if the requests are manipulated in any way after it leaves the app on the client-side.
* Encountered issue with OPNSense router running in 'live mode' and wiping any config changes upon reboot. Resolved by following official documentation and initiating config process using a separate dedicated account.
* 



###### Sprint 4 - 08/12/2025

* 
