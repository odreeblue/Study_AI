from PIL import Image
import numpy as np
img = Image.open("2022-09-06_22-16-00.png")
print(img)
img = np.asarray(img)
print(img.shape)
#print(img.size)