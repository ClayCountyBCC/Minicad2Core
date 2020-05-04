import React, { useReducer } from 'react';
import { IState, IAction } from './interfaces';

const initialState: IState =
{
  current_view: "active calls",
  map: 0,
  map_view: 0
}

export function StoreProvider(props: any)
{
  const [state, dispatch] = useReducer(reducer, initialState);
  return (<Store.Provider value={{ state, dispatch }}>{props.children}</Store.Provider>);
}

export const Store = React.createContext<IState | any>(initialState);

function reducer(state: IState, action: IAction): IState
{
  switch (action.type)
  {
    case "units":
      return state;

    case "calls":
      return state;

    case "save_map":
      return {
        ...state,
        map: action.payload
      }

    case "save_map_view":
      return {
        ...state,
        map_view: action.payload
      }

    case "set_current_view":
      return {
        ...state,
        current_view: action.payload
      };

    default:
      console.log('action type not found, returning current state', action.type);
      return state;
  }
}