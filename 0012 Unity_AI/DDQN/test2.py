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
import torch

a = np.zeros((20,20))
b = np.zeros((1,20))
b[0,0] = 1
b[0,1] = 2
b[0,2] = 3

ab = np.concatenate([a,b])

ab_reshape = np.reshape(ab,(1,1,21,20))
print(ab_reshape.shape)
ab_reshape_double = np.concatenate([ab_reshape,ab_reshape],axis=0)

#ab_reshape_double = np.stack([ab_reshape, ab_reshape],axis=0)

print(ab_reshape_double.shape)
print(ab_reshape_double[:,:,0:20,:])


ab_reshape_double_torch = torch.from_numpy(ab_reshape_double).type(torch.FloatTensor)


print(ab_reshape_double_torch[:,:,0:20,:])
print(ab_reshape_double_torch[:,:,0:20,:].size())


#ab_torch = torch.from_numpy(ab_reshape).type(torch.FloatTensor)
#ab_torch_slice = ab_torch[:,:,0:20,:]
#
#
#
#print(ab_torch_slice)
#print(ab_torch_slice.size())
#print(ab_reshape)
#print(ab_reshape.shape)

#x = torch.zeros(10,1,20,20)

#print(x.size())
