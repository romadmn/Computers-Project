import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IUser } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) {}

  getAll(): Observable<IUser[]> {
    return this.http.get<IUser[]>('/api/user/');
  }

  getById(userId: number): Observable<IUser> {
    return this.http.get<IUser>('/api/user/' + userId);
  }

  post(user: IUser) {
    return this.http.post('/api/user/', user);
  }

  put(user: IUser) {
    return this.http.put('/api/user/', user);
  }
  delete(userId: number) {
    return this.http.delete('/api/user/' + userId);
  }

}
