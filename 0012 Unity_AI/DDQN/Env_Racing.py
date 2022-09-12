import os
import socket
import struct
import subprocess
import time
#subprocess.call("./MiroGame.app")
import base64
from io import BytesIO
from PIL import Image
import numpy as np
class Env():
    def __init__(self):
        '''게임 환경 실행'''
        #os.system("Racing.exe")
        time.sleep(5)
        self.server_ip = '127.0.0.1'
        self.server_port = 50001
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        
    def connect(self):
        self.socket.connect((self.server_ip, self.server_port))
        # 초기 연결시 image 데이터 가져오기
        _ = self.step(1,0) # 아래로 한번 이동
        initial_data = self.step(0,0) # 다시 위로 이동
        return initial_data

    def receive_image(self,sock, image_size):
        data = b''
        to_receive = image_size # 송신 받아야할 이미지 크기
        while to_receive > 0:
            data += sock.recv(image_size)
            to_receive = image_size - len(data)

        return data

    def step(self, direction,done):
        # 진행 방향 명령(direction), 게임이 끝났는지 여부(done) 
        senddata = struct.pack('ii',direction,done) # 바이너리 데이터로 패킹
        self.socket.sendall(senddata) # Unity 로 패킹한 데이터 송신
        
        # 데이터 수신
        pos_x = struct.unpack('f',self.socket.recv(4))[0] # 공의 x 좌표
        pos_z = struct.unpack('f',self.socket.recv(4))[0] # 공의 z 좌표
        is_col = struct.unpack('f',self.socket.recv(4))[0] # 공이 벽에 부딪혔는지 bool
        image_size = int(struct.unpack('f',self.socket.recv(4))[0]) # 전송될 이미지 사이즈 크기
        print("x: "+str(pos_x)+", \
           z: "+str(pos_z)+", \
           collision: "+str(is_col)+", \
           image_size : "+str(image_size))
        
        # binary data 받기
        image_data = self.receive_image(self.socket,image_size)
        _ = self.receive_image(self.socket, 51000-image_size)
        # base64 string으로 변환하기
        image_data = base64.b64decode(image_data)
        
        # python 내 pixel형태로 변환
        stream = BytesIO(image_data)
        image = Image.open(stream).convert('L')
        stream.close()
        img_crop = image.crop((225,0,906-25,656)) # 이미지 자르기
        img_resize = img_crop.resize((int(656/8),int(656/8))) # 크기 줄이기
        img_array = np.asarray(img_resize) # 82,82

        # pos_x, pos_z, is_col 데이터를 numpy array로 만들기
        extra_data = np.zeros((1,img_array.shape[1]))
        extra_data[0,0] = pos_x
        extra_data[0,1] = pos_z
        extra_data[0,2] = is_col

        # 최종적으로 array를 합치기
        observation_next = np.concatenate([img_array, extra_data])
        observation_next = np.reshape(observation_next,(1,1,83,82))
        return observation_next 