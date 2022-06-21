'''
#key input 예제
import struct
a = input()
print("입력받은 문자 : "+a)
    # up : ^[[A, down : ^[[B , right : ^[[C, left : ^[[D
print(type(a)) # type : str
if a == "w":
    aa = 1
    print("up key")
    b = a.encode()
    print(len(b))
    print(type(b))
    c = struct.pack('iii',aa,aa,aa)
    print(len(c))
'''


# 클라이언트
import socket
import struct
server_ip = '192.168.200.179' # 위에서 설정한 서버 ip
#server_ip = '118.235.3.203'
server_port = 50001 # 위에서 설정한 서버 포트번호

socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
socket.connect((server_ip, server_port))

typeOfService = 1
displayId = 0
payloadLength = 4

# /end 입력될 때 까지 계속해서 서버에 패킷을 보냄
while True:
    
    direction_input = input()
    if direction_input == "w":
        direction = 0
    elif direction_input == "s":
        direction = 1
    elif direction_input == "d":
        direction = 2
    elif direction_input == "a":
        direction = 3
    elif direction_input == "x":
        break
    senddata = struct.pack('iiii',typeOfService,displayId,payloadLength,direction)
    #socket.sendall(msg.encode(encoding='utf-8'))
    socket.sendall(senddata)
    #data = socket.recv(100)
    #msg = data.decode() 
    #print('echo msg:', msg)
    
    #if msg == '/end':
    #    break

socket.close()