import React, { useState, useEffect } from 'react';
import { Store } from '../Store';
//import { IFetchData } from './interfaces';

export const useFetchData = (baseFetchData: Function, dispatch_type: string, initial_load: boolean) =>
{
  const { dispatch } = React.useContext(Store);
  const [isLoading, setIsLoading] = useState(false);
  const [isError, setIsError] = useState(false);

  const fetchData = async () =>
  {
    setIsError(false);
    setIsLoading(true);

    try
    {
      const result = await baseFetchData();
      dispatch({ type: dispatch_type, payload: result });

    } catch (error)
    {
      setIsError(true);
      console.log('error in useFetchData', error);
    }
    setIsLoading(false);
  }

  useEffect(() =>
  {
    initial_load && fetchData();
  }, []);

  return { isLoading, isError, fetchData };
}

export default useFetchData;