import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';

@Injectable()
export class BaseService {
    constructor() {}

    jwt_auth() {
        let user = localStorage.getItem('currentUser');
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
        let user = localStorage.getItem('currentUser');
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
        let user = localStorage.getItem('currentUser');
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
}
