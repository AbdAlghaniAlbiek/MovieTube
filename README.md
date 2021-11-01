# MovieTube
This app is created for streaming videos from the server (Node js backend) to this UWP client app like YouTube app 😅   
Also it has the 3 security fundmentals:
1. Encryption/Decryption data that sended/received between server and client using AES-128-cbc alghorithm
2. Verify the requests that are from signed account not from any user and I achieved this using JWT tech
3. to verify the token is sended from the right server, I decode token to have "sercret keywork" and check this sercret keywork if it's equal to the stored secret keyword in my UWP application or not
