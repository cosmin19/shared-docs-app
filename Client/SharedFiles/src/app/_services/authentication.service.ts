import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map'
import { Subject } from 'rxjs/Subject';
import { tokenNotExpired, JwtHelper } from 'angular2-jwt';
import { AppConfig } from '../app.config';
import { BaseService } from './base.service';
import 'rxjs/add/operator/timeout'
import { catchError, retry, tap } from 'rxjs/operators';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { map } from 'rxjs/operator/map';
import { Router } from '@angular/router';

import { AlertService } from './alert.service';
import { ErrorObservable } from 'rxjs/observable/ErrorObservable';
import { RegisterDto } from '../_models/registerDto';
import { MessageDto } from '../_models/message';
import { ChangePassDto } from '../_models/changepass';
import { PostLoginDto } from '../_models/postLogin';
import { Loading } from '../_models/loading';


@Injectable()
export class AuthenticationService extends BaseService {

    /* ------------------------------ Variables ------------------------------ */
    private subject = new Subject<any>();
    loggedIn: boolean;
    private _baseUrl: string = '';

    /* ------------------------------ Ctor ------------------------------ */
    constructor(
        private http: HttpClient,
        private _alertService: AlertService,
        private router: Router,
        private _appConfig: AppConfig
    ) {
        super();

        this._baseUrl = _appConfig.baseUrl;

        /* Check if token is expired and sed isLoggedIn*/
        try {
            let user = localStorage.getItem('electron-crt-user');
            if (user) {
                let auth_token = JSON.parse(user)['token'];
                if (auth_token) {
                    this.loggedIn = !this._jwtHelper.isTokenExpired(auth_token);
                }
            }
        }
        catch (e) {
            this.loggedIn = false;
        }
    }

    login(email: string, password: string, returnUrl: string, loading: Loading): void {
        let bodyData = JSON.stringify({ username: email, password: password });
        this.http.post<any>(this._baseUrl + 'api/account/login', bodyData, this.jwt_content_type_json())
            .subscribe(
                data => {
                    let id = data.id;
                    let expires_in = data.expires_in;
                    let token = data.access_token.Result;
                    if (id && expires_in && token) {
                        let user = { id: id, token: token, expires_in: expires_in };
                        localStorage.removeItem('electron-crt-user');
                        localStorage.setItem('electron-crt-user', JSON.stringify(user));

                        this.loggedIn = true;
                        this.subject.next(true);

                        this.router.navigate([returnUrl]);
                        if (loading)
                            loading.loading = false;
                    }
                },
                error => {
                    if (loading)
                        loading.loading = false;
                    localStorage.removeItem('electron-crt-user');
                    var message = error.error.message;
                    this._alertService.error("Login", message);
                }
            );
    }

    /* ------------------------------ Logout ------------------------------ */
    logout() {
        localStorage.removeItem('electron-crt-user');
        this.loggedIn = false;
        this.subject.next(false);
        this.router.navigate(['/login']);
        return true;
    }

    /* ------------------------------ Register ------------------------------ */
    register(model: RegisterDto, loading: Loading): void {
        let bodyData = JSON.stringify(model);
        this.http.post<MessageDto>(this._baseUrl + 'api/account/register', bodyData, this.jwt_content_type_json())
            .subscribe(
                data => {
                    if (loading)
                        loading.loading = false;

                    var message = data.message;
                    this._alertService.success("Register", message);
                        
                    this.router.navigate(['/login']);
                },
                error => {
                    var message = error.error.message;
                    console.log(error)
                    this._alertService.error("Register", message);
                    if (loading)
                        loading.loading = false;
                }
            );
    }

    /* ------------------------------ Change password ------------------------------ */
    changePass(model: ChangePassDto): void {
        let bodyData = JSON.stringify(model);
        this.http.post<MessageDto>(this._baseUrl + 'api/client/changePassword', bodyData, this.jwt_auth_content_type_json())
            .subscribe(
                data => {
                    var message = data.message;
                    this._alertService.success("Change password", message);
                    this.router.navigate(['/customer/info']);
                },
                error => {
                    var message = error.message;
                    this._alertService.error("Change password", message);
                }
            );
    }

    /* ------------------------------ Is Logged In ------------------------------ */
    isLoggedIn(): boolean {
        return this.loggedIn;
    }

    /* ------------------------------ Get is authenticated ------------------------------ */
    getIsLoggedIn(): Observable<boolean> {
        return this.subject.asObservable();
    }
}