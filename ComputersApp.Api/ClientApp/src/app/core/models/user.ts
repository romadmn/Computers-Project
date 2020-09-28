import { IToken } from './token';
export interface IUser {
    id?: number;
    email: string;
    password: string;
    token?: IToken;
}