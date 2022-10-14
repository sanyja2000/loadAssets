#hashgen
import hashlib
import json
import os
import sys

rootdir = os.path.dirname(os.path.realpath(__file__))+"\\ftpcontent\\imgs"
print("Root:"+rootdir)
def file_as_bytes(file):
    with file:
        return file.read()

assets = {"files":[]}

for root, subdirs, files in os.walk(rootdir):
    for filename in files:
        file_path = os.path.join(root, filename)
        md5 = hashlib.md5(file_as_bytes(open(file_path, 'rb'))).hexdigest()
        print(file_path,md5)
        assets["files"].append({"filename":file_path.replace(rootdir+"\\",""),"hash":md5})

output = json.dumps(assets)
with open("ftpcontent\\hashes.json","w") as hashesFile:
    hashesFile.write(output)
#print hashlib.md5(file_as_bytes(open(full_path, 'rb'))).hexdigest()

input("..")
