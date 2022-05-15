from collections import namedtuple
import torch
Transition = namedtuple('Transition',('state','action','next_state','reward'))



memory = [[[1,2,3,4]],[2],[3],[4]]
print(memory)
print(type(memory))

batch = Transition(*zip(*memory))
print(batch)

state_patch = torch.cat(batch.state)
print(state_patch)