import { IComputer } from './computer';
export interface ICpu {
    id?: number;
    name: string;
    corsAmount: number;
    frequency: number;
    computers: IComputer[];
}
