import { ReactiveFormsModule, FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { Component, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AppConfig } from '../app.config';
import { Client } from '../_models/client/index';

import { CustomValidators } from '../_directives';
import { EqualValidator } from './../_directives/validators/equal-validator.directive';
import { AuthenticationService } from '../services/authentication.service';
import { AlertService } from '../services/alert.service';

@Component({
    templateUrl: 'register.component.html',
    styleUrls: ['register.component.css']
})

export class RegisterComponent implements OnInit {

    registerForm: FormGroup;

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
            firstName: [null],
            lastName: [null],
        }, { validator: CustomValidators.CheckIfMatchingPasswords('password', 'confirmPassword') });

    }

    ngOnInit(): void {
    }

    submit(): void {
        let newClient = new Client();
            newClient.email = this.email.value;
            newClient.password = this.password.value;
            newClient.firstName = this.firstName.value;
            newClient.lastName = this.lastName.value;

            this._authService.register(newClient);
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
    get firstName() {
        return this.registerForm.get('firstName') as FormControl;
    }
    get lastName() {
        return this.registerForm.get('lastName') as FormControl;
    }
}
