IF "%1" == "server" (
    start Build/socerer1.exe -logfile log-server.txt -mlapi server 
) ELSE IF "%1" == "client" (
    start Build/socerer1.exe -logfile log-server.txt -mlapi client
)