import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private http = inject(HttpClient);
  baseUrl = 'http://localhost:5054/api';

  //! NOTE: this does not send the request yet. This is a lazy (cold) Observable
  createUser(formData: any) {
    return this.http.post(this.baseUrl + '/signup', formData);
  }
}
