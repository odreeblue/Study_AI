# 클라이언트
import socket
import struct
from pynput import keyboard
import time
# pynput 을 사용하려면 맥OS의 "보안 및 개인 정보 보호"->"손쉬운사용" -> "터미널.app"체크("시스템/응용프로그램/유틸리티/터미널.app")
                                                                    #-> "visual studio code" 체크
#server_ip = '192.168.200.179' # 위에서 설정한 서버 ip
#server_ip = '192.168.200.108'
server_ip = '127.0.0.1'
#server_ip = '118.235.3.203'
server_port = 50001 # 위에서 설정한 서버 포트번호

socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
socket.connect((server_ip, server_port))
# /end 입력될 때 까지 계속해서 서버에 패킷을 보냄
while True:
    
    direction_input = input()
    if direction_input == "w":
        direction = 0
        senddata = struct.pack('i',direction)
        socket.sendall(senddata)
    elif direction_input == "s":
        direction = 1
        senddata = struct.pack('i',direction)
        socket.sendall(senddata)
    elif direction_input == "d":
        direction = 2
        senddata = struct.pack('i',direction)
        socket.sendall(senddata)
    elif direction_input == "a":
        direction = 3
        senddata = struct.pack('i',direction)
        socket.sendall(senddata)
    elif direction_input == "x":
        break
    recvdata = socket.recv(12)
    recvdata = struct.unpack('fff',recvdata)
    print(recvdata[0])
    
socket.close()