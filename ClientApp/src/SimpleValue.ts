import { Get } from './Utilities';

interface ISimpleValue
{
  label: string
  value: string
}

class SimpleValue implements ISimpleValue
{
  public label: string = "";
  public value: string = "";

  public static async GetUnitGroups(): Promise<Array<SimpleValue>>
  {
    const data = await Get<Array<SimpleValue>>('API/Group/GetUnitGroups');
    return data;
  }

}

export default SimpleValue;