from flask import Flask
from flask import request, jsonify
from hashlib import sha256
import os
import uuid
import json
import facebook
app = Flask(__name__)

@app.route('/facebook_login', methods=['POST'])
def facebook_login():
    
    data = request.get_json()
    facebook_token = data["facebook_token"]
    facebook_user_id = data["facebook_user_id"]

    graph = facebook.GraphAPI(access_token=facebook_token)
    me = graph.get_object(id='me')

    if me['id']!=facebook_user_id:
        raise Exception()
   
    user_info = login_user(facebook_user_id)

    return jsonify(user_info)


@app.route('/set_clicks', methods=['POST'])
def set_clicks():
    data = request.get_json()
    user_id = data['user_id']
    token = data['token']
    clicks = data['clicks']
    
    saved_infos = validate_user(user_id,token)
    
    saved_infos['clicks'] = clicks
    filename = user_id+'.txt'
    f = open(filename, "w")
    f.write(json.dumps(saved_infos))
    f.close()

    return jsonify({})

def validate_user(user_id,token):
    filename = user_id+'.txt'

    if not os.path.isfile(filename):
        raise Exception()

    f = open(filename, "r")
    file_content = f.read()
    f.close()
    saved_infos = json.loads(file_content)

    if saved_infos['token'] != token:
        raise Exception()
   
    return saved_infos
    
def login_user(facebook_id):
    token = str(uuid.uuid4())
    user_id = sha256(facebook_id.encode('utf-8')).hexdigest()
  
    filename = "./"+user_id+".txt"

    saved_infos = {
        "facebook_id" : facebook_id,
        "token" : token,
        "clicks" : 0
    }
    
    if not os.path.isfile(filename):
        f = open(filename, "w")
        f.write(json.dumps(saved_infos))
        f.close()
    else:
        saved_infos = update_token(filename,token)

    return {"token": token,"user_id":user_id, "clicks": saved_infos["clicks"]}

def update_token(filename,new_token):
      f = open(filename, "r+")
      file_content = f.read()
      saved_infos = json.loads(file_content)
                
      saved_infos["token"] = new_token
      
      f.seek(0)
      f.write(json.dumps(saved_infos))
      f.truncate()
      f.close()

      return saved_infos
        
