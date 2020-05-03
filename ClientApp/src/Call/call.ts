import Point from '../Point';

interface ICall
{
  inci_id: string;
  district: string;
  statbeat: string;
  street: string;
  cross_street: string;
  x: number;
  y: number;
  point: Point;
}

class Call implements ICall
{
  public inci_id: string;
  public district: string;
  public statbeat: string;
  public street: string;
  public cross_street: string;
  public x: number;
  public y: number;
  public point: Point;  
}


export default Call;