import React from 'react';
import Nav from './nav';
//import { Store } from './Store';
import * as signalR from '@microsoft/signalr';
import useControlData from './hooks/useControlData';

const App = () =>
{
  const callControl = useControlData("/callHub", "Calls", "calls");
  const unitControl = useControlData("/unitHub", "Units", "units");
  console.log('data controls', callControl, unitControl);


  //// network-wired icon
  //// https://fontawesome.com/icons/network-wired?style=solid
  //// or link icon
  //// https://fontawesome.com/icons/link?style=solid
  //// use round icon in menu bar
  //// the icon border color will indicate if the connection is working
  //// may transition to a loading icon from bulma if things are not connecting.

  return (
    <>
      <Nav />
    </>
  );
}

export default App;
