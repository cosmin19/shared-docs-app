import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AlertService, AuthenticationService } from '../_services/index';
import { AppConfig } from '../app.config';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';

@Component({
    templateUrl: 'login.component.html',
    styleUrls: ['login.component.css']
})

export class LoginComponent implements OnInit {

    loginForm: FormGroup;

    returnUrl: string;

    constructor(fb: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private _authService: AuthenticationService,
        private _alertService: AlertService,
        private _appConfig: AppConfig) 
    { 
        this.loginForm = fb.group({
            email: [null],
            password: [null]
        });
    }

    ngOnInit() {
        if(this._authService.isLoggedIn()) {
            this.router.navigate(['/customer/info']);
        }
        else {
            if (this._authService.isLoggedIn())
                this._authService.logout();
            this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
        }
    }

    get email() {
        return this.loginForm.get('email') as FormControl;
    }

    
    get password() {
        return this.loginForm.get('password') as FormControl;
    }

    submit() {
        this._authService.login(this.email.value, this.password.value, this.returnUrl);
    }

    clearForm(): void {
        this.loginForm.reset();
    }
}
