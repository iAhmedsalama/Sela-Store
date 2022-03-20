import { Injectable } from '@angular/core';
import { environment } from './../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, of, ReplaySubject } from 'rxjs';
import { IUser } from '../shared/models/user';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl;

  //create new BehaviorSubject of type IUser
  private currentUserSource = new ReplaySubject<IUser>(1);

  //create new Observable for current user
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  login(values: any) {
    return this.http.post(this.baseUrl + 'account/login', values).pipe(
      map((user: any): void => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    );
  }

  register(values: any) {
    return this.http.post(this.baseUrl + 'account/register', values).pipe(
      map((user: any) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  checkEmailExists(email: string) {
    return this.http.get(this.baseUrl + 'account/emailexists?email=' + email);
  }

  //add methods to persist the loged user if user have token
  loadCurrentUser(token: string) {
    if(token === null){
      this.currentUserSource.next(null);
      return of (null);
    }
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);

    return this.http.get(this.baseUrl + 'account', { headers }).pipe(
      map((user: any) => {
        if (user) {
          localStorage.setItem('token', user.token);

          //update observable with the user
          this.currentUserSource.next(user);
        }
      })
    );
  }

  // getCurrentUserValue() {
  //   return this.currentUserSource.value;
  // }
}
