# Usage
1.  Minimal APIs with an intention to show Docker and k8s implementation
2.  Images on Azure ACR
3.  Azure File Share used for shared volume mounts in k8s
4.  Ping runs on 5001, with endpoints "/" and "/send". "/" is a ping status endpoint.
5.  "/send" takes plain text data in request body, calls Pong which sends appended text back to Ping
6.  Data sent in the Request body is stored in a flat file on disk, shared by both.
   
