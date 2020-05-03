

interface IPoint
{
  latitude: number;
  longitude: number;
  state_plane_x: number;
  state_plane_y: number;
  UTM: string;
  USNG: string;
}

export class Point implements IPoint
{
  public latitude: number;
  public longitude: number;
  public state_plane_x: number;
  public state_plane_y: number;
  public UTM: string;
  public USNG: string;

  constructor() {}
}

export default Point;