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

        def portGen(ports):
            port_list = []
            portGroup = ""
            for x in range(ports):
                port = str(r.randint(20,1024))
                port_list.append(port)
            for y in range(len(port_list)):
                if y == 0:
                    portGroup += port_list[y]
                else:
                    portGroup = portGroup+", "+port_list[y]
            return portGroup

        def MachineGen(subnet,hosts):
            splitter = subnet.split(".")
            net_bit = splitter[0:3]
            host_list = []
            for x in range(hosts):
                machine_IP = [net_bit[0]+"."+net_bit[1]+"."+net_bit[2]+"."+str(r.randint(10,200)),f"{portGen(r.randint(1,5))}"]
                host_list.append(machine_IP)

            return host_list


        Network = NetworkGen()
        Machines = MachineGen(Network, r.randint(2,6))
        print(Machines)
        with open("LabVar.txt","w") as f:
            f.write(Network+"\n")
except(IndexError):
    print("No flag")

