import React, { useEffect } from "react";
import * as signalR from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
    .withAutomaticReconnect()
    .withUrl(
        "https://localhost:7202/hubs/userCount",
        signalR.HttpTransportType.WebSockets
    )
    .build();

connection.on("updateTotalViews", (data) => {
    setTotalViews(data);
});

function newWindowLoadedOnClient() {
    connection
        .invoke("NewWindowLoaded")
        .then((value) => console.log(value + "hi"));
}

function fulfilled() {
    console.log("Connection to User Hub Successful");
    newWindowLoadedOnClient();
}

function rejected() {
    //rejected logs
}

connection.start().then(fulfilled, rejected);

export default function TotalPageViews() {
    const [totalViews, setTotalViews] = React.useState(0);

    useEffect(() => {
        // When the server calls the "updateTotalViews" function on the client, update the state of the totalViews variable
        connection.on("updateTotalViews", (data) => {
            setTotalViews(data);
        });
    }, []);

    return (
        <div>
            <h1>Total Page Views</h1>
            <p>{totalViews}</p>
        </div>
    );
}
