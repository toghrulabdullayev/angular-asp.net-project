import { Component, inject } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { FirstKeyPipe } from '../../shared/pipes/first-key-pipe';
import { Auth } from '../../shared/services/auth';

@Component({
  selector: 'app-registration',
  imports: [ReactiveFormsModule, FirstKeyPipe],
  templateUrl: './registration.html',
  styles: ``,
})
export class Registration {
  // constructor(public formBuilder: FormBuilder) {}
  private formBuilder = inject(FormBuilder); // to fix the error, inject manually
  private service: Auth = inject(Auth); // our very own auth service
  isSubmitted = false;

  passwordMatchValidator: ValidatorFn = (control: AbstractControl): null => {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    if (password && confirmPassword && password.value != confirmPassword.value) {
      confirmPassword?.setErrors({ passwordMismatch: true });
    } else {
      confirmPassword?.setErrors(null);
    }

    return null;
  };

  //! formBuilder is used before init error when using a constructor approach
  form = this.formBuilder.group(
    {
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: [
        '',
        [Validators.required, Validators.minLength(6), Validators.pattern(/(?=.*[^a-zA-Z0-9 ])/)],
      ],
      confirmPassword: [''],
    },
    { validators: this.passwordMatchValidator },
  );

  onSubmit() {
    this.isSubmitted = true;

    // this.service.createUser() builds an httpclient (Observable definition), .subscribe() actually sends the request
    if (this.form.valid) {
      this.service.createUser(this.form.value).subscribe({
        next: (res: any) => {
          if (res.succeeded) {
            this.form.reset();
            this.isSubmitted = false
          }
          console.log(res);
        },
        error: (err) => console.log('error', err),
      });
    }
  }

  hasDisplayableError(controlName: string): boolean {
    const control = this.form.get(controlName);
    return !!control?.invalid && (this.isSubmitted || !!control?.touched);
  }
}
