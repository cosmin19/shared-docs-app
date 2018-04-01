import { ReactiveFormsModule, FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';

import { AlertService, AuthenticationService } from '../_services/index';
import { AppConfig } from '../app.config';
import { SelectItem } from 'primeng/primeng';
import { CustomValidators } from '../_directives';
import { RegisterDto } from '../_models/registerDto';
import { Loading } from '../_models/loading';

@Component({
    templateUrl: 'register.component.html',
    styleUrls: ['register.component.css']
})

export class RegisterComponent implements OnInit, OnDestroy {

    registerForm: FormGroup;

    loading = new Loading();
    loadingUrl: string;


    countryList: SelectItem[];
    countyList: SelectItem[];
    cityList: SelectItem[];

    constructor(fb: FormBuilder,
        private router: Router,
        private _authService: AuthenticationService,
        private _alertService: AlertService,
        private _appConfig: AppConfig) 
    {
        if (_authService.isLoggedIn())
            this._authService.logout();

        this.registerForm = fb.group({
            email: [null],
            password: [null],
            confirmPassword: [null],
            username: [null],
            firstname: [null],
            lastname: [null],
        }, { validator: CustomValidators.CheckIfMatchingPasswords('password', 'confirmPassword') });
    }

    ngOnInit(): void {
        this.loadingUrl = this._appConfig.loadingGifUrl;
    }
    
    ngOnDestroy() {
        this.loading.loading = false;
    }

    submit(): void {
        this.loading.loading = true;
        let newClient = new RegisterDto();
        newClient.email = this.email.value;
        newClient.password = this.password.value;
        newClient.username = this.username.value;
        newClient.firstname = this.firstname.value;
        newClient.lastname = this.lastname.value;
        this._authService.register(newClient, this.loading);
    }

    clearForm(): void {
        this.registerForm.reset();
    }

    /* GETTERS */
    get email() {
        return this.registerForm.get('email') as FormControl;
    }
    get password() {
        return this.registerForm.get('password') as FormControl;
    }
    get confirmPassword() {
        return this.registerForm.get('confirmPassword') as FormControl;
    }
    get username() {
        return this.registerForm.get('username') as FormControl;
    }
    get firstname() {
        return this.registerForm.get('firstname') as FormControl;
    }
    get lastname() {
        return this.registerForm.get('lastname') as FormControl;
    }
}
