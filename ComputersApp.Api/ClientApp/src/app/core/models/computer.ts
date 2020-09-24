import { OsType } from './OsType.enum';
import { ICpu } from './cpu';
export interface IComputer {
    id?: number;
    name: string;
    cpu?: ICpu;
    osType?: OsType;
    ramAmount: number;
    ssdAmount: number;
}
