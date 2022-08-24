import os
import socket
import struct
import subprocess
import time
#subprocess.call("./MiroGame.app")

class Env():
    def __init__(self):
        '''게임 환경 실행'''
        os.system("open -a MiroGame.app")
        time.sleep(5)
        self.server_ip = '127.0.0.1'
        self.server_port = 50001
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        
    def connect(self):
        self.socket.connect((self.server_ip, self.server_port))

    def step(self, direction,done):
        senddata = struct.pack('ii',direction,done)
        self.socket.sendall(senddata)
        recvdata = self.socket.recv(12)
        recvdata = struct.unpack('fff', recvdata)
        return recvdata 