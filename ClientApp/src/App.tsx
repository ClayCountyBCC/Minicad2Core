import React from 'react';
import Nav from './nav';
//import { Store } from './Store';
import * as signalR from '@microsoft/signalr';

const App = () =>
{
  //const { pending, error, abort } = useFetch('api/Call/Get');
  const connection = new signalR.HubConnectionBuilder()
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl("/callHub")
    .build();

  const unitConnection = new signalR.HubConnectionBuilder()
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl("/unitHub")
    .build();

  // network-wired icon
  // https://fontawesome.com/icons/network-wired?style=solid
  // or link icon
  // https://fontawesome.com/icons/link?style=solid
  // use round icon in menu bar
  // the icon border color will indicate if the connection is working
  // may transition to a loading icon from bulma if things are not connecting.

  connection.on("SendMessage", (message: string) =>
  {
    console.log('message', message);
  });

  unitConnection.on("Units", (units: Array<any>) =>
  {
    console.log('units', units);
  });

  connection.start();
  unitConnection.start();
  //let message = '';
  //connection.invoke("SendTime", message).catch(err => console.error(err));

  return (
    <>
      <Nav />
    </>
  );
}

export default App;
