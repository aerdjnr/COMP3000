import random as r
import sys as s

def Corrector(target):
    target = str(target)
    target = target.replace("[","")
    target = target.replace("]","")
    return target

global Network 

try:
    if s.argv[1]=="-g":
        def NetworkGen():
            octect1 = str(r.randint(11,189))
            octect2 = str(r.randint(1,254))
            octect3 = str(r.randint(1,254))
            network = (octect1+"."+octect2+"."+octect3+".0")
            return network
        def ServiceGen(flag):
            serviceBank = [
                ["telnet.service","23"],
                ["sshd.service", "22"],
                ["samba-ad-dc.service","53","389","636"],
                ["httpd.service","80","443"],
                ["smb.service","445"],
                ["systemd-journald.service"],
                ["ufw.service"],
            ]

            store = []
            if flag == 0:
                serviceCount = r.randint(1,4)
                for x in range(serviceCount):
                    service = serviceBank[r.randint(0,serviceCount)]
                    if service[0] in store:
                        continue
                    else:
                        store += service
                
                return store
            if flag == 1:
                serviceCount = r.randint(1,4)
                for x in range(serviceCount):
                    service = serviceBank[r.randint(0,serviceCount)]
                    if service[0] in store:
                        continue
                    else:
                        store += service
                store.append(serviceBank[5][0])
                store.append(serviceBank[6][0])
                return store

        def MachineGen(subnet):
            splitter = subnet.split(".")
            net_bit = splitter[0:3]
            machine_IP = net_bit[0]+"."+net_bit[1]+"."+net_bit[2]+"."+str(r.randint(10,200))
            return machine_IP

        def RuleGen(subnet):
            machine =  MachineGen(subnet)
            ruleCount = r.randint(0,3)
            a_or_d = ""
            ufw_Rules = ["To                    Action        From","--                    ------        ----"]
            for x in range(ruleCount):
                if r.randint(0,1)==0:
                    a_or_d = "DENY    "
                else:
                    a_or_d = "ALLOW   "
                portGen = str(r.randint(1000,1024))
                if int(portGen)>999:
                    ufw_Rules.append(portGen+"/tcp"+"              "+a_or_d+"      "+subnet+"/24")
                    continue
                if int(portGen)>99:
                    ufw_Rules.append(portGen+"/tcp"+"               "+a_or_d+"      "+subnet+"/24")
                else:
                    ufw_Rules.append(portGen+"/tcp "+"               "+a_or_d+"      "+subnet+"/24")

    
            #for x in range(len(ufw_Rules)):
            #    print(ufw_Rules[x])
            return ufw_Rules

        Network = NetworkGen()
        CurrentServices = Corrector(ServiceGen(0))
        DesiredServices = Corrector(ServiceGen(1))
        CurrentRules = Corrector(RuleGen(Network))
        with open("LabVar.txt","w") as f:
            f.write(Network+"\n")
            f.write(str(CurrentServices)+"\n")
            f.write(str(DesiredServices)+"\n")
            f.write(str(CurrentRules))
except(IndexError):
    print("No flag")

try:
    if s.argv[1]=="-u":
        def Assemble(segment):
            f = open("LabVar.txt","r")
            for i, line in enumerate(f):
                if i == segment:  
                    reassemble = line.replace("'","")
                    reassemble = reassemble.split(", ")
                    
            return reassemble

        if s.argv[2] == "-s":
            Rules = Assemble(3)
            for x in range(2,len(Rules)):
                print(f"[ {x-1}]"+Rules[x])

        if s.argv[2] == "-d":
            Rules = Assemble(3)
            File = []
            def Delete(filename, rule):
                old = open(filename,"r")
                File = old.readlines()
                old.close()
                
                f = open(filename, "w")
                del File[3]
                del Rules[rule]
                File.append(Corrector(Rules))
                for x in range(len(File)):
                    f.write(File[x])

            Delete("LabVar.txt", int(s.argv[3])+1)
            for x in range(2,len(Rules)):
                print(f"[ {x-1}]"+Rules[x])

        if s.argv[2]=="-r":
            parsed_q = s.argv[3].split(" ")
            action = parsed_q[0]
            action = action.upper()
            if action == "DENY":
                action = "DENY    "
            else:
                action = "ALLOW   "
            # direction = parsed_q[1]
            net = parsed_q[2]
            port = parsed_q[4]
            if int(port)>999:
                rule = (port+"/tcp"+"               "+action+"      "+net+"/24")
            if 1000>int(port)>99:
                rule = (port+"/tcp"+"               "+action+"      "+net+"/24")
            else:
                rule = (port+"/tcp "+"               "+action+"      "+net+"/24")
            print(rule)
            def OverWrite(filename, newRule):
                old = open(filename,"r")
                File = old.readlines()
                old.close()

                Rules = Assemble(3)
                Rules.append(newRule)
                del File[3]
                File.append(Corrector(Rules))
                f = open(filename,"w")
                for x in range(len(File)):
                    f.write(File[x])
            OverWrite("LabVar.txt", rule)

except(IndexError):
    print("No flag")