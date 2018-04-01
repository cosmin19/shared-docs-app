import { Component, OnInit, ViewEncapsulation, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AlertService, AuthenticationService } from '../_services/index';
import { AppConfig } from '../app.config';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import * as $ from 'jquery';
import { Loading } from '../_models/loading';

@Component({
    moduleId: module.id,
    templateUrl: 'login.component.html',
    styleUrls: ['login.component.css']
})

export class LoginComponent implements OnInit, OnDestroy {

    loginForm: FormGroup;
    returnUrl: string;
    
    loading = new Loading();
    loadingUrl: string;

    constructor(fb: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private _authService: AuthenticationService,
        private _alertService: AlertService,
        private _appConfig: AppConfig
    ){ 
        this.loginForm = fb.group({
            email: [null],
            password: [null]
        });
    }

    ngOnInit() {
        this.loading.loading = false;
        this.loadingUrl = this._appConfig.loadingGifUrl;

        if(this._authService.isLoggedIn()) {
            this.router.navigate(['']);
        }
        else {
            if (this._authService.isLoggedIn())
                this._authService.logout();
            this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
        }
    }

    ngOnDestroy() {
        this.loading.loading = false;
    }

    submit() {
        this.loading.loading = true;
        this._authService.login(this.email.value, this.password.value, this.returnUrl, this.loading);
    }

    clearForm(): void {
        this.loginForm.reset();
    }

    /* GETTERS  */
    get email() {
        return this.loginForm.get('email') as FormControl;
    }
    
    get password() {
        return this.loginForm.get('password') as FormControl;
    }

    
}
