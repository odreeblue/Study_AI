# Package import to be used for implementation(구현)
import numpy as np
import matplotlib.pyplot as plt
import gym
from JSAnimation.IPython_display import display_animation
from v1_func import display_frames_as_gif


# A function that makes animations
from matplotlib import animation

frames = []
env = gym.make('CartPole-v0')
observation = env.reset() # Env needs to be initialized first

for step in range(0, 200):
    frames.append(env.render(mode='rgb_array')) # Add an image of each time to the frame
    action = np.random.choice(2) # 0 (Move the cart to the left ) , 1 (Move the cart to right)

    observation, reward, done, info = env.step(action) # Execute the action

display_frames_as_gif(frames)