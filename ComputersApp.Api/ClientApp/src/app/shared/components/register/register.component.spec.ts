/// <reference types="Jasmine" />

import { UserService } from './../../../core/services/user.service';
import { RegisterComponent } from './register.component';
import { EMPTY, Observable, of } from 'rxjs';
import { FormBuilder } from '@angular/forms';
import { IUser } from 'src/app/core/models/user';

describe('RegisterComponent', () => {
    let component: RegisterComponent;
    let userService: UserService;
    let postAuthSpy: any;

    beforeEach(() => {
        // Arrange
        userService = new UserService(null);
        postAuthSpy = spyOn(userService, 'post').and.returnValue(EMPTY);
        component = new RegisterComponent(new FormBuilder(), userService);
        component.ngOnInit();
    });

    it('should create form with 3 control', () => {
        // Assert
        expect(component.registerForm.contains('email')).toBeTruthy();
        expect(component.registerForm.contains('password')).toBeTruthy();
        expect(component.registerForm.contains('confirmPassword')).toBeTruthy();
    });

    it('should mark email as invalid if empty value', () => {
        // Arrange
        const control = component.registerForm.get('email');

        // Act
        control.setValue('');

        // Assert
        expect(control.valid).toBeFalsy();
    });

    it('should mark email as invalid if value does not match a pattern', () => {
        // Arrange
        const control = component.registerForm.get('email');
        const invalidEmail = 'email';

        // Act
        control.setValue(invalidEmail);

        // Assert
        expect(control.valid).toBeFalsy();
    });

    it('should mark password as invalid if empty value', () => {
        // Arrange
        const control = component.registerForm.get('password');

        // Act
        control.setValue('');

        // Assert
        expect(control.valid).toBeFalsy();
    });

    it('should mark password as invalid if value does not match a pattern', () => {
        // Arrange
        const control = component.registerForm.get('password');
        const invalidPassword = 'password';

        // Act
        control.setValue(invalidPassword);

        // Assert
        expect(control.valid).toBeFalsy();
    });

    it('should mark password as invalid if confirmPassword does not match password', () => {
        // Arrange
        const passwordControl = component.registerForm.get('password');
        const password = 'MyPassword111';
        const confirmPassword = 'NotMyPassword111';
        const confirmPasswordControl = component.registerForm.get('confirmPassword');

        // Act
        passwordControl.setValue(password);
        confirmPasswordControl.setValue(confirmPassword);

        // Assert
        expect(confirmPasswordControl.valid).toBeFalsy();
    });

    it('should return from function if form invalid', () => {
        // Arrange
        const control = component.registerForm.get('email');

        // Act
        control.setValue('');
        component.onSubmit();

        // Assert
        expect(postAuthSpy).not.toHaveBeenCalled();

    });

    it('should register new user', () => {
        // Arrange
        const user: IUser = { email: 'ferencrman@gmail.com', password: 'MyPassword111'};
        component.registerForm.get('email').setValue(user.email);
        component.registerForm.get('password').setValue(user.password);
        component.registerForm.get('confirmPassword').setValue(user.password);

        // Act
        component.onSubmit();

        // Assert
        expect(postAuthSpy).toHaveBeenCalledTimes(1);
    });

    it('must match password returns true', () => {
        // Arrange
        const validUser = { email: 'ferencrman@gmail.com', password: 'MyPassword111', confirmPassword: 'MyPassword111' };
        component.registerForm.get('email').setValue(validUser.email);
        component.registerForm.get('password').setValue(validUser.password);
        component.registerForm.get('confirmPassword').setValue(validUser.confirmPassword);

        // Act
        const result = component.mustMatch('password', 'confirmPassword');

        // Assert
        expect(component.registerForm.invalid).toBeFalsy();
    });

    it('must match password returns false', () => {
        // Arrange
        const invalidUser = { email: 'ferencrman@gmail.com', password: 'MyPassword111', confirmPassword: 'NotMyPassword111' };
        component.registerForm.get('email').setValue(invalidUser.email);
        component.registerForm.get('password').setValue(invalidUser.password);
        component.registerForm.get('confirmPassword').setValue(invalidUser.confirmPassword);

        // Act
        component.mustMatch('password', 'confirmPassword');

        // Assert
        expect(component.registerForm.invalid).toBeTruthy();
    });

});

