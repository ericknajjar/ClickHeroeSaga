from flask import Flask
from flask import request, jsonify
from hashlib import sha256
import os
import uuid
import json
app = Flask(__name__)

# curl -H "Content-Type: application/json" -X POST -d '{"email":"erick@b.com","password":"1234"}' localhost:5000/pior_login_do_mundo
# curl -H "Content-Type: application/json" -X POST -d '{"token":"24717128-38bc-4c05-adc0-10d3a4a2f31d","user_id":"1ca0d4d6a1cd8fc07158580fcddb15968a25bcd55cc5c50d9a7152617653ec46", "clicks":3}' localhost:5000/set_clicks
def hello_world():
    return 'Hello, 2!'

@app.route('/pior_login_do_mundo', methods=['POST'])
def pior_login_do_mundo():
    
    data = request.get_json()
    user_info = create_user(data)
   
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
    
def create_user(credentials):
    email = credentials["email"]
    password = credentials["password"]
    token = str(uuid.uuid4())
    user_id = sha256(email.encode('utf-8')).hexdigest()
  
    filename = "./"+user_id+".txt"

    saved_infos = {
        "email" : email,
        "password" : password,
        "token" : token,
        "clicks" : 0
    }
    
    if not os.path.isfile(filename):
        f = open(filename, "w")
        f.write(json.dumps(saved_infos))
        f.close()
    else:
        saved_infos = update_token(filename,token,password)

    return {"token": token,"user_id":user_id, "clicks": saved_infos["clicks"]}

def update_token(filename,new_token, password):
      f = open(filename, "r+")
      file_content = f.read()
      saved_infos = json.loads(file_content)
      
      if saved_infos["password"] != password:
          raise Exception()
          
      saved_infos["token"] = new_token
      
      f.seek(0)
      f.write(json.dumps(saved_infos))
      f.truncate()
      f.close()

      return saved_infos
        
