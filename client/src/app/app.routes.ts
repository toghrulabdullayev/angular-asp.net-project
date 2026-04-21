import { Routes } from '@angular/router';
import { User } from './user/user';
import { Registration } from './user/registration/registration';
import { Login } from './user/login/login';

export const routes: Routes = [
  {
    path: '',
    component: User,
    children: [
      { path: '', redirectTo: 'signup', pathMatch: 'full' },
      { path: 'signup', component: Registration },
      { path: 'login', component: Login },
    ],
  },
];
