import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-registration',
  imports: [ReactiveFormsModule],
  templateUrl: './registration.html',
  styles: ``,
})
export class Registration {
  // constructor(public formBuilder: FormBuilder) {}
  private formBuilder = inject(FormBuilder); // to fix the error, inject manually

  //! formBuilder is used before init error when using a constructor approach
  form = this.formBuilder.group({
    fullName: [''],
    email: [''],
    password: [''],
    confirmPassword: [''],
  });

  onSubmit() {
    console.log(this.form.value);
  }
}
