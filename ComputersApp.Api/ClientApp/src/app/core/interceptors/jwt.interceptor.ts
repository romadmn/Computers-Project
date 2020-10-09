import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';
import { catchError, filter, switchMap, take } from 'rxjs/operators';
import { Router } from '@angular/router';
import { IUser } from '../models/user';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    private refreshingInProgress: boolean;
    private accessTokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>(null);
    constructor(private authenticationService: AuthenticationService, private router: Router) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        const currentUser = JSON.parse(localStorage.getItem('currentUser'));
        let accessToken = null;
        if (currentUser) {
            accessToken = currentUser.token.jwt;
        }

        return next.handle(this.addAuthorizationHeader(request, accessToken)).pipe(
            catchError(err => {

              if (err instanceof HttpErrorResponse && err.status === 401) {
                const refreshToken = this.authenticationService.currentUserValue.token.refreshToken;

                if (refreshToken && accessToken) {
                  return this.refreshToken(request, next);
                }
                return this.logoutAndRedirect(err);
              }

              if (err instanceof HttpErrorResponse && err.status === 403) {
                return this.logoutAndRedirect(err);
              }
              const error = err.error.error;
              return throwError(error);
            })
          );
    }
    private logoutAndRedirect(err): Observable<HttpEvent<any>> {
        this.authenticationService.logout();
        this.router.navigateByUrl('/login');
        const error = err.error.error;
        return throwError(error);
    }
    private addAuthorizationHeader(request: HttpRequest<any>, token: string): HttpRequest<any> {
        if (token) {
          return request.clone({setHeaders: {Authorization: `Bearer ${token}`}});
        }

        return request;
      }
    private refreshToken(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (!this.refreshingInProgress) {
          this.refreshingInProgress = true;
          this.accessTokenSubject.next(null);

          return this.authenticationService.refreshToken().pipe(
            switchMap((res) => {
              this.refreshingInProgress = false;
              this.accessTokenSubject.next(res.token.jwt);

              return next.handle(this.addAuthorizationHeader(request, res.token.jwt));
            })
          );
        } else {

          return this.accessTokenSubject.pipe(
            filter(token => token !== null),
            take(1),
            switchMap(token => {
              return next.handle(this.addAuthorizationHeader(request, token));
            }));
        }
      }

}
