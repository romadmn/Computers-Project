import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { IRegister } from 'src/app/core/models/register';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  @Output() onCancel: EventEmitter<void> = new EventEmitter<void>();
  public registerForm: FormGroup;
  submitted = false;
  loading = false;
  error = '';

  constructor(private formBuilder: FormBuilder, private userService: UserService) { }

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required,
        Validators.minLength(8),
        Validators.maxLength(16),
        Validators.pattern('^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$')]],
      confirmPassword: ['', [Validators.required,
        Validators.minLength(8),
        Validators.maxLength(16),
        Validators.pattern('^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$')]]
  }, {
      validator: this.mustMatch('password', 'confirmPassword')
  });
  }
  get form() { return this.registerForm.controls; }

  onSubmit() {
    this.submitted = true;

      if (this.registerForm.invalid) {
          return;
      }

      this.loading = true;
    const newUser: IRegister = {
      email: this.registerForm.get('email').value,
      password: this.registerForm.get('password').value,
      passwordConfirm: this.registerForm.get('confirmPassword').value,
    };
    this.userService.post(newUser).subscribe(() => {
      this.onCancel.emit();
    },
    error => {
        this.error = error;
        this.loading = false;
    });
  }

  cancel() {
    this.onCancel.emit();
  }

  mustMatch(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
        const control = formGroup.controls[controlName];
        const matchingControl = formGroup.controls[matchingControlName];

        if (matchingControl.errors && !matchingControl.errors.mustMatch) {
            return;
        }

        if (control.value !== matchingControl.value) {
            matchingControl.setErrors({ mustMatch: true });
        } else {
            matchingControl.setErrors(null);
        }
    }
}
}
