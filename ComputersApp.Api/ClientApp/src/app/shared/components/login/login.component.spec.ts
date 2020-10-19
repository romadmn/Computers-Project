/// <reference types="Jasmine" />

import { IUser } from './../../../core/models/user';
import { AuthenticationService } from './../../../core/services/authentication.service';
import { ActivatedRoute, Data, Params, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EMPTY, Observable, of } from 'rxjs';
import { LoginComponent } from './login.component';
import { TestBed } from '@angular/core/testing';
import { ComputersComponent } from '../computers/computers.component';
import { AuthGuard } from 'src/app/core/guards/auth.guard';

describe('LoginComponent', () => {
    let component: LoginComponent;
    let authService: AuthenticationService;
    const routerSpy = { navigate: jasmine.createSpy('navigate') };
    let spy: any;

    beforeEach(() => {
        // Arrange
        TestBed.configureTestingModule({
            declarations: [LoginComponent, ComputersComponent],
            providers: [
                {
                    provide: ActivatedRoute,
                    useValue: {
                        snapshot: {
                            queryParams: [
                                {
                                    returnUrl: '',
                                },
                            ],
                        }
                    },
                },
                { provide: Router, useValue: routerSpy }
            ],
            imports: [FormsModule,
                ReactiveFormsModule,
                RouterModule.forRoot([
                    { path: '', component: ComputersComponent, canActivate: [AuthGuard] }
                ])]
        }).compileComponents();
        authService = new AuthenticationService(null);
        spy = spyOn(authService, 'login').and.callFake(() => {
            return EMPTY;
        });
        component = new LoginComponent(new FormBuilder(), TestBed.get(ActivatedRoute), null, authService);
        component.ngOnInit();
    });

    it('should create form with 2 control', () => {
        // Assert
        expect(component.loginForm.contains('email')).toBeTruthy();
        expect(component.loginForm.contains('password')).toBeTruthy();
    });

    it('should mark email as invalid if empty value', () => {
        // Arrange
        const control = component.loginForm.get('email');

        // Act
        control.setValue('');

        // Assert
        expect(control.valid).toBeFalsy();
    });

    it('should mark email as invalid if value does not match a pattern', () => {
        // Arrange
        const control = component.loginForm.get('email');
        const invalidEmail = 'romaddd';

        // Act
        control.setValue(invalidEmail);

        // Assert
        expect(control.valid).toBeFalsy();
    });

    it('should mark password as invalid if empty value', () => {
        // Arrange
        const control = component.loginForm.get('password');

        // Act
        control.setValue('');

        // Assert
        expect(control.valid).toBeFalsy();
    });

    it('should mark password as invalid if value does not match a pattern', () => {
        // Arrange
        const control = component.loginForm.get('password');
        const invalidPassword = 'password';

        // Act
        control.setValue(invalidPassword);

        // Assert
        expect(control.valid).toBeFalsy();
    });

    it('should return from function if form invalid', () => {
        // Arrange
        const control = component.loginForm.get('email');

        // Act
        control.setValue('');
        component.onSubmit();

        // Assert
        expect(spy).not.toHaveBeenCalled();

    });

    it('should login', () => {
        // Arrange
        const user: IUser = { email: 'ferencrman@gmail.com', password: 'MyPassword111' };
        component.loginForm.get('email').setValue(user.email);
        component.loginForm.get('password').setValue(user.password);

        // Act
        component.onSubmit();

        // Assert
        expect(spy).toHaveBeenCalledTimes(1);
    });

});

