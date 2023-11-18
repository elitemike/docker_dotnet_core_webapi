# Simple dotnet 7 project with SignalR and docker image build 

This example shows a dotnet 7 web api project with SignalR enabled.  The application is setup with the newer lightweight application config.  All config is done within the program.cs file

THe dockerfile exists in the src folder. It's easiest to start in the src folder, otherwise you have to pass the path to the docker file. 

Replace `yourImageName` with whatever you want to call the image 

    docker build . -t yourImageName

Run the container replacing `yourImageName` with the name you gave during the build step.  --name is optional, but makes life easier.  The app is listening to port 80 inside the container.  Port 5000 can be changed to whatever port you want to access the api with. 

    docker run -d -p 5000:80 --name yourContainerName yourImageName 

Test the container by hitting 

http://localhost:5000/api/values

The response will be

    [
    "value1",
    "value2"
    ]

