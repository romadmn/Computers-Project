import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { IUser } from '../models/user';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Role } from '../models/role.enum';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    public currentUserSubject: BehaviorSubject<IUser>;
    public currentUser: Observable<IUser>;
    jwtHelper = new JwtHelperService();

    constructor(private http: HttpClient) {
        this.currentUserSubject = new BehaviorSubject<IUser>(JSON.parse(localStorage.getItem('currentUser')));
        this.currentUser = this.currentUserSubject.asObservable();
    }

    public get currentUserValue(): IUser {
        return this.currentUserSubject.value;
    }

    login(email: string, password: string) {
        return this.http.post<IUser>('/api/User/authenticate', { email: email, password: password })
            .pipe(map(user => {
                localStorage.setItem('currentUser', JSON.stringify(user));
                this.currentUserSubject.next(user);
                return user;
            }));
    }

    refreshToken(): Observable<IUser> {
      const accessToken = this.currentUserValue.token.jwt;
      const refreshToken = this.currentUserValue.token.refreshToken;
      return this.http.post<IUser>(
        '/api/User/refresh',
       {
        jwt: accessToken,
        refreshToken: refreshToken
       }).pipe(
        tap(response => {
          localStorage.setItem('currentUser', JSON.stringify(response));
          this.currentUserSubject.next(response);
        })
      );
     }

    logout() {
        localStorage.removeItem('currentUser');
        this.currentUserSubject.next(null);
    }

    isAdmin() {
        return this.getUserRole() === Role.Admin;
      }

    getUserRole() {
      if (this.currentUserValue && this.currentUserValue.token.jwt) {
        const token = this.currentUserValue.token.jwt;
        const role = this.jwtHelper.decodeToken(token)[
          'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
          ];
        return role;
      }
      return -1;
    }
}
