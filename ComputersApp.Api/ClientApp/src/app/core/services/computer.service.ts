import { IComputer } from './../models/computer';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ComputerService {

  constructor(private http: HttpClient) {}

  getAll(): Observable<IComputer[]> {
    return this.http.get<IComputer[]>('/api/computer/');
  }

  getById(computerId: number): Observable<IComputer> {
    return this.http.get<IComputer>('/api/computer/' + computerId);
  }

  post(computer: IComputer) {
    return this.http.post('/api/computer/', computer);
  }

  put(computer: IComputer) {
    return this.http.put('/api/computer/', computer);
  }
  delete(computerId: number) {
    return this.http.delete('/api/computer/' + computerId);
  }

}
