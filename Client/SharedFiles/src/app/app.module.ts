import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AppContentComponent } from './app-content.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { AuthenticationService } from './services/authentication.service';
import { AlertService } from './services/alert.service';
import { UserService } from './services/user.service';

@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        LoginComponent,
        RegisterComponent,
        AppContentComponent,
        PageNotFoundComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        ReactiveFormsModule
    ],
    providers: [
        AuthenticationService,
        AlertService,
        UserService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
