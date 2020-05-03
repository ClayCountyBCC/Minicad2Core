import SimpleValue from './SimpleValue';
import Call from './Call/call';

export interface IState
{
  current_view: string;
  map: any;
  map_view: any;
  //call_base_connection: IConnection;
  //call_stream_connection: IConnection;
  //note_base_connection: IConnection;
  //note_stream_connection: IConnection;
  //unit_base_connection: IConnection;
  //unit_stream_connection: IConnection;
  //active_calls: Array<Call>;
  //units: [];
}

export interface IAction
{
  type: string
  payload: any
}

export interface IConnection
{
  connected: boolean;
  has_errors: boolean;
  retry: boolean;
}