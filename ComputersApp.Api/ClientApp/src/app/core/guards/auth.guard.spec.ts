import { IToken } from './../models/token';
import { IUser } from './../models/user';
import { TestBed, inject, getTestBed } from '@angular/core/testing';
import { AuthGuard } from './auth.guard';
import { AuthenticationService } from '../services/authentication.service';
import { Router } from '@angular/router';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Role } from '../models/role.enum';
 
describe('AuthGuard', () => {
  let injector: TestBed;
  let authService: AuthenticationService
  let guard: AuthGuard;
  let routeMock: any = { snapshot: {}, data: {roles: Role.Admin}};
  let routeStateMock: any = { snapshot: {}, url: '/cookies'};
  let routerMock = {navigate: jasmine.createSpy('navigate')}

  beforeEach(() => {
  TestBed.configureTestingModule({
    providers: [AuthGuard, { provide: Router, useValue: routerMock }],
    imports: [HttpClientTestingModule]
  });
  injector = getTestBed();
  authService = injector.get(AuthenticationService);
  guard = injector.get(AuthGuard);
});

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });

  it('should redirect an unauthenticated user to the login route', () => {
    authService.currentUserSubject.next(null);
    expect(guard.canActivate(routeMock, routeStateMock)).toEqual(false);
    expect(routerMock.navigate).toHaveBeenCalledWith(['/login'], { queryParams: { returnUrl: routeStateMock.url  } });
  });

  it('should allow the authenticated user to access app', () => {
    const currentUser: IUser = { email: 'ferencrman@gmail.com', password: 'pass',
    token: {jwt: 'jwt', refreshToken: 'refreshToken'}};
    authService.currentUserSubject.next(currentUser);
    spyOn(authService, 'getUserRole').and.returnValue(Role.Admin);
    expect(guard.canActivate(routeMock, routeStateMock)).toEqual(true);
  });
});
