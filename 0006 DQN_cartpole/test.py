from collections import namedtuple
import torch

#text = "word1anotherword23nextone456lastone333"
#numbers = [x for x in text if x.isdigit()]
#print(numbers)

t = torch.FloatTensor([[3.0, 1.0,4.0,100.0]])
print(t.max(1))
print(t.max(1)[1])
print(t.max(1)[1].view(1,1))