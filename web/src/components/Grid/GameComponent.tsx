import React, { useEffect, useState } from 'react';
import * as signalR from "@microsoft/signalr";

export const GameComponent: React.FC = () => {
    const [generation, setGeneration] = useState(null);

    useEffect(() => {
        const hubConnection = new signalR.HubConnectionBuilder()
            .withUrl('/gameHub') // Specify the URL where the hub is hosted
            .build();

        hubConnection.start()
            .then(() => {
                console.log('Connection established!');
            })
            .catch(err => console.error(err));

        hubConnection.on('ReceiveGeneration', serializedGame => {
            // Handle received generation update
            setGeneration(JSON.parse(serializedGame));
        });

        return () => {
            hubConnection.stop();
        };
    }, []);

    return (
        <div>
            {/* Render the current generation */}
            {generation && (
                <pre>{JSON.stringify(generation, null, 2)}</pre>
            )}
        </div>
    );
};

