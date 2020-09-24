import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ICpu } from '../models/cpu';

@Injectable({
  providedIn: 'root'
})
export class CpuService {

  constructor(private http: HttpClient) {}

  getAll(): Observable<ICpu[]> {
    return this.http.get<ICpu[]>('/api/cpu/');
  }

  getById(cpuId: number): Observable<ICpu> {
    return this.http.get<ICpu>('/api/cpu/' + cpuId);
  }

  post(cpu: ICpu) {
    return this.http.post('/api/cpu/', cpu);
  }

  put(cpu: ICpu) {
    return this.http.put('/api/cpu/', cpu);
  }
  delete(cpuId: number) {
    return this.http.delete('/api/cpu/' + cpuId);
  }

}
