# 에이전트의 두뇌 역할을 하는 클랫, DQN을 실제 수행함 ###
# Q 함수를 딥러닝 신경망 형태로 정의
import torch
from torch import nn
from torch import optim
import torch.nn.functional as F
torch.random.manual_seed(777)

BATCH_SIZE = 32
CAPACITY = 10000

class Net(nn.Module):
    def __init__(self,n_in,n_mid,n_out):
        super(Net,self).__init__()# 부모클래스 nn.Module의 __init__을 호출함
        #n_in = 441
        self.conv1 = nn.Conv2d(in_channels=1, out_channels=2, kernel_size=10, stride=1)
        self.conv2 = nn.Conv2d(in_channels=2, out_channels=4, kernel_size=10, stride=1)

        self.fc1 = nn.Linear(4*64*64+4,100)
        self.fc2 = nn.Linear(100,100)
        self.fc3 = nn.Linear(100,4)

        #self.fc1 = nn.Linear(n_in, n_mid)
        #self.fc2 = nn.Linear(n_mid,n_mid)
        #self.fc3 = nn.Linear(n_mid,n_out)
    
    def forward(self, x):
        x = x[:,:,0:82,:]
        extra_data = x[:,:,-1,0:4]
        extra_data = extra_data.reshape(-1,4)
        #torch.LongTensor([[random.randrange(self.num_actions)]])
        #data = torch.zeros(21,21)
        #position_x = torch.abs(torch.round(x[0,0]-torch.tensor(5)*2))
        #position_y = torch.abs(torch.round(x[0,1]+torch.tensor(5)*2))
        #data[position_x.long(),position_y.long()] = torch.tensor(1)
        #dd = torch.reshape(data,(-1,441))
        x = F.relu(self.conv1(x))
        x = F.relu(self.conv2(x))
        x = x.view(-1, 4 * 64 * 64)
        x = torch.cat([extra_data,x],dim=1)
        x = F.relu(self.fc1(x))
        x = F.relu(self.fc2(x))
        output = self.fc3(x)

        
        #h1 = F.relu(self.fc1(x))
        #h2 = F.relu(self.fc2(h1))
        #output = self.fc3(h2)
        return output