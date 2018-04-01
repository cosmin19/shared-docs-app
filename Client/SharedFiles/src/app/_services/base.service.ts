
import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';
import { tokenNotExpired, JwtHelper } from 'angular2-jwt';


@Injectable()
export class BaseService {
    protected _jwtHelper: JwtHelper;
    constructor() {
        this._jwtHelper = new JwtHelper();
    }

    jwt_auth() {
        if(this.isTokenExpired()) {
            localStorage.removeItem('electron-crt-user');
        }

        let user = localStorage.getItem('electron-crt-user');
        if (user) {
            let auth_token = JSON.parse(user)['token'];
            if (auth_token) {

                let httpOptions = {
                    headers: new HttpHeaders({ 'Authorization': 'Bearer ' + auth_token })
                };
                return httpOptions;
            }
        }
        return null;
    }

    jwt_auth_text() {
        if(this.isTokenExpired()) {
            localStorage.removeItem('electron-crt-user');
        }
        
        let user = localStorage.getItem('electron-crt-user');
        if (user) {
            let auth_token = JSON.parse(user)['token'];
            if (auth_token) {

                let httpOptions = {
                    responseType: 'text',
                    headers: new HttpHeaders({ 'Authorization': 'Bearer ' + auth_token })
                };
                return httpOptions;
            }
        }
        return null;
    }


    jwt_content_type_json() {
        let httpOptions = {
            headers: new HttpHeaders({ 'Content-Type': 'application/json' })
        };
        return httpOptions;
    }

    jwt_auth_content_type_json() {

        if(this.isTokenExpired()) {
            localStorage.removeItem('electron-crt-user');
        }

        let user = localStorage.getItem('electron-crt-user');
        if (user) {
            let auth_token = JSON.parse(user)['token'];
            if (auth_token) {

                let httpOptions = {
                    headers: new HttpHeaders({
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + auth_token
                    })
                };
                return httpOptions;
            }
        }
        return null;
    }

    /* ------------------------------ Get Decoded Token ------------------------------ */
    getDecodedToken() {
        let user = localStorage.getItem('electron-crt-user');
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

    isTokenExpired() : boolean {
        let user = localStorage.getItem('electron-crt-user');
        if (user) {
            let auth_token = JSON.parse(user)['token'];
            if (auth_token) {
                return this._jwtHelper.isTokenExpired(auth_token);
            }
        }
        return true;
    }
}
