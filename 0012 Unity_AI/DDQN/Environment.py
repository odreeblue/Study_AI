from Agent import Agent
import numpy as np
import torch
from Env_Racing import Env
import time
import math
import warnings
warnings.filterwarnings("ignore", category=UserWarning)

class Environment:
    def __init__(self,gamma, max_steps, num_episodes, num_states, num_actions):
        self.gamma = gamma
        self.max_steps = max_steps
        self.num_epsidoes = num_episodes
        self.num_states = num_states # 태스크의 상태 변수 수(x,z)를 받아옴
        self.num_actions = num_actions # 태스크의 행동 가짓수(위, 아래 ,오른쪽, 왼쪽)를 받아옴
        self.agent = Agent(num_states, num_actions, gamma) # 에이전트 역할을 할 객체를 생성
        self.env = Env() # from Env_Racing import Env


    def run(self):
        '''실행'''
        initial_state = self.env.connect()# 게임과 tcp/ip 연결하고 초기 데이터 가져오기
        complete_episodes = 0 #현재까지 목표지점에 도착한 에피소드
        for episode in range(self.num_epsidoes): #최대 에피소드 수만큼 반복
            time.sleep(2)
            state = initial_state
            state = torch.from_numpy(state).type(torch.FloatTensor) # Numpy변수를 파이토치 텐서로 변환
            # state = torch.unsqueeze(state, 0)# size 2를 size 1*2 로 변환
            #print(state)

            count_ball = 5
            
            for step in range(self.max_steps): # 1에피소드에 해당하는 반복문

                action = self.agent.get_action(state, episode) # 다음 행동을 결정
                
                if step!=self.max_steps-1: # max_steps에 다다르지 못했을 때
                    observation_next = self.env.step(action.item(),0)
                    observation_next[0,0,-1,3] = count_ball
                    if observation_next[0,0,-1,2] == 0: # 충돌없을 때
                        reward = torch.FloatTensor([0.0])
                        state_next = observation_next
                        state_next = torch.from_numpy(state_next).type(torch.FloatTensor)
                        self.agent.memorize(state,action,state_next, reward)
                        self.agent.update_q_function()
                        state = state_next
                    elif observation_next[0,0,-1,2]==1: # 충돌있을 때 
                        reward = torch.FloatTensor([-1.0])
                        state_next = None # 충돌하면 다음 행동은 없다
                        self.agent.memorize(state,action,state_next, reward)
                        self.agent.update_q_function()
                        state = state_next
                        break
                    elif observation_next[0,0,-1,2]==10: # 보너스를 먹으면
                        reward = torch.FloatTensor([10.0])
                        state_next = observation_next
                        state_next = torch.from_numpy(state_next).type(torch.FloatTensor)
                        self.agent.memorize(state,action,state_next, reward)
                        self.agent.update_q_function()
                        count_ball = count_ball -1
                    elif observation_next[0,0,-1,2]==15: # 보너스를 먹으면
                        reward = torch.FloatTensor([20.0])
                        self.agent.memorize(state,action,state_next, reward)
                        self.agent.update_q_function()
                        complete_episodes +=1
                        count_ball = count_ball - 1
                        print('%d Episode, %d steps에 목표 지점 도착 ! 현재까지 목표지점 도착한 횟수: %d' % (episode,step, complete_episodes))
                        
                        break
                elif step == self.max_steps-1: # max_steps에 다다랐을때
                    observation_next = self.env.step(action.item(),1)
                    reward = torch.FloatTensor([-0.1])
                    state_next = None
                    self.agent.memorize(state,action,state_next,reward)
                    self.agent.update_q_function()
                    print('%d Episode, %d steps에도 목표지점 도착 못함 ! 현재까지 목표지점 도착한 횟수: %d' % (episode,step, complete_episodes))
                    break
            print('%d Episode, %d steps 진행중! 현재까지 목표지점 도착한 횟수: %d' % (episode,step, complete_episodes))
            if (episode % 2 ==0):
                self.agent.update_target_q_function()