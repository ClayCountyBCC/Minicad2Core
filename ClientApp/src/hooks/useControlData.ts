import { useContext } from 'react';
import { Store } from '../Store';
import * as signalR from '@microsoft/signalr';
//import { IFetchData } from './interfaces';

export const useControlData = async (hub_url: string, hub_event: string, dispatch_type: string) =>
{
  const hub_connected = dispatch_type + '_connected';

  const { dispatch } = useContext(Store);

  function SetConnection(connected: boolean, error: any = null)
  {
    dispatch({ type: hub_connected, payload: connected });
    console.log(dispatch_type + ' connection change', error);
  }

  const connection = new signalR.HubConnectionBuilder()
    .withAutomaticReconnect([0, 0, 10000])
    .configureLogging(signalR.LogLevel.Information)
    .withUrl(hub_url)
    .build();

  connection.onreconnecting(error =>
  {
    SetConnection(false, error);    
  });

  connection.onreconnected(connectionId =>
  {
    SetConnection(true);
  });

  connection.on(hub_event, (data: Array<any>) =>
  {
    console.log(hub_event, data);
    dispatch({ type: dispatch_type, payload: data });
  });

  connection.onclose(error =>
  {
    SetConnection(false, error);
  });
  
  async function start()
  {
    try
    {
      await connection.start();
      SetConnection(true);
      console.assert(connection.state === signalR.HubConnectionState.Connected);
      console.log("connected");
    } catch (err)
    {
      SetConnection(false, err);
      console.assert(connection.state === signalR.HubConnectionState.Disconnected);
      setTimeout(() => start(), 5000);
    }
  };

  start();
  
  return { connectionState: connection.state, connectionStart: start };
}

export default useControlData;