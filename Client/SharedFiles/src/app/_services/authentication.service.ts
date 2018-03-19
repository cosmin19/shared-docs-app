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


@Injectable()
export class AuthenticationService extends BaseService {

    /* ------------------------------ Variables ------------------------------ */
    private subject = new Subject<any>();
    loggedIn: boolean;
    private _baseUrl: string = '';
    private _jwtHelper: JwtHelper;

    /* ------------------------------ Ctor ------------------------------ */
    constructor(
        private http: HttpClient,
        private _alertService: AlertService,
        private router: Router,
        private _appConfig: AppConfig
    ) {
        super();

        this._jwtHelper = new JwtHelper();
        this._baseUrl = _appConfig.baseUrl;

        /* Check if token is expired and sed isLoggedIn*/
        try {
            let user = localStorage.getItem('currentUser');
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

    login(email: string, password: string, returnUrl: string): void {
        let bodyData = JSON.stringify({ username: email, password: password });
        this.http.post(this._baseUrl + 'login', bodyData, { observe: 'response', responseType: 'text', headers: this.jwt_content_type_json().headers })
            .subscribe(
                data => {
                    let userToken = data.headers.get('Authorization');
                    /* Dupa autentificare */
                    if (userToken) {
                        let auth_token = userToken.replace('Bearer ', '');
                        if (auth_token) {

                            let user = { user: email, token: auth_token };
                            localStorage.setItem('currentUser', JSON.stringify(user));
                            /*  Daca token-ul e valid si autentificarea s-a facut cu succes, 
                                fac al doilea request pentru a lua datele necesare din baza 
                            */
                            this.postLogin(email)
                                .subscribe(
                                    data => {
                                        data.token = auth_token;

                                        localStorage.removeItem('currentUser');
                                        localStorage.setItem('currentUser', JSON.stringify(data));

                                        this.loggedIn = true;
                                        this.subject.next(true);

                                        this.router.navigate([returnUrl]);
                                    },
                                    error => {
                                        localStorage.removeItem('currentUser');
                                        var message = error.message;
                                        this._alertService.error("Post login data", message);
                                    }
                                );
                        }
                    }
                },
                error => {
                    localStorage.removeItem('currentUser');
                    var message = error.message;
                    this._alertService.error("Login", message);
                }
            );
    }

    postLogin(email: string): Observable<PostLoginDto> {
        return this.http.get<PostLoginDto>(this._baseUrl + 'api/client/post-login?email=' + email, this.jwt_auth_content_type_json());
    }

    /* ------------------------------ Logout ------------------------------ */
    logout() {
        localStorage.removeItem('currentUser');
        this.loggedIn = false;
        this.subject.next(false);
        return true;
    }

    /* ------------------------------ Register ------------------------------ */
    register(model: RegisterDto): void {
        // register(model: Client): void {
        let bodyData = JSON.stringify(model);
        this.http.post<MessageDto>(this._baseUrl + 'api/client/register', bodyData, this.jwt_content_type_json())
            .subscribe(
                data => {
                    var message = data.message;
                    this._alertService.success("Register", message);
                    this.router.navigate(['/login']);
                },
                error => {
                    var message = error.message;
                    this._alertService.error("Register", message);
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

    /* ------------------------------ Get Decoded Token ------------------------------ */
    getDecodedToken() {
        let user = localStorage.getItem('currentUser');
        if (user) {
            let auth_token = JSON.parse(user)['token'];
            if (auth_token) {
                return this._jwtHelper.decodeToken(auth_token);
            }
        }
        return null;
    }

    getEmailFromToken(): string {
        let token = this.getDecodedToken();
        if (token["sub"])
            return token["sub"];
        return null;
    }
}