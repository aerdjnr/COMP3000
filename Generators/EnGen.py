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


        def MachineGen(subnet):
            splitter = subnet.split(".")
            net_bit = splitter[0:3]
            machine_IP = net_bit[0]+"."+net_bit[1]+"."+net_bit[2]+"."+str(r.randint(10,200))
            return machine_IP


        Network = NetworkGen()
        with open("LabVar.txt","w") as f:
            f.write(Network+"\n")
except(IndexError):
    print("No flag")

